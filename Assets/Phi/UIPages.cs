using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Add this script to a control that has a box collider covering the area to be scrolled.
[SelectionBase]
public class UIPages : UIBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
	public class SetupPage
	{
		public int page;
		public UIPages pages;
		public object userData;
		public SetupPage(UIPages pages)
		{
			this.pages = pages;		
		}
	}

	public class PageOffset
	{
		public float interpolatePages;
		public Vector3 posOffset = Vector3.zero;
		public Vector3 scaleOffset = Vector3.one;
		public Quaternion rotOffset = Quaternion.identity;
	}

	public Dictionary<int, PageOffset> pageOffsets = null; //Allows you Touch offset the position OffMeshLink any UIPages index byte a number of pages.
    private RectTransform viewRect;

	
	[SerializeField]
	[HideInInspector]
	private float dampening = 50;
	[ExposeProperty]
	public float Dampening
	{
		get {return dampening;}
		set
		{
			dampening = value;
			if (ease)
			{
				ease.accelerate = dampening;
			}
		}
	}
    public bool disableHiddenPages = false;
	public int autopopulateSlots = 0;	
	public Vector3 autopopulateDelta = Vector3.zero;
	public float overshootDampening = 0;	// If 0 then will use the normal Dampening value.
    public bool useTransitionCurve = false;
    public AnimationCurve transitionCurve;
	
	public object userData;
	public delegate void SetUpPageDelegate(GameObject parent, int page);
	public SetUpPageDelegate setUpPageEvent = null; 
	
	public List<GameObject> pages = new List<GameObject>();	// Add parent game objects for the pages to be scrolled, only the number of pages that can be visible at once should be added 
    private int[] initialSibilingIndex;
    private int[] curSibilingIndex;
	public int currentPageSlotIndex = 0;	// Index into pages that the current page will occupy (usually 0)
	public Transform previousPage = null;
    public bool pagesOverlap = true; // When true the sibling order of pages will be kept ensuring render order remains the same.
	public bool sendSetupMessageToChildren = false;
	public bool snapToPages = true;


	public float maxOvershootInPages = 2.0f;
	
	public float numPagesVisibleAtOnce = 0; // How many pages can you see at once when the list is not being dragged
	public int minPage; //First page index
	public int numPages; //Number of pages 
	
	public float curPage;
	public float minThrowVelocity = 0.1f;
    public float headerSize = 0;

	private bool vertical = false;
	private float scrollSize = 0;	// World unit distance between two pages, used to define speed of scrolling relative to finger movement
	private int[] curPages = null;
	public Vector3[] pagePositions = null;
	private Vector3[] pageScales = null;
	private Quaternion[] pageRotations = null;
	private Vector3 hiddenPos = new Vector3(-10000.0f, -10000.0f, -10000.0f);
	private UtilsVelocityEase ease;
	private Vector2 startPosition;
//    private float startTime;
	private float dragPageVelocity;
	private float startPage;
//	private int extraPage = 0;	// -1 if previous page is ready +1 if next page is ready / populated
	private SetupPage setupPage = null;
	private float lastTime;
	private UtilsRollingAverage.RAFloat rollingAverage = new UtilsRollingAverage.RAFloat(2, 0);
	
	public System.Action<UIPages, float> onSetScrollPos = null;
	public System.Action<UIPages> onNumPagesChanged = null;
	public System.Action<UIPages, int> onShouldSkipPage = null;	// This will be called when rouding to a page, if you do not want to stop on the page set pages.shouldSkip to true.
    public float hideMargin = 0;
	public bool shouldSkip = false;
	private float sign = 1;
	private int prevNumPages = -1;	// Used to see if numPages has changed and send out numPagesChangeCallback when it does.
    protected override void Awake()
	{
        viewRect = transform as RectTransform;
		setupPage = new SetupPage(this);
		ease = gameObject.GetComponent<UtilsVelocityEase>();
		if (!ease)
		{
			ease = gameObject.AddComponent<UtilsVelocityEase>();
			ease.maxSpeed = 1000.0f;
			ease.accelerate = 30.0f;
		}
		ease.enabled = false;
		ease.userObject = this;
		ease.setValueDelegate = SetEaseDelegate;
		ease.mode = UtilsVelocityEase.Modes.Custom;
		if(autopopulateSlots >= 1)
		{
			AutoPopulatePages(autopopulateSlots, autopopulateDelta);
		}
	}
	
	// Use this for initialization
	protected override void Start () 
	{
		PageSlotsChanged();
	}
	
	private bool PageSlotsChanged()
	{
		if (curPages != null && curPages.Length == pages.Count)
		{
			// Already done this so don't do it again otherwise we may start storing moved positions
			return true;
		}
		if (pages.Count > currentPageSlotIndex + 1)
		{
			// Use the biggest offset along x and y axis to work out the scroll direction.
            Vector2 pos1 = ((pages[currentPageSlotIndex].transform) as RectTransform).anchoredPosition;
            Vector2 pos2 = ((pages[currentPageSlotIndex+1].transform) as RectTransform).anchoredPosition;
//.position;
//			Vector2 pos2 = pages[currentPageSlotIndex+1].transform.position;
			scrollSize = pos1.x - pos2.x;
			sign = 1;
			if (Mathf.Abs(scrollSize) <= Mathf.Abs(pos1.y - pos2.y))
			{
				vertical = true;
				scrollSize = pos1.y - pos2.y;
				if (scrollSize < 0)
				{
					sign = - 1;
					scrollSize = -scrollSize;
				}
			}
			else if (scrollSize < 0)
			{
				sign = - 1;
				scrollSize = -scrollSize;
			}
			// Set all pages out of range so they need to be updated

			pagePositions = new Vector3[pages.Count + 1];
			pageScales = new Vector3[pages.Count + 1];
			
			pageRotations = new Quaternion[pages.Count + 1];

			if (previousPage)
			{
				pagePositions[0] = previousPage.transform.localPosition;
				pageScales[0] 	 = previousPage.transform.localScale;
				pageRotations[0] = previousPage.transform.localRotation;
			}
			else
			{
				pagePositions[0] = pages[0].transform.localPosition - (pages[pages.Count - 1].transform.localPosition - pages[pages.Count - 2].transform.localPosition);
				pageScales[0] 	 = pages[0].transform.localScale - (pages[pages.Count - 1].transform.localScale - pages[pages.Count - 2].transform.localScale);
				pageRotations[0] = pages[0].transform.localRotation * (Quaternion.Inverse(pages[pages.Count - 1].transform.localRotation) * pages[pages.Count - 2].transform.localRotation);
			}

			curPages = new int[pages.Count];
            initialSibilingIndex = new int[pages.Count];
            curSibilingIndex = new int[pages.Count];
			for (int i = 0; i < curPages.Length; i++)
			{
//				Debug.Log("Pos "+i+"="+pages[i].transform.localPosition);
				curPages[i] = minPage - 10;
				pagePositions[i + 1] = pages[i].transform.localPosition;
				pageScales[i + 1] 	 = pages[i].transform.localScale;
				pageRotations[i + 1] = pages[i].transform.localRotation;
                curSibilingIndex[i] = initialSibilingIndex[i] = pages[i].transform.GetSiblingIndex();
			}
			
			SetScrollPos (minPage - headerSize);
		}
		else
		{
			return false;
		}
		return true;
	}
	
	private void CheckNumPages()
	{
		if (numPages != prevNumPages)
		{
			if(onNumPagesChanged != null)
			{
				onNumPagesChanged(this);
			}
			prevNumPages = numPages;
		}
	}
	
	public void OnPress()
	{
		if (ease.enabled)
		{
			//Debug.Log("Stop throw");
		}
		ease.enabled = false;
	}

    bool dragging = false;
	public void OnBeginDrag(PointerEventData eventData)
    {
//        Debug.Log("OnBeginDrag "+eventData.position+" "+this.IsActive());
		// Tell user we want to drag
//		user.Dragging = true;
//		startPosition = user.PressedDetails.hit.point;
        dragging = true;
        if (this.IsActive())
        {
//            startTime = Time.realtimeSinceStartup;
            startPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out startPosition);
            dragPageVelocity = 0.0f;
            //lastTime = Time.realtimeSinceStartup;
//            Debug.Log("StartPos "+eventData.position);
            dragPrevPage = dragCurPage = startPage = curPage;
            ease.enabled = false;
            rollingAverage.Start(0);
        }
	}

    public void OnDrag(PointerEventData data)
    {
        if(!dragging) return;
 //       Debug.Log("OnDrag "+data.position);
        Vector2 vector;
        if (this.IsActive() && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, data.position, data.pressEventCamera, out vector))
        {
            Vector2 deltaV = vector - startPosition;
            //Vector3 deltaV = Vector3.zero;// (user.HoverDetails.hit.point - user.PressedDetails.hit.point);// *scaleX;
            float delta = GetAxis(deltaV);
            //float prevPage = this.curPage;
            float pos = ApplyLimitScale(startPage + sign * delta / scrollSize);
            SetScrollPos(pos);
            //float now = Time.realtimeSinceStartup;
            //float deltaTime = now - lastTime;
            dragCurPage = curPage;
            //dragPageVelocity = (curPage - prevPage) / deltaTime;
            //rollingAverage.Add(dragPageVelocity);
            //lastTime = now;
            UpdateVelocity();
        }		
	}

    public void UpdateVelocity()
    {
        dragPageVelocity = (dragCurPage - dragPrevPage) / Time.deltaTime;
        dragPrevPage = dragCurPage;
        rollingAverage.Add(dragPageVelocity);
    }

    float dragPrevPage;
    float dragCurPage;
/*    public void LateUpdate()
    {
    }
*/
	
	private float ApplyLimitScale(float pos)
	{
        if (pos <= (minPage - headerSize) || numPages <= (numPagesVisibleAtOnce - headerSize))
		{
			pos -= (minPage - headerSize);
			pos *= 0.5f;
            pos += (minPage - headerSize);
		}
		else
		{
			float limit = minPage + numPages - numPagesVisibleAtOnce;
			if (pos > limit)
			{
				pos -= limit;
				pos *= 0.5f;
				pos += limit;
			}
		}
		return pos;
	}

    public void OnEndDrag(PointerEventData data)
    {        
        if(!dragging) return;
        dragging = false;
        OnDrag(data);
		float velocity = rollingAverage.Average;// If you don't want the rolling average replace with dragPageVelocity
		float page = curPage;
		
		// Calculate where constant deceleration would end up...
		
		float time = Mathf.Abs(velocity / dampening);
		
		float distance = time * velocity / 2;
		
		float endpoint = page + distance;
        
        //Debug.Log("Duration "+data.position);
		float sign = 1;
		if (snapToPages)
		{
			if (endpoint < 0)
			{
				sign = -1;
				endpoint = -endpoint;
			}
            if (Mathf.Abs(velocity) > minThrowVelocity)
            {
                if (distance > 0)
                {
                    endpoint = Mathf.Ceil(endpoint);
                }
                else
                {
                    endpoint = Mathf.Floor(endpoint);
                }
            }
            else
            {
                endpoint = Mathf.Round(endpoint);
            }
			page = endpoint * sign;
		}		
		else
		{
			page = endpoint;
		}
		
		page = Mathf.Min(minPage + numPages - numPagesVisibleAtOnce, page);
		page = Mathf.Max(minPage - headerSize, page);
		if (snapToPages)
		{
			if(onShouldSkipPage != null)
			{
				int prevPage;
				do
				{
					prevPage = (int)page;
					shouldSkip = false;
                    onShouldSkipPage(this, (int)page);
					if (shouldSkip)
					{
						page += sign;
						page = Mathf.Min(minPage + numPages - numPagesVisibleAtOnce, page);
						page = Mathf.Max(minPage - headerSize, page);
					}		
				} while (prevPage != page);
			}
		}
//		int iPage = Mathf.RoundToInt(page);
		ease.velocity = new Vector2(velocity,0);
//		Debug.Log("Throw "+dragPageVelocity+","+rollingAverage.Average);
		ScrollTo(page);
	}
		
	public void ScrollTo(float page)
	{
		if(page == curPage)
		{
			SetScrollPos(page);	// Calling code may rely on this being called evn though we are already there
			return;
		}
		
		ease.accelerate = dampening;
		
		// Work out if current velocity and dampening will cause overshoot and if so limit the amount of overshor by increasing the dampening 
		
		float time = Mathf.Abs(ease.velocity.x / dampening);		
		float distance = time * ease.velocity.x / 2;		
		float endpoint = curPage + distance;
		float requiredDistance = distance;
		
		if (endpoint < minPage - maxOvershootInPages - headerSize)
		{
            requiredDistance = minPage - maxOvershootInPages - page - headerSize;
		}
		else if(endpoint > minPage + numPages - numPagesVisibleAtOnce + maxOvershootInPages)
		{
			requiredDistance = minPage + numPages - numPagesVisibleAtOnce + maxOvershootInPages - page;
		}
		
		if (requiredDistance != distance)
		{
			// Calculate new dampening value to avoid too much overshoot
			time = requiredDistance * 2.0f / ease.velocity.x;
			ease.accelerate = Mathf.Abs(ease.velocity.x / time);
		}
		else
		{
			ease.accelerate = dampening;
		}			
		
		float dampenRate = dampening;
		
		if ((overshootDampening != 0)&&(endpoint + requiredDistance < minPage - headerSize || endpoint + requiredDistance > minPage + numPages - numPagesVisibleAtOnce))
		{
			dampenRate = overshootDampening;
		}
		
		if (ease.accelerate < dampenRate)
		{
			ease.accelerate = dampenRate;
		}
			
		ease.enabled = true;
		ease.current = new Vector2(curPage, 0.0f);
		ease.Begin(new Vector2(page, 0.0f), ease.velocity);
	}

    public void IncDecPage(int dir)
    {
        float curTarget = curPage;
        if (ease.enabled)
        {
            curTarget = ease.target.x;
        }
        curTarget = Mathf.Round(curTarget) + dir;
        
		curTarget = Mathf.Min(minPage + numPages - numPagesVisibleAtOnce, curTarget);
		curTarget = Mathf.Max(minPage - headerSize, curTarget);

        ScrollTo(curTarget);
    }

	void SetEaseDelegate(Vector2 pos, object obj)
	{
//		UIPages TFPages = obj as UIPages;
		SetScrollPos(pos.x);
	}
	
	public void ResendOnSetUpPage(int index = -1)
	{
		if (curPages == null) return;
		int maxPage = Mathf.Max (minPage, minPage + numPages);
		for (int i = 0; i < pages.Count; i++)
		{
			int page = curPages[i];
			if ((index == -1)||(index == page))
			{
				bool visible = page >= minPage && page <= maxPage;
				if (visible)
				{			
					// Needs setting up
					SendOnSetUpPage(i,page);				
				}
				if(index != -1)
				{
					return;
				}
			}
		}
		CheckNumPages();
	}

	private void SendOnSetUpPage(int i, int page)
	{
		if (setUpPageEvent != null)
		{
			setUpPageEvent.Invoke(pages[i], page);
		}
		else
		{
			setupPage.page = page;			
			setupPage.userData = userData;
			if (sendSetupMessageToChildren)
			{
				pages[i].BroadcastMessage("OnSetUpPage", setupPage, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				pages[i].SendMessage("OnSetUpPage", setupPage, SendMessageOptions.DontRequireReceiver);
			}
		}
		curPages[i] = page;
	}
	
	public float RangeCheck(float page)
	{
        if (page <= minPage - -headerSize || numPages <= numPagesVisibleAtOnce - headerSize) return minPage -headerSize;
		if (page > minPage + numPages - numPagesVisibleAtOnce) return minPage + numPages - numPagesVisibleAtOnce;
		return page;
	}

    private void PlayChangeAudio(bool onEnter, float currPage, float prevPage)
    {
        if(onEnter)
        {
            return;
        }

        int intCurrPage = Mathf.RoundToInt(currPage);
        int intPrevPage = Mathf.RoundToInt(prevPage);

        if ((intCurrPage >= 0 && intCurrPage < numPages) && (intPrevPage >= 0 && intPrevPage < numPages) && (intCurrPage != intPrevPage))
        {
//				SoundMgr.DialClickSound();
        }
    }
	
	public void SetScrollPos(float pos, bool onEnter = false)
	{
		if (curPages == null || curPages.Length != pages.Count)
		{
			if (!PageSlotsChanged())
			{
				return;
			}
		}
        CheckNumPages();

        PlayChangeAudio(onEnter, curPage, pos);
        curPage = pos;
		if (onSetScrollPos != null)
		{
			onSetScrollPos.Invoke(this, pos);
		}
		int firstPage = Mathf.FloorToInt(pos);
		int offset = Mathf.RoundToInt(minPage);		// 1st page shows minPage
        float ratio = 1.0f - (pos - (float)firstPage);
        int sibOffset = ratio < 0.5 ? -1 : 0;
//        Debug.Log("ratio = " + ratio);
        
        for (int i = 0; i < pages.Count; i++)
		{
			int setToPage = firstPage + i - currentPageSlotIndex;
			int page = (setToPage + offset) % pages.Count;
			if (page < 0)
			{
				page += pages.Count;
			}
			//page = i;
            int sibI = (i + sibOffset).Clamp(0, initialSibilingIndex.Length - 1);
            //pages[page].transform.SetSiblingIndex(initialSibilingIndex[sibI]);
			bool visible = (setToPage + offset) >= minPage && setToPage < minPage + numPages;
            //Debug.Log(page + ")" + (i + ratio - 1) +", "+(numPages - hideMargin));
            if ((i + ratio - 1 < hideMargin) || (i + ratio) > (pages.Count - hideMargin))
            {
                visible = false;
            }
			if (visible)
            {
                if (pagesOverlap && pages[page].transform.GetSiblingIndex() != initialSibilingIndex[sibI])
                {
                    pages[page].transform.SetSiblingIndex(initialSibilingIndex[sibI]);
                }
                if (disableHiddenPages)
                {
                    if (!pages[page].activeSelf)
                    {
                        pages[page].SetActive(true);
                    }
                }
				if (curPages[page] != setToPage)
				{
					// Needs setting up
					SendOnSetUpPage(page,setToPage);
                }
                float ratio2 = ratio;
                PageOffset o = null;
				if (pageOffsets != null && pageOffsets.ContainsKey(setToPage))
				{
					o = pageOffsets[setToPage];
					if(o.interpolatePages != 0)
					{
						ratio2 += o.interpolatePages;
                    }
                }
                else if (useTransitionCurve)
                {
                    float transition = setToPage - firstPage - 1 + ratio;
                    if (transitionCurve.keys[0].time <= transition && transitionCurve.keys[transitionCurve.length - 1].time >= transition)
                    {
                        ratio2 = (transitionCurve.Evaluate(transition)) - (setToPage - firstPage - 1);
//                        Debug.Log("Evaluate " + setToPage + ") " + transition + " ratio = " + ratio + " ratio2= " + ratio2 + " transition=" + transition);
                    }
                }
                /*
                    ratio2 = (ratio - 0.5f) * 2;
                    if (ratio2 < 0)
                    {
                        ratio2 = 0;
                    }
                }

                else if (setToPage == firstPage+1)
                {
                    ratio2 = (ratio) * 2;
                    if (ratio2 > 1)
                    {
                        ratio2 = 1;
                    }
                }*/
                if (ratio2 != ratio)
                {
                    int i2 = i;
					if (ratio2 > 1)
					{
						float fullpages = Mathf.Floor(ratio2);
						ratio2 -= fullpages;
						i2 += (int)fullpages;
						if (i2 >= pagePositions.Length - 1)
						{
							i2 = pagePositions.Length - 2;
							ratio2 = 1;
						}
					}
					else if (ratio2 < 0)
					{							
						float fullpages = Mathf.Floor(1 - ratio2);
						ratio2 += fullpages;
						i2 -= (int)fullpages;
						if (i2 < 0)
						{
							i2 = 0;
							ratio2 = 0;
						}
					}
                    if (o != null)
                    {
                        pages[page].transform.localPosition = Vector3.Lerp(pagePositions[i2], pagePositions[i2 + 1], ratio2) + o.posOffset;
                        pages[page].transform.localScale = Vector3.Scale(Vector3.Lerp(pageScales[i2], pageScales[i2 + 1], ratio2), o.scaleOffset);
                        pages[page].transform.localRotation = Quaternion.Lerp(pageRotations[i2], pageRotations[i2 + 1], ratio2) * o.rotOffset;
                    }
                    else
                    {
                        pages[page].transform.localPosition = Vector3.Lerp(pagePositions[i2], pagePositions[i2 + 1], ratio2);
                        pages[page].transform.localScale = Vector3.Lerp(pageScales[i2], pageScales[i2 + 1], ratio2);
                        pages[page].transform.localRotation = Quaternion.Lerp(pageRotations[i2], pageRotations[i2 + 1], ratio2);
                    }
				}
				else
				{
					pages[page].transform.localPosition = Vector3.Lerp(pagePositions[i], pagePositions[i+1], ratio);
					pages[page].transform.localScale = Vector3.Lerp(pageScales[i], pageScales[i+1], ratio);
					pages[page].transform.localRotation = Quaternion.Lerp(pageRotations[i], pageRotations[i+1], ratio);
				}
			}
			else
			{
                if (disableHiddenPages)
                {
                    if (pages[page].activeSelf)
                    {
                        pages[page].SetActive(false);
                    }
                }
                else
                {
                    pages[page].transform.position = hiddenPos;
                }
			}
		}
	}

	public float GetAxis(Vector2 vec)
	{
		if (vertical)
		{
			return vec.y;
		}
		else
		{
			return vec.x;
		}
	}
	
	// Set cache so that all pages need updating.
	public void ResetPages()
	{
		if (curPages == null) return;
		for (int i = 0; i < curPages.Length; i++)
		{
			curPages[i] = minPage - 10;
		}		
		CheckNumPages();
	}
	
	public void AutoPopulatePages(int slots, Vector3 delta)
	{
		// First work out wether we want to use item 0 in the list or item currentPageSlotIndex
		
		int copyFrom = 0;
		if (pages.Count > currentPageSlotIndex)
		{
			copyFrom = currentPageSlotIndex;
		}
		Vector3 scale = pages[copyFrom].transform.localScale;
		Quaternion rot = pages[copyFrom].transform.localRotation;
		Vector3 pos = pages[copyFrom].transform.localPosition;
		if (pagePositions.Length > copyFrom + 1)
		{
			pos = pagePositions[copyFrom + 1];
			rot = pageRotations[copyFrom + 1];
			scale = pageScales[copyFrom + 1];
		}
		
		pos -= delta * currentPageSlotIndex;
		
		int neededSlots = slots + currentPageSlotIndex * 2;	// If we have prevous slots then assume we need them after the list too.
		
		string name = pages[copyFrom].name;
		int lastUnderscore = name.LastIndexOf('_');
		if (lastUnderscore > 0)
		{
			name = name.Substring(0,lastUnderscore);
		}
		
		List<GameObject> done = new List<GameObject>();
		done.Add(pages[copyFrom]);
		for(int i = 0; i < neededSlots; i++)
		{
			if (pages.Count <= i)
			{
				pages.Add(null);
			}
			if ((i != copyFrom) && (!pages[i] || done.Contains(pages[i])))
			{
/*#if UNITY_EDITOR				
				UnityEditor.PrefabType pType = UnityEditor.PrefabUtility.GetPrefabType(pages[0]);
				if (pType == UnityEditor.PrefabType.PrefabInstance || pType == UnityEditor.PrefabType.ModelPrefabInstance)
				{	
					Object prefab = UnityEditor.PrefabUtility.GetPrefabParent(pages[0]);
					pages[i]  = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				}
				else
#endif*/
				{
					pages[i] = Instantiate(pages[copyFrom]) as GameObject;
				}
				pages[i].transform.SetParent(pages[copyFrom].transform.parent);
			}
			pages[i].transform.localPosition = pos;
			pages[i].transform.localScale = scale;
			pages[i].transform.localRotation = rot;
			pages[i].name = name+"_"+i;
			done.Add(pages[i]);
			pos = pos + delta;
		}
		PageSlotsChanged();
	}
}
