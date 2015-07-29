
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialHandler : MonoBehaviour
{
    public static System.Action<string> FireEvents = null;   // Items can subscribe to this to be told when the tutorial system fires an event.
    public static System.Action<string> ShouldSkipLesson = null;   // If you want to implement a skip handler sign up to this event and set TutorialHandler.ShouldSkip to true if you want to skip the lesson. Note the passed string is the skip value within a lesson
    public static bool ShouldSkip = false;
    static private Tutorial[] Tutorials
    {
        get { return TutorialReader.Instance.Tutorials; }
    }
    private bool m_Inited;
    public bool Inited { get { return m_Inited; } }

    //-Gameobject Stuff -------
    [SerializeField]
    private GameObject m_TutorialUIParent;
    [SerializeField]
    private GameObject m_TutorialBlueWindow;
    [SerializeField]
    private Animator m_WormAnimator;
    [SerializeField]
    private TextMeshProUGUI m_TutorialText;
    [SerializeField]
    private Button m_NextButton;
    [SerializeField]
    private GameObject m_WormPosition;

    private bool m_Locked;

    public bool Lock
    {
        get { return m_Locked; }
        set { m_Locked = value; }
    }

    private float m_BlockTimer = 0;
    private const float TIME_TO_BLOCK = 5f;

    //-Start Conditions indexed by ID!!!
    // Change to list of names that can start to avoid confusion between tutorial index and id
    // If a tutorial name is here it can be started
    private List<string> m_StartConditions = new List<string>();
    
    public void SetTutorialCondition(string name, bool value)
    {
        if (value)
        {
            if (!m_StartConditions.Contains(name))
            {
                m_StartConditions.Add(name);
            }
        }
        else
        {
            if (m_StartConditions.Contains(name))
            {
                m_StartConditions.Remove(name);
            }
        }
    }

	public GameObject zoomHand;
	public GameObject statsHand;
	public GameObject foodHand;


    private bool m_PlayingTutorial, m_EndingTutorial;
	static public Tutorial CurrentTutorial
	{
		get
		{
			return m_CurTutorial;
		}
	}
    static private Tutorial m_CurTutorial = null;
	private int m_Letter_i, m_Lesson_i, m_Entry_i, m_FullLength;

    private const string TutorialPlayerPrefID = "TUTORIALS_COMPLETED";

    static public bool CheckTutsCompleted(string name)
    {
        return TutorialsCompleted.Contains(name);
    }

    // Check all completed tutorials to see if they contain an tutorial entry that fired te named event
    // Note this is not optimal but should be called infrequently enough not to matter.
    static public bool CheckTutorialContainingEventCompleted(string eventName)
    {
        // Go through the tutorials and see if they contain the named event
        List<string> tutNames = TutorialsCompleted;
        for (int i = tutNames.Count - 1; i >= 0; i--)
        {
            Tutorial t = GetTutorialByName(tutNames[i]);
            if (t != null)
            {
                for (int j = t.Lessons.Length - 1; j >= 0; j--)
                {
                    Lesson l = t.Lessons[j];
                    if(l != null)
                    {
                        for (int k = l.TutEntries.Length -1; k >= 0; k--)
                        {
                            TutEntry te = l.TutEntries[k];
                            if(te != null)
                            {
                                if (te.fire == eventName)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    static public Tutorial GetTutorialByName(string name)
    {    
        // Could create and use a dictionary if need be.
        for (int i = 0; i < Tutorials.Length; i++)
        {
            if(Tutorials[i].Name == name)
            {
                return Tutorials[i];
            }
        }
        return null;
    }

    private static List<string> TutorialsCompleted {
        get { return ProfilesManagementScript.Instance.CurrentProfile.TutorialsCompleted; }
        set {ProfilesManagementScript.Instance.CurrentProfile.TutorialsCompleted = value; }
    }
				
    private float m_Timer;
    private string m_TutorialCountingDown = "";
    private bool /*the secret to comedy*/ m_IsTiming;

	public static bool hasTapped;

    private void SetTimerOnTutorial(string tutName, float time)
    {
        if (time <= 0)
        {
            // No need to time just trigger the thing
            SetTutorialCondition(tutName, true);
        }
        else
        {
            Debug.Log("Setting Timer. Tut : [" + tutName + "]; Time : [" + time + "];");
            m_TutorialCountingDown = tutName;
            m_Timer = time;
            m_IsTiming = true;
        }
    }

    private void TurnOffTimer()
    {
        m_TutorialCountingDown = "";
        m_Timer = 0;
        m_IsTiming = false;
    }

    public bool IsPlaying
    {
        get
        {
            return (m_PlayingTutorial || m_EndingTutorial);
        }
    }
							
    // Use this for initialization
    void Start(){

        TutorialHandler.FireEvents += EventFired;
        ShouldSkipLesson += OnShouldSkipLesson;
        Init();

        // Avoid getting stuck if user exits during the eatStrawberry tutorial
        if(!CheckTutsCompleted("EatStrawberry") && ProfilesManagementScript.Instance.CurrentProfile.m_ShownStartEatStrawberry)
        {
            MarkTutorialComplete("EatStrawberry");
        }

    }
    public void Init()
    {
        if (m_Inited)
            return;

        m_TutorialUIParent.SetActive(true);
        m_TutorialUIParent.transform.localScale = Vector3.zero;
        m_Inited = true;
        m_WormAnimator.gameObject.SetActive(false);
        ShouldBeVisible = false;

        if (TutorialsCompleted == null)
            TutorialsCompleted = new List<string>();
        m_StartConditions.Clear();

        // Start wake up tutorial first time they come back to the game.
        if (ProfilesManagementScript.Instance.CurrentAnimin.Hatched && !CheckTutsCompleted("WakeUp"))
        {
            SetTutorialCondition("WakeUp", true);
        }

        // Flag any tutorials that have initial set so that they can be started, also if their trigger depended on a timer after a previous tutorial
        // was completed and that was completed in a previous session then restart the timer...

        for (int i = 0; i < Tutorials.Length; i++)
        {
            Tutorial t = Tutorials[i];
            if(CheckTutsCompleted(t.Name))
            {
                continue;
            }
            Condition c = t.Condition;
            if (c != null)
            {
                if (c.Initial != null)
                {
                    SetTutorialCondition(t.Name, true);
                }
                else if (c.Timer != null)
                {
                    string triggerName = c.Timer.trigger;
                    if (CheckTutsCompleted(triggerName))
                    {
                        SetTimerOnTutorial(t.Name, c.Timer.secf);
                    }
                }

                /*
                if (Tutorials[i].Condition.ButtonCond != null)
                {
                    ButtonCond buttcond = Tutorials[i].Condition.ButtonCond;
                    GameObject go = GameObject.Find(buttcond.name);
                    if (go != null)
                    {
                        if (go.GetComponent<Button>() != null && go.GetComponent<BoxCollider>() != null)
                        {
                            //UIEventListener.Get(go).onClick += OnTutorialStartClick;
                        }
                    }
                }
                */
            }
        }
    }
	
    public void BeginBlock(){
        m_BlockTimer = TIME_TO_BLOCK;
    }

    bool ShouldBeVisible
    {
        get
        {
            return m_ShouldBeVisible;
        }
        set
        {
            m_ShouldBeVisible = value;
            UpdateVisibility();
        }
    }

    static int m_Hidden = 0;
    bool m_ShouldBeVisible = false;
    static public void Hide(bool hide)
    {
        m_Hidden += hide ? 1 : -1;
        if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
            UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.UpdateVisibility();
    }

    bool isVisible = false;
    Tweener visTween = null;

    void UpdateVisibility()
    {
        bool newVis = m_ShouldBeVisible && m_Hidden <= 0;
        if (newVis != isVisible)
        {
            if (visTween != null)
            {
                visTween.Kill();
            }
            float duration = 0.5f;
            if (newVis)
            {
                DOTween.defaultEaseType = Ease.OutBounce;
            }
            else
            {
                DOTween.defaultEaseType = Ease.InCirc;
                duration = 0.25f;
            }
            visTween = m_TutorialUIParent.transform.DOScale(newVis ? 1 : 0, duration);
            isVisible = newVis;
            if(newVis)
            {
                AudioController.Play("Andy_Wormhole_Talk");
            }
            else
            {
                AudioController.Stop("Andy_Wormhole_Talk", 0.25f);
            }
        }
    }

    void OnDisable()
    {
        AudioController.Stop("Andy_Wormhole_Talk", 0.25f);
    }

    bool ignoreChangeTween = false;
    // Update is called once per frame


    void Update()
    {
        if (m_BlockTimer > 0)
        {
//            Debug.Log("IsBlocking : [" + (int)m_BlockTimer + "];");
            m_BlockTimer -= Time.deltaTime;
            if (m_BlockTimer <= 0)
            {
                m_BlockTimer = 0;
            } else
                return;
        }
        if (!m_PlayingTutorial)
        {
			if (!m_Locked)
            {
                for (int i = 0; i < Tutorials.Length; i++)
                {
                    if (CheckStartCondition(Tutorials[i].Name))
                    {
                        if (m_TutorialUIParent == null)
                            return;
						if (m_Hidden > 0)
                            return;

                        SetTutorialCondition(Tutorials[i].Name, false); // No need to start it anymore as we have now started 

						m_TutorialText.text = "";

                        ignoreChangeTween = !ShouldBeVisible;   // If we were not visible then we will tween on and do not want another pulse within that
                        ShouldBeVisible = true;
                        m_WormAnimator.gameObject.SetActive(true);
                        //Blocker.gameObject.SetActive(true);
                        //Block(true);
                        m_WormAnimator.SetTrigger("worm_GoOut");
                        m_CurTutorial = Tutorials[i];
                        m_Letter_i = 0;
                        m_Lesson_i = 0;
                        m_Entry_i = 0;

                        //MakeScreensVisible(new GameObject[]{ UIGlobalVariablesScript.Singleton.CaringScreenRef });
                        m_PlayingTutorial = true;

						// If hatched and not playing the wakeup tutorial wake animin up
						if(ProfilesManagementScript.Instance.CurrentAnimin.Hatched && m_CurTutorial.Name != "WakeUp")
						{
							CharacterProgressScript cp = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
							if (cp.IsSleeping)
							{
								// Ensure we are awake!
								cp.exitSleep();
							}
						}


                        ConsiderSkipLesson();
                        break;
                    }
                }
            }
        }
        else if (!m_EndingTutorial && m_CurTutorial != null)
        {
            if (m_Entry_i >= m_CurTutorial.Lessons[m_Lesson_i].TutEntries.Length)
                NextLesson();

			if (m_FullLength >= m_Letter_i)
            {
                if (m_Letter_i == 0)
                {
					
					string text = m_CurTutorial.Lessons[m_Lesson_i].TutEntries[m_Entry_i].text.Replace ("T-Bo", ProfilesManagementScript.Instance.CurrentAnimin.AniminName);

					if(!ProfilesManagementScript.Instance.CurrentAnimin.IsMale)
					{						
						text = text.Replace(" him ", " her ");
						text = text.Replace(" him.", " her.");
						text = text.Replace(" he ", " she ");
						text = text.Replace(" he's ", " she's ");
						text = text.Replace(" his ", " her ");
					}


					m_FullLength = text.Length;
					m_TutorialText.text = text;
					m_TutorialText.maxVisibleCharacters = 1;

                    AudioController.Play("Andy_Wormhole_Talk");
                    // Starting to show new entry so fire it's event
                    FireEvents(m_CurTutorial.Lessons[m_Lesson_i].TutEntries[m_Entry_i].fire);
                    if (!ignoreChangeTween)
                    {
                        m_TutorialBlueWindow.GetComponent<DOTweenAnimation>().DORestart();
                    }
                    ignoreChangeTween = false;
                }
                //Debug.Log("DeltaTime : [" + Time.deltaTime + "]; Multiplied : [" + Time.deltaTime * 100 + "];");
				int extraLetters = Mathf.CeilToInt(Mathf.Max(Time.deltaTime * 50f));
				m_Letter_i += extraLetters;
				m_TutorialText.maxVisibleCharacters = m_Letter_i;
                m_NextButton.gameObject.SetActive(false);
            }
            else
            {
//                if (!WaitingForInput)
                {
                    bool showNextButton = true;
                    if(m_Entry_i == m_CurTutorial.Lessons[m_Lesson_i].TutEntries.Length - 1)
                    {
                        // On last entry in lesson so check for an exit condition
                        Condition c = m_CurTutorial.Lessons[m_Lesson_i].EndCondition;
                        if (c != null && (c.AdHocCond == null || c.ButtonCond == null))
                        {
                            // Need to wait for an action
                            showNextButton = false;
                        }
                    }
                    m_NextButton.gameObject.SetActive(showNextButton);
                }
            }
        }
        else
        {
            if (m_WormAnimator.GetCurrentAnimatorStateInfo(0).IsName("worm_hidden"))
            {
                m_WormAnimator.gameObject.SetActive(false);
                m_PlayingTutorial = false;
                m_EndingTutorial = false;
                BeginBlock();
            }
        }
		if (m_IsTiming)
        {
//            Debug.Log("Timing : [" + ((int)m_Timer) + "];");
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime;
            else
            {
                Debug.Log("Turning off timer. Cond : [" + m_TutorialCountingDown + "] = true;");

                SetTutorialCondition(m_TutorialCountingDown, true);
                TurnOffTimer();
            }
        }

		if(m_WormPosition)
		{
			m_WormPosition.transform.position = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.05f, -0.1f, (GetComponent<Camera>().farClipPlane+GetComponent<Camera>().nearClipPlane)/2));
		}
    }		

    //This method test whether or not to start the tutorial.
    private bool CheckStartCondition(string name)
    {
        // Should not need to check this as tuts should not be started if they have been played unless we want to show them again.
        //if (CheckTutsCompleted(name))
        //    return false;
		if(!ProfilesManagementScript.Instance.CurrentAnimin.Hatched)
		{
			if (name != "Initial")
			{
				return false;
			}
		}

        return m_StartConditions.Contains(name);
    }



    [System.Obsolete("Use TriggerAdHoc() instead all triggers can trigger enter or exit conditions to allow flexibility with moving them about in the xml")  ]
    public void TriggerAdHocStartCond(string call)
    {
        TriggerAdHoc(call);
    }

    public static void TriggerAdHocStatic(string call)
    {
        if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
            UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc(call);
    }

    public static void CallPhone()
    {
        if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
            UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.DoCallPhone();
    }

    public void DoCallPhone()
    {
        if(m_PlayingTutorial) return;   // No need to show anything as current tutorial will popup.
        if(m_IsTiming)
        {
            m_Timer = 0;
        }
        m_BlockTimer -= TIME_TO_BLOCK - 1;
        if (m_BlockTimer < 0)
        {
            m_BlockTimer = 0;
        }
        if(m_StartConditions.Count == 0)
        {
            // Add a phone tutorial
            ProfilesManagementScript.Instance.CurrentProfile.m_PhoneMessage++;
            string tutName = "Phone"+ProfilesManagementScript.Instance.CurrentProfile.m_PhoneMessage;
            if(GetTutorialByName(tutName) == null)
            {
                // Reached end
                tutName = "Phone1";
                ProfilesManagementScript.Instance.CurrentProfile.m_PhoneMessage = 1;
            }
            SetTutorialCondition(tutName, true);
        }

    }

    Condition GetCurrentExitCondition()
    {
        if (m_CurTutorial != null && m_Lesson_i < m_CurTutorial.Lessons.Length)
        {
            return m_CurTutorial.Lessons[m_Lesson_i].EndCondition;
        }
        return null;
    }

    void TriggerCurExitCondition()
    {
        NextLesson();
    }

    public void TriggerAdHoc(string call)
    {
        Init();
        // Check exit condition of current lesson first
        Condition c = GetCurrentExitCondition();
        if (c != null)
        {
            if (c.AdHocCond != null)
            {
                if (c.AdHocCond.call == call)
                {
                    TriggerCurExitCondition();
                }
            }
        }

/*        if (IsPlaying)
            return;*/

        // Look for start conditions
        for (int i = 0; i < Tutorials.Length; i++)
        {
            string name = Tutorials[i].Name;
            if (CheckTutsCompleted(name) == true)
                continue;
            if (Tutorials[i].Condition == null)
                continue;
            if (Tutorials[i].Condition.AdHocCond == null)
                continue;
            if (Tutorials[i].Condition.AdHocCond.call == call)
            {
                SetTutorialCondition(name, true);
            }
        }
    }


    // Note this can now trigger exit conditions too!
    [System.Obsolete("Use TriggerButton(go) instead as triggers are now universal across start and exit events")]
    public void OnTutorialStartClick(GameObject go)
    {
        TriggerButton(go);
    }

    public void TriggerButton(GameObject go)
    {
        Condition c = GetCurrentExitCondition();
        if (c != null)
        {
            if (c.ButtonCond != null)
            {
                if (c.ButtonCond.name == go.name)
                {
                    TriggerCurExitCondition();
                }
            }
        }

//        if (!IsPlaying)
        {
            for (int i = 0; i < Tutorials.Length; i++)
            {
                string name = Tutorials[i].Name;
                if (CheckTutsCompleted(name) == true)
                    continue;
                if (Tutorials[i].Condition == null)
                    continue;
                if (Tutorials[i].Condition.ButtonCond == null)
                    continue;
                Debug.Log("Checking GameObject : [" + go.name + "] against : [" + Tutorials[i].Condition.ButtonCond.name + "];");
                if (go.name == Tutorials[i].Condition.ButtonCond.name)
                {
                    SetTutorialCondition(name, true);
                }
            }
        }
    }

    //- EXIT Conditions ----------------------------------------------------------------

    [System.Obsolete("Use TriggerButton(go) instead as triggers are now universal across start and exit events")]
    public void OnTutorialEndClick(GameObject go)
    {
        TriggerButton(go);
    }
	
    //- End of Lesson Processing ----------------------------------------------------------------
    public void NextButtonPress(bool ignoreCheck = false)
    {
        if (!m_NextButton.gameObject.activeInHierarchy)
            return;
        int maxEntries = 0;
        if (m_CurTutorial != null)
        {
            Lesson CurrentLesson = m_CurTutorial.Lessons[m_Lesson_i];
            maxEntries = CurrentLesson.TutEntries.Length;
        }


        Debug.Log("Testing Entry : [" + m_Entry_i + ":" + maxEntries + "]");
		
        m_Letter_i = 0;
        m_Entry_i += 1;

        if (m_Entry_i >= maxEntries)
        {
            NextLesson();
        }
    }

    //----Load next lesson------------------------------------------------
    private void NextLesson()
    {
        m_Letter_i = 0;
        m_Entry_i = 0;
        int maxLessons = 0;
        if (m_CurTutorial != null)
        {
            maxLessons = m_CurTutorial.Lessons.Length;
        }

        if (FireEvents != null)
        {
            FireEvents("NextLesson");
        }
		
        //Debug.Log ("maxLessons : [" + maxLessons + "];");
		
		if (++m_Lesson_i >= maxLessons || (CurrentTutorial != null && CheckTutsCompleted(CurrentTutorial.Name)))
        {
            EndOfTutorial();
        }
        else
        {
            ConsiderSkipLesson();
        }
    }

    void ConsiderSkipLesson()
    {
        ShouldSkip = false;
        if(m_CurTutorial != null)
        {
            Lesson lesson = m_CurTutorial.Lessons[m_Lesson_i];
            if (ShouldSkipLesson != null && lesson.skip != null && lesson.skip.Length > 0)
            {
                ShouldSkipLesson(lesson.skip);
            }
        }
        else
        {
            ShouldSkip = true;
        }
        if (ShouldSkip)
        {
            NextLesson();
        }
    }

    void MarkTutorialComplete(string name, bool triggerFollowOn = true)
    {
        if (!TutorialsCompleted.Contains(name))
        {
            TutorialsCompleted.Add(name);
        }

        if (triggerFollowOn)
        {
            for (int i = 0; i < Tutorials.Length; i++)
            {
                if (Tutorials[i].Condition != null)
                {
                    if (Tutorials[i].Condition.Timer != null)
                    {
                        string triggerName = Tutorials[i].Condition.Timer.trigger;
                        if (triggerName == name && !CheckTutsCompleted(Tutorials[i].Name))
                        {
                            SetTimerOnTutorial(Tutorials[i].Name, Tutorials[i].Condition.Timer.secf);
                        }
                    }
                }
            }
        }
    }

    private void EndOfTutorial()
    {
        //----This code is fired at the end of a tutorial----------------
        if (m_CurTutorial != null)
        {
            MarkTutorialComplete(m_CurTutorial.Name);
        }
        m_CurTutorial = null;

        m_NextButton.gameObject.SetActive(false);

        if(m_StartConditions.Count == 0)
        {
            // We have no queued tutorials so end and hide worm etc
            ShouldBeVisible = false;
            m_WormAnimator.SetTrigger("worm_GoIn");
            m_EndingTutorial = true;
        }
        else
        {
            m_PlayingTutorial = false; // Cause us to start the next tutorial straight away
        }
    }

    public void CloseTutorial()
    {
        m_Lesson_i = m_CurTutorial != null ? m_CurTutorial.Lessons.Length : 1;
        EndOfTutorial();
    }

    public void ResetTutorials()
    {
        TutorialsCompleted = new List<string>();
    }

    /*
    private Button[] m_Buttons;
    private bool[] m_EnabledButtons;
    private bool m_BoolArraySet;

   
    private void Block(bool on)
    {

        if (on && !m_BoolArraySet)
        {
            m_Buttons = UIGlobalVariablesScript.Singleton.UIRoot.GetComponentsInChildren<Button>(true);
            m_EnabledButtons = new bool[m_Buttons.Length];
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].gameObject.GetComponent<Button>().enabled = false;
            }
            m_BoolArraySet = true;

        }
        else
        {
            for (int i = 0; i < m_Buttons.Length; i++)
            {
                m_Buttons[i].gameObject.GetComponent<Button>().enabled = true;

            }
            m_Buttons = null;
            m_BoolArraySet = false;
        }
        NextButton.GetComponent<BoxCollider>().enabled = true;
    }
    */

    /*
    public void MakeScreensVisible(GameObject[] turnons)
    {
        GameObject[] turnoffs = new GameObject[]
        {
            UIGlobalVariablesScript.Singleton.CaringScreenRef,
            UIGlobalVariablesScript.Singleton.AlarmUI,
            UIGlobalVariablesScript.Singleton.StereoUI,
            UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef,
            UIGlobalVariablesScript.Singleton.LightbulbUI,
            UIGlobalVariablesScript.Singleton.AchievementsScreenRef,
            UIGlobalVariablesScript.Singleton.EDMBoxUI,
            UIGlobalVariablesScript.Singleton.PianoUI,
            UIGlobalVariablesScript.Singleton.JunoUI,
            UIGlobalVariablesScript.Singleton.MinigamesMenuMasterScreenRef,
            UIGlobalVariablesScript.Singleton.StatsScreenRef,
            UIGlobalVariablesScript.Singleton.PicturesScreenRef,
            UIGlobalVariablesScript.Singleton.SettingsScreenRef,
            UIGlobalVariablesScript.Singleton.CreditsScreenRef,
            UIGlobalVariablesScript.Singleton.UIRoot.transform.FindChild("ParentalControlsUI").gameObject,
            UIGlobalVariablesScript.Singleton.UIRoot.transform.FindChild("UI - Set Parental Password").gameObject
        };
        for (int i = 0; i < turnoffs.Length; i++)
        {
            turnoffs[i].SetActive(false);
        }
        for (int i = 0; i < turnons.Length; i++)
        {
            turnons[i].SetActive(true);
        }
    }
    */
    
    void OnDestroy()
    {
        TutorialHandler.FireEvents -= EventFired;        
        ShouldSkipLesson -= OnShouldSkipLesson;
    }

    void EventFired(string fired)
    {
        if (fired == "GiveStrawberry")
        {
            if (!ProfilesManagementScript.Instance.CurrentProfile.m_StrawberryAdded)
            {
                ProfilesManagementScript.Instance.CurrentProfile.m_StrawberryAdded = true;
				ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Strawberry, 1);
            }
        }
        else if (fired == "GivePhone")
        {
            if (!ProfilesManagementScript.Instance.CurrentProfile.m_PhoneAdded)
            {
                ProfilesManagementScript.Instance.CurrentProfile.m_PhoneAdded = true;
				ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Phone, 1);
                ItemUnlockBehaviour.Show(InventoryItemId.Phone);
            }
        }
        else if (fired == "ShownStartEatStrawberry")
        {
            ProfilesManagementScript.Instance.CurrentProfile.m_ShownStartEatStrawberry = true;
        }
        else if (fired == "GiveZef")
        {
            if (!ProfilesManagementScript.Instance.CurrentProfile.m_GivenZef)
            {
				
				CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
				progressScript.SpawnStageItem(InventoryItemId.Zef, new Vector3(0.7f, 0.0f, 0.4f) * 200.0f);
				progressScript.SpawnStageItem(InventoryItemId.Zef, new Vector3(-0.1f, 0.0f, -0.7f) * 200.0f);
				progressScript.SpawnStageItem(InventoryItemId.Zef, new Vector3(-0.4f, 0.0f, 0.3f) * 200.0f);
                ProfilesManagementScript.Instance.CurrentProfile.m_GivenZef = true;
            }
		}
		else if (fired == "GiveItemAlbum")
		{
			if (ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetNumItemsOwned(InventoryItemId.ItemAlbum)== 0)
			{
				ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.ItemAlbum, 1);
				ItemUnlockBehaviour.Show(InventoryItemId.ItemAlbum);
			}
		}
		else if (fired == "GiveBlocks")
		{
			if (ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetNumItemsOwned(InventoryItemId.Box1) < 3)
			{
				ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Box1, 3);
			}
		}

    }

    void OnShouldSkipLesson(string skipID)
    {
		bool markAsComplete = false;
        if(skipID == "SkipIfStrawberryOnGround")
        {
            // Skip if we have added strawberry and we are no longer holding it
            ShouldSkip = ProfilesManagementScript.Instance.CurrentProfile.m_StrawberryAdded && !ProfilesManagementScript.Instance.CurrentProfile.Inventory.OwnItem(InventoryItemId.Strawberry);
        }
        else if (skipID == "SkipIfPhoneOnGround")
        {
            // Skip if we have added strawberry and we are no longer holding it
			ShouldSkip = ProfilesManagementScript.Instance.CurrentProfile.m_PhoneAdded && !ProfilesManagementScript.Instance.CurrentProfile.Inventory.OwnItem(InventoryItemId.Phone);
		}
		else if (skipID == "GivenZef")
		{
			ShouldSkip = ProfilesManagementScript.Instance.CurrentProfile.m_GivenZef;
		}
		else if (skipID == "GivenBlocks")
		{
			ShouldSkip = ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetNumItemsOwned(InventoryItemId.Box1) > 2;
		}
		else if (skipID == "NoBlocksInInventory")
		{
			ShouldSkip = ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetNumItemsInInventory(InventoryItemId.Box1) == 0;
			markAsComplete = true;
		}
		if (markAsComplete || skipID == "MarkTutorialAsComplete")
		{
			// Mark this tutorial as complete so that if the user exits the tutorial before finishing all text entries any follow on tutorials
			// will occur on next bootup.
			// Note we are not finishing and closing the tutorial just marking it as complete in the save game.
			if (m_CurTutorial != null)
			{
				MarkTutorialComplete(m_CurTutorial.Name, false);
			}
		}

    }

    
}
