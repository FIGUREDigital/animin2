
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour {

	private Tutorial[] Tutorials{
		get { return TutorialReader.Instance.Tutorials;}
	}

    //-Gameobject Stuff -------
	[SerializeField]
	private GameObject TutorialUIParent;
	[SerializeField]
	private Animator WormAnimator;
	[SerializeField]
	private Text TutorialText;
	[SerializeField]
    private Button NextButton;
    [SerializeField]
    private GameObject StatsButton;
    [SerializeField]
    private HandPointer TutorialHand;

    private bool m_Locked;
    public bool Lock{
        get { return m_Locked; }
        set { m_Locked = value; }
    }


    //-Start Conditions
    private bool[] m_StartConditions;
    public bool[] StartConditions
    {
        get
        {
            return m_StartConditions;
        }
        set
        {
            m_StartConditions = value;
        }
    }

    public void SetTutorialCondition(string name, bool value){
        for (int i = 0; i < Tutorials.Length; i++)
        {
            if (name == Tutorials[i].Name)
            {
                StartConditions[Tutorials[i].id_num] = value;
                return;
            }
        }
        Debug.Log("ERROR: Tutorial Name [" + name + "] not found!");
    }



	private bool m_PlayingTutorial, m_EndingTutorial;
	private int m_CurTutorial_i;
	private int m_Letter_i, m_Lesson_i, m_Entry_i;

	private GameObject m_CurrentListening;
	public GameObject CurrentListeningGO{ get { return m_CurrentListening; } }

	private string m_CurrentAdHocExitCond;
    public string CurrentAdHocExitCond{ get { return m_CurrentAdHocExitCond; } }
	private bool m_WaitingForInput;


	private const string TutorialPlayerPrefID = "TUTORIALS_COMPLETED";
    private bool CheckPref(int id){
        return PlayerPrefs.GetString(TutorialPlayerPrefID + id) == "true";
    }






	private float m_Timer;
	private int m_TutorialCountingDown= -1;
    private bool /*the secret to comedy*/ m_IsTiming;

	private void SetTimerOnTutorial(int TutId, float time){
		Debug.Log ("Setting Timer. Tut : ["+TutId+"]; Time : ["+time+"];");
		m_TutorialCountingDown = TutId;
		m_Timer = time;
        m_IsTiming = true;
	}
	private void TurnOffTimer(){
		m_TutorialCountingDown = -1;
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
	void Start () {
		//Blocker.gameObject.SetActive (false);
		WormAnimator.gameObject.SetActive(false);
		TutorialUIParent.SetActive (false);
        TutorialHand.gameObject.SetActive(false);

		TutorialReader.Instance.Deserialize ();
		for (int i = 0; i < Tutorials.Length; i++) {
			if (PlayerPrefs.GetString(TutorialPlayerPrefID + i) == null)
				PlayerPrefs.SetString(TutorialPlayerPrefID + i,"false");
		}
        StartConditions = new bool[Tutorials.Length];
        for (int i = 0 ; i < Tutorials.Length; i ++){
            if (Tutorials[i].Condition != null)
            {
                if (Tutorials[i].Condition.Initial != null)
                {
                    StartConditions[i] = true;
                }
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
            }
        }
	}
	


	// Update is called once per frame
	void Update () {
		if (!m_PlayingTutorial) {
			//I HATE MONODEVELOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
//																								OOOOOOOOOOOOOOOOOOOOOOOOOP!
            if(!m_Locked){
			//TutorialReader.Instance.test();
			for (int i = 0; i < Tutorials.Length; i++) {
                    if (CheckStartCondition(i))
                    {
                        if (TutorialUIParent == null)
                            return;
                        TutorialUIParent.SetActive(true);
                        WormAnimator.gameObject.SetActive(true);
                        //Blocker.gameObject.SetActive(true);
                        Block(true);
                        WormAnimator.SetTrigger("worm_GoOut");

                        m_CurTutorial_i = i;
                        m_Letter_i = 0;
                        m_Lesson_i = 0;
                        m_Entry_i = 0;
					
                        MakeScreensVisible(new GameObject[]{ UIGlobalVariablesScript.Singleton.CaringScreenRef });

                        m_PlayingTutorial = true;
                        break;
                    }
				}
			}
		} else if (!m_EndingTutorial){
			if (m_Entry_i >= Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].TutEntries.Length) NextLesson();
			string text = Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].TutEntries[m_Entry_i].text;

			if (text.Length >= m_Letter_i){
                Debug.Log("DeltaTime : [" + Time.deltaTime+"]; Multiplied : [" + Time.deltaTime * 100+"];");

                m_Letter_i += (int)(m_Lesson_i + Mathf.Max((Time.deltaTime * 75f),1));

                TutorialText.text = text.Substring(0,Mathf.Min(m_Letter_i,text.Length));
				NextButton.gameObject.SetActive(false);
			} else {
				if (!m_WaitingForInput)
					NextButton.gameObject.SetActive(true);
			}
		} else {
			if (WormAnimator.GetCurrentAnimatorStateInfo(0).IsName("worm_hidden")){
				
				TutorialUIParent.SetActive (false);
				WormAnimator.gameObject.SetActive(false);
				m_PlayingTutorial = false;
				m_EndingTutorial = false;
				TutorialUIParent.SetActive (false);
			}
		}
        if (m_IsTiming)
        {
            Debug.Log("Timing : ["+((int)m_Timer)+"];");
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime;
            else
            {
                Debug.Log("Turning off timer. Cond : ["+m_TutorialCountingDown+"] = true;");
                StartConditions[m_TutorialCountingDown] = true;
                TurnOffTimer();
            }
        }



        if (ProfilesManagementScript.Singleton.CurrentAnimin.Health / PersistentData.MaxHealth <= 0.4f)
        {
            UIGlobalVariablesScript.Singleton.TutHandler.TriggerAdHocStartCond("HealthBelow40");
        }
        if (Time.timeSinceLevelLoad >= (5 * 60))
        {
            UIGlobalVariablesScript.Singleton.TutHandler.TriggerAdHocStartCond("5Minutes");
        }
        if (Time.timeSinceLevelLoad >= (9 * 60))
        {
            UIGlobalVariablesScript.Singleton.TutHandler.TriggerAdHocStartCond("9Minutes");
        }



        if (IsPlaying)
        {

        }

	}















    //- ENTRY CONDITIONS ----------------------------------------------------------------
    public bool CheckCharacterProgress(CharacterProgressScript script, RaycastHit hitInfo){
        bool cont = false;
        /*
        switch (m_CurrentExitCond) {
            case ("WakeUp"):
                if (hitInfo.collider.gameObject == script.SleepBoundingBox)
                    cont = true;
                break;
        }
        */
        switch (m_CurrentAdHocExitCond)
        {
            case("walkto"):
            case("runto"):
                if (hitInfo.collider.name.StartsWith("Invisible Ground Plane"))
                    cont = true;
                break;
            case("tap"):
                if (hitInfo.collider.name.StartsWith("MainCharacter") || hitInfo.collider.gameObject == script.ObjectHolding)
                    cont = true;
                break;

        }
        return cont;
    }

    //This method test whether or not to start the tutorial.
    private bool CheckStartCondition(int id){

        if (CheckPref(id))
            return false;

        return StartConditions[id];

        /*switch (id) {
        case (0):
            return true;
        case (1):
            return (m_Timer<0);
        case (2):
            return (m_Timer<0);
        case (3):
            return (m_Timer<0);
        case (4):
            return (m_Timer<0);
        case (5):
            return (m_Timer<0);
        case (6):
            return (m_Timer<0);
        case (7):
            return (m_Timer<0);
            
        default:
            return false;
        }
        */      
    }

    public void TriggerAdHocStartCond(string call){
        if (IsPlaying)
            return;

        for (int i = 0; i < Tutorials.Length; i++)
        {
            if (CheckPref(i) == true) continue;
            if (Tutorials[i].Condition == null)  continue;
            if (Tutorials[i].Condition.AdHocCond == null)  continue;
            if (Tutorials[i].Condition.AdHocCond.call == call)
            {
                StartConditions[i] = true;
            }
        }
    }


    public void OnTutorialStartClick(GameObject go){
        if (!IsPlaying)
        {
            for (int i = 0; i < Tutorials.Length; i++)
            {
                if (CheckPref(i) == true) continue;
                if (Tutorials[i].Condition == null)  continue;
                if (Tutorials[i].Condition.ButtonCond == null)  continue;
                Debug.Log("Checking GameObject : ["+go.name+"] against : ["+Tutorials[i].Condition.ButtonCond.name+"];");
                if (go.name == Tutorials[i].Condition.ButtonCond.name)
                {
                    StartConditions[i] = true;
                }
            }
        }
    }
    //- EXIT Conditions ----------------------------------------------------------------
	public void OnTutorialEndClick(GameObject go){
		if (go == m_CurrentListening) {
			//UIEventListener.Get (m_CurrentListening).onClick -= OnTutorialEndClick;
			m_WaitingForInput = false;
			//go.GetComponent<UIWidget>().depth = m_SavedDepth;
			go.GetComponent<BoxCollider>().enabled = false;
			//NextLesson();
			NextButtonPress(true);
		}
	}

    public void TriggerAdHocExitCond(string TutorialName, string StampName){
        //Debug.Log("Current Tutorial Name : ["+Tutorials[m_CurTutorial_i].Name+"]; Name to Change : ["+TutorialName+"];");
        if (Tutorials[m_CurTutorial_i].Name == TutorialName)
        {
            TriggerAdHocExitCond(Tutorials[m_CurTutorial_i].id_num, StampName);
        }
    }
    public void TriggerAdHocExitCond(int id, string StampName){
        if (Tutorials[m_CurTutorial_i].id_num == id && 
            //Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].ExitStr == StampName && 
            Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].EndCondition.AdHocCond.call == CurrentAdHocExitCond &&
            m_WaitingForInput)
        {
            m_WaitingForInput = false;
            NextButtonPress(true);
        }
    }

	
	//- End of Lesson Processing ----------------------------------------------------------------
    public void NextButtonPress(bool ignoreCheck = false){
        TutorialHand.gameObject.SetActive(false);

        Lesson CurrentLesson = Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i];

        int maxEntries = CurrentLesson.TutEntries.Length;


		Debug.Log ("Testing Entry : ["+m_Entry_i+":"+maxEntries+"]");
		
		m_Letter_i = 0;
		m_Entry_i += 1;

        bool noNext = false;
        if (CurrentLesson.EndCondition != null)
            noNext = true;
        if (ignoreCheck)
            noNext = false;

        if (noNext) maxEntries -= 1;

		if (m_Entry_i >= maxEntries) {

			if (noNext) {

                // - When at the end of a lesson, this code here fires
                switch(CurrentLesson.EndCondition.type){
                    case (Condition.Type.Button):
                        GameObject go = GameObject.Find(CurrentLesson.EndCondition.ButtonCond.name);
                        if (go != null && go.GetComponent<Button>() != null)
                        {
                            //Enable Button
                            go.GetComponent<Button>().enabled = true;

                            //Set up listener
                            //UIEventListener.Get(go).onClick += OnTutorialEndClick;


                            TutorialHand.gameObject.SetActive(true);
                            TutorialHand.gameObject.transform.position = go.transform.position;

                            //Set Tutorial to wait for listener input
                            m_CurrentListening = go;
                            m_WaitingForInput = true;
                        }
                        break;
                    case(Condition.Type.AdHoc):
                        switch (CurrentLesson.EndCondition.AdHocCond.call)
                        {
                            case ("EatStrawberry"):
                                ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Strawberry, 1);

                                UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = InventoryItemId.Strawberry;
                               // UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Image>().spriteName = "strawberry";
                               // UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().normalSprite = "strawberry";
                                UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<UIClickButtonMasterScript>().enabled = false;
                                UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().enabled = true;


                                ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin].Hungry = 0;
                                ProfilesManagementScript.Singleton.CurrentAnimin.Hungry = 0;

                                TutorialHand.gameObject.SetActive(true);
                                TutorialHand.gameObject.transform.position = UIGlobalVariablesScript.Singleton.FoodButton.transform.position;
                                break;
                        }
                        m_CurrentAdHocExitCond = CurrentLesson.EndCondition.AdHocCond.call;
                        m_WaitingForInput = true;
                        break;
                }

                //Disable next button
                NextButton.gameObject.SetActive(false);

				//Exit stamp handling
                /*OnTutorialEndClickurrentExitCond){
    				case ("Stats"):
    					m_CurrentListening = UIGlobalVariablesScript.Singleton.StatsButton;
    					UIWidget widget = m_CurrentListening.GetComponent<UIWidget> ();
    					m_CurrentListening.GetComponent<BoxCollider>().enabled = true;

    					UIEventListener.Get (m_CurrentListening).onClick += OnTutorialClick;
    					
    					NextButton.gameObject.SetActive(false);
    					m_WaitingForInput = true;
    					break;
                    case("EatStrawberry"):
					
                        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Strawberry, 1);

                        UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = InventoryItemId.Strawberry;
                        UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Image>().spriteName = "strawberry";
                        UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().normalSprite = "strawberry";
                        UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<UIClickButtonMasterScript>().enabled = false;
                        UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().isEnabled = true;


					ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin].Hungry = 0;
					ProfilesManagementScript.Singleton.CurrentAnimin.Hungry = 0;
					m_WaitingForInput = true;
					break;
				default:
					m_WaitingForInput = true;
					break;
				}
                */  



			} else {
				NextLesson ();
			}
		}
	}


    //----Load next lesson------------------------------------------------
	public void NextLesson(){
		if (m_WaitingForInput) return;
		
		m_Letter_i = 0;
		m_Entry_i =0;
		int maxLessons = Tutorials [m_CurTutorial_i].Lessons.Length;
		
		//Debug.Log ("maxLessons : [" + maxLessons + "];");
		
		if (++m_Lesson_i >= maxLessons) {

            //----This code is fired at the end of a tutorial----------------

			WormAnimator.SetTrigger ("worm_GoIn");
			
			PlayerPrefs.SetString(TutorialPlayerPrefID + m_CurTutorial_i,"true");

			//TutorialReader.Instance.TutorialFinished[m_CurTutorial_i] = true;
			
			//Blocker.gameObject.SetActive(false);
			Block(false);
			
			m_EndingTutorial = true;
			NextButton.gameObject.SetActive(false);


            for (int i = 0; i < Tutorials.Length; i++)
            {
                if (Tutorials[i].Condition != null)
                {
                    if (Tutorials[i].Condition.Timer != null)
                    {
                        int trig = Tutorials[i].Condition.Timer.trigi;
                        if (trig == m_CurTutorial_i && !CheckPref(i))
                        {
                            SetTimerOnTutorial(i, Tutorials[i].Condition.Timer.secf);
                        }
                    }
                }
            }
		}
	}











	private Button[] m_Buttons;
	private bool[] m_EnabledButtons;
	private bool m_BoolArraySet;

	private void Block (bool on){

		if (on && !m_BoolArraySet) {
			m_Buttons = UIGlobalVariablesScript.Singleton.UIRoot.GetComponentsInChildren<Button>(true);
			m_EnabledButtons = new bool[m_Buttons.Length];
			for (int i = 0; i < m_Buttons.Length; i++){
                m_Buttons[i].gameObject.GetComponent<Button>().enabled = false;
			}
			m_BoolArraySet = true;

		} else {
            for (int i = 0; i < m_Buttons.Length; i++){
                m_Buttons[i].gameObject.GetComponent<Button>().enabled = true;

			}
			m_Buttons = null;
			m_BoolArraySet = false;
		}
		NextButton.GetComponent<BoxCollider> ().enabled = true;
	}






	public void ResetTutorials(){
		for (int i = 0; i < Tutorials.Length; i++) {
			PlayerPrefs.SetString(TutorialPlayerPrefID + i,"false");
			TurnOffTimer();
		}
	}


	public void MakeScreensVisible(GameObject[] turnons){
		GameObject[] turnoffs = new GameObject[]{
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
			UIGlobalVariablesScript.Singleton.UIRoot.transform.FindChild ("ParentalControlsUI").gameObject,
			UIGlobalVariablesScript.Singleton.UIRoot.transform.FindChild ("UI - Set Parental Password").gameObject
		};
		for (int i = 0; i < turnoffs.Length; i++){
			turnoffs[i].SetActive(false);
		}
		for (int i = 0; i < turnons.Length; i++){
			turnons[i].SetActive(true);
		}
	}
}