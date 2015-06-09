using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUnlockBehaviour : MonoBehaviour 
{
	static ItemUnlockBehaviour instance;

	[System.Serializable]
	public class UnlockItems
	{
		public InventoryItemId itemID;
		public Image uiItem;
	}

	public UnlockItems[] showForItems;

	public GameObject fullScreenCloseButton;
	public ParticleSystem particleEffects;
    public InventoryItemId showing = InventoryItemId.None;

	ParticleSystem.Particle[] particles;

	private bool turnOffItem;
    bool hiddenTut = false;

	public UnityEngine.UI.Graphic[] fadeImages;

	float fade;	// 0 = hidden 1 = fully visible.
	float fadeDirection = 0;	

	Image showImage = null;	// The UI.Image to be shown

	// Use this for initialization
	void Start () 
	{		
		ShowHideComponents(false);
	}

	void OnEnable()
	{
		instance = this;
	}

	void OnDisable()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public void OnCloseButton()
	{
		fadeDirection = -1;			
		particleEffects.enableEmission = false;
	}

	public static void Show(InventoryItemId showItemID)
	{
		if (instance != null)
		{
			instance.DoShow(showItemID);
		}
	}
    void FinishedUnlock(InventoryItemId itemID)
    {
        TutorialHandler.TriggerAdHocStatic("Unlocked"+itemID.ToString());
    }
	void DoShow(InventoryItemId showItemID)
	{
        AudioController.Play("NewItemUnlocked");
        if(!hiddenTut)
        {
            TutorialHandler.Hide(true);
            hiddenTut = true;
        }
        // Cope with unlocking multiple items while already showing unlocked dialog
        if (showing != showItemID && showing != InventoryItemId.None)
        {
            FinishedUnlock(showing);
        }
        showing = showItemID;
		showImage = null;
		// Set all images to off		
		for(int i = 0; i < showForItems.Length; i++)
		{
			showForItems[i].uiItem.gameObject.SetActive(false);	// Found item will be set 
			if(showForItems[i].itemID == showItemID)
			{
				showImage = showForItems[i].uiItem;
			}
		}

		if (showImage == null)
		{
			return;	// None found so no need to show
		}
        else
        {
			SetAlpha(0);
        }
		ShowHideComponents(true);
		fadeDirection = 1;		
		particleEffects.enableEmission = true;
	}

	public void ShowHideComponents(bool show)
	{			
		fullScreenCloseButton.SetActive(show);
		//particleEffects.SetActive(show);
		
		for(int i = 0 ; i < fadeImages.Length; i++)
		{			
			fadeImages[i].gameObject.SetActive (show);
		}
		if(showImage != null)
		{
			showImage.gameObject.SetActive(show);
		}

        if (hiddenTut != show)
        {
            TutorialHandler.Hide(show);
            hiddenTut = show;
        }
		if (!show && showing != InventoryItemId.None)
        {
            FinishedUnlock(showing);
            showing = InventoryItemId.None;
        }
	}

	// Update is called once per frame
	void Update () 
	{
		if(fadeDirection == 0) return;
		fade += fadeDirection * Time.deltaTime;

		if (fade > 1)
		{
			fade = 1;
			fadeDirection = 0;
		}

		if(fade < 0)
		{
			if (fadeDirection < 0)
			{
				// Finished fading out
				ShowHideComponents(false);
				fadeDirection = 0;
			}
		}
		SetAlpha(fade);
	}

	void SetAlpha(float a)
	{
		Color c;
		for(int i = 0 ; i < fadeImages.Length; i++)
		{

			c= fadeImages[i].color;
			c.a = a;
			fadeImages[i].color = c;
		}
		
		c = showImage.color;
		c.a = a;
		showImage.color = c;
		/*
		if (!particleEffects.enableEmission) 
		{
			if(particles == null)
			{
				particles = new ParticleSystem.Particle[particleEffects.maxParticles];
			}
			particleEffects.GetParticles(particles);
			float advanceLife = Time.deltaTime * particleEffects.startLifetime;
			for(int i = 0; i < particleEffects.particleCount; i++)
			{
				ParticleSystem.Particle p = particles[i];
				p.lifetime -= advanceLife;
			}
			particleEffects.SetParticles(particles, particleEffects.particleCount);
		}*/
	}
}
