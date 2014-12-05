
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{

    private Tutorial[] Tutorials
    {
        get { return TutorialReader.Instance.Tutorials; }
    }
    private bool m_Inited;
    public bool Inited { get { return m_Inited; } }

    //-Gameobject Stuff -------
    [SerializeField]
    private GameObject m_TutorialUIParent;
    [SerializeField]
    private Animator m_WormAnimator;
    [SerializeField]
    private Text m_TutorialText;
    [SerializeField]
    private Button m_NextButton;
    [SerializeField]
    private HandPointer m_TutorialHand;
    [SerializeField]
    private GameObject m_WormPosition;

    private bool m_Locked;

    public bool Lock
    {
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

    public void SetTutorialCondition(string name, bool value)
    {
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

    private bool WaitingForInput
    {
        get{ return false; } 
        set{ m_WaitingForInput = value; }
    }


    private const string TutorialPlayerPrefID = "TUTORIALS_COMPLETED";

    private bool CheckPref(int id)
    {
        return PlayerPrefs.GetString(TutorialPlayerPrefID + id) == "true";
    }






    private float m_Timer;
    private int m_TutorialCountingDown = -1;
    private bool /*the secret to comedy*/ m_IsTiming;

    private void SetTimerOnTutorial(int TutId, float time)
    {
        Debug.Log("Setting Timer. Tut : [" + TutId + "]; Time : [" + time + "];");
        m_TutorialCountingDown = TutId;
        m_Timer = time;
        m_IsTiming = true;
    }

    private void TurnOffTimer()
    {
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
    void Start(){
        Init();
    }
    public void Init()
    {
        if (m_Inited)
            return;
        m_Inited = true;
        m_WormAnimator.gameObject.SetActive(false);
        m_TutorialUIParent.SetActive(false);
        m_TutorialHand.gameObject.SetActive(false);

        for (int i = 0; i < Tutorials.Length; i++)
        {
            if (PlayerPrefs.GetString(TutorialPlayerPrefID + i) == null)
                PlayerPrefs.SetString(TutorialPlayerPrefID + i, "false");
        }
        StartConditions = new bool[Tutorials.Length];

        for (int i = 0; i < Tutorials.Length; i++)
        {
            if (Tutorials[i].Condition != null)
            {
                if (Tutorials[i].Condition.Initial != null)
                {
                    StartConditions[i] = true;
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
	


    // Update is called once per frame
    void Update()
    {
        if (!m_PlayingTutorial)
        {
            if (!m_Locked)
            {
                for (int i = 0; i < Tutorials.Length; i++)
                {
                    if (CheckStartCondition(i))
                    {
                        if (m_TutorialUIParent == null)
                            return;
                        m_TutorialUIParent.SetActive(true);
                        m_WormAnimator.gameObject.SetActive(true);
                        //Blocker.gameObject.SetActive(true);
                        //Block(true);
                        m_WormAnimator.SetTrigger("worm_GoOut");

                        m_CurTutorial_i = i;
                        m_Letter_i = 0;
                        m_Lesson_i = 0;
                        m_Entry_i = 0;
					
                        //MakeScreensVisible(new GameObject[]{ UIGlobalVariablesScript.Singleton.CaringScreenRef });

                        m_PlayingTutorial = true;
                        break;
                    }
                }
            }
        }
        else if (!m_EndingTutorial)
        {
            if (m_Entry_i >= Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].TutEntries.Length)
                NextLesson();
            string text = Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].TutEntries[m_Entry_i].text;

            if (text.Length >= m_Letter_i)
            {
                //Debug.Log("DeltaTime : [" + Time.deltaTime + "]; Multiplied : [" + Time.deltaTime * 100 + "];");

                m_Letter_i += (int)(m_Lesson_i + Mathf.Max((Time.deltaTime * 75f), 1));

                m_TutorialText.text = text.Substring(0, Mathf.Min(m_Letter_i, text.Length));
                m_NextButton.gameObject.SetActive(false);
            }
            else
            {
                if (!WaitingForInput)
                    m_NextButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (m_WormAnimator.GetCurrentAnimatorStateInfo(0).IsName("worm_hidden"))
            {
				
                m_TutorialUIParent.SetActive(false);
                m_WormAnimator.gameObject.SetActive(false);
                m_PlayingTutorial = false;
                m_EndingTutorial = false;
                m_TutorialUIParent.SetActive(false);
            }
        }
        if (m_IsTiming)
        {
            Debug.Log("Timing : [" + ((int)m_Timer) + "];");
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime;
            else
            {
                Debug.Log("Turning off timer. Cond : [" + m_TutorialCountingDown + "] = true;");
                StartConditions[m_TutorialCountingDown] = true;
                TurnOffTimer();
            }
        }

        m_WormPosition.transform.position = camera.ViewportToWorldPoint(new Vector3(0.05f, -0.1f, (camera.farClipPlane+camera.nearClipPlane)/2));
    }
    void OnDrawGizmosSelected() {
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.1F);
    }















    //- ENTRY CONDITIONS ----------------------------------------------------------------
    public bool CheckCharacterProgress(CharacterProgressScript script, RaycastHit hitInfo)
    {
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
    private bool CheckStartCondition(int id)
    {

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

    public void TriggerAdHocStartCond(string call)
    {
        if (IsPlaying)
            return;
        Init();

        for (int i = 0; i < Tutorials.Length; i++)
        {
            if (CheckPref(i) == true)
                continue;
            if (Tutorials[i].Condition == null)
                continue;
            if (Tutorials[i].Condition.AdHocCond == null)
                continue;
            if (Tutorials[i].Condition.AdHocCond.call == call)
            {
                StartConditions[i] = true;
            }
        }
    }


    public void OnTutorialStartClick(GameObject go)
    {
        if (!IsPlaying)
        {
            for (int i = 0; i < Tutorials.Length; i++)
            {
                if (CheckPref(i) == true)
                    continue;
                if (Tutorials[i].Condition == null)
                    continue;
                if (Tutorials[i].Condition.ButtonCond == null)
                    continue;
                Debug.Log("Checking GameObject : [" + go.name + "] against : [" + Tutorials[i].Condition.ButtonCond.name + "];");
                if (go.name == Tutorials[i].Condition.ButtonCond.name)
                {
                    StartConditions[i] = true;
                }
            }
        }
    }
    //- EXIT Conditions ----------------------------------------------------------------
    public void OnTutorialEndClick(GameObject go)
    {
        if (go == m_CurrentListening)
        {
            //UIEventListener.Get (m_CurrentListening).onClick -= OnTutorialEndClick;
            WaitingForInput = false;
            //go.GetComponent<UIWidget>().depth = m_SavedDepth;
            go.GetComponent<BoxCollider>().enabled = false;
            //NextLesson();
            NextButtonPress(true);
        }
    }

    public void TriggerAdHocExitCond(string TutorialName, string StampName)
    {
        //Debug.Log("Current Tutorial Name : ["+Tutorials[m_CurTutorial_i].Name+"]; Name to Change : ["+TutorialName+"];");
        if (Tutorials[m_CurTutorial_i].Name == TutorialName)
        {
            TriggerAdHocExitCond(Tutorials[m_CurTutorial_i].id_num, StampName);
        }
    }

    public void TriggerAdHocExitCond(int id, string StampName)
    {
        if (Tutorials[m_CurTutorial_i].id_num == id &&
            //Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].ExitStr == StampName && 
            Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i].EndCondition.AdHocCond.call == CurrentAdHocExitCond &&
            WaitingForInput)
        {
            WaitingForInput = false;
            NextButtonPress(true);
        }
    }

	
    //- End of Lesson Processing ----------------------------------------------------------------
    public void NextButtonPress(bool ignoreCheck = false)
    {
        m_TutorialHand.gameObject.SetActive(false);

        Lesson CurrentLesson = Tutorials[m_CurTutorial_i].Lessons[m_Lesson_i];

        int maxEntries = CurrentLesson.TutEntries.Length;


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
        int maxLessons = Tutorials[m_CurTutorial_i].Lessons.Length;
		
        //Debug.Log ("maxLessons : [" + maxLessons + "];");
		
        if (++m_Lesson_i >= maxLessons)
        {
            EndOfLesson();
        }
    }

    private void EndOfLesson()
    {
        //----This code is fired at the end of a tutorial----------------

        m_WormAnimator.SetTrigger("worm_GoIn");

        PlayerPrefs.SetString(TutorialPlayerPrefID + m_CurTutorial_i, "true");

        //TutorialReader.Instance.TutorialFinished[m_CurTutorial_i] = true;

        //Blocker.gameObject.SetActive(false);
        //Block(false);

        m_EndingTutorial = true;
        m_NextButton.gameObject.SetActive(false);


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

    public void CloseTutorial()
    {
        m_Lesson_i = Tutorials[m_CurTutorial_i].Lessons.Length;
        EndOfLesson();
    }

    public void ResetTutorials()
    {
        for (int i = 0; i < Tutorials.Length; i++)
        {
            PlayerPrefs.SetString(TutorialPlayerPrefID + i, "false");
            TurnOffTimer();
        }
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
}