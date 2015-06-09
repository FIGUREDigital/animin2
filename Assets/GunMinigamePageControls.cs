using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UiImage = UnityEngine.UI.Image;
using TMPro;

public enum ReadyStates
{
    Ready3,
    Ready2,
    Ready1,
    Go,
	None,
    Count,
}


public class GunMinigamePageControls : MonoBehaviour
{

    [SerializeField]
    private UnityEngine.UI.Image m_Bar, m_GunIcon;
	
	[SerializeField]
	private UIText text123;

	public Animation m_321;
    
    public UnityEngine.UI.Image Bar { get { return m_Bar; } }

    public UnityEngine.UI.Image Icon { get { return m_GunIcon; } }

	public UIText completeMessage;

	public GameObject awesome;


    [SerializeField]
	private TextMeshProUGUI m_Points;

	public TextMeshProUGUI Points { get { return m_Points; } }


    [SerializeField]
    private GameObject m_TutorialEnemies, m_TutorialMove;
    private int TutorialCounter;

    private bool m_Paused;

    public bool Paused
    {
        get{ return m_Paused; }
        set{ m_Paused = value; }
    }

    private JoystickPageControls JoystickControls
    {
        get
        {
            if (m_JoystickControls == null)
                m_JoystickControls = UiPages.GetPage(Pages.JoystickPage).GetComponent<JoystickPageControls>();

            return m_JoystickControls;
        }
    }

    private JoystickPageControls m_JoystickControls;

    public bool WaitingForTouch
    {
        get
        {
//            Debug.Log("Waiting for Input : [" + (TutorialCounter == 0 || TutorialCounter == 1) + "];");
            return  (!ProfilesManagementScript.Instance.CurrentProfile.TutorialCanonClashPlayed) && (TutorialCounter == 0 || TutorialCounter == 1);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ProfilesManagementScript.Instance.CurrentProfile.TutorialCanonClashPlayed == false)
        {
            switch (TutorialCounter)
            {
                case 0:
                    if (!m_Paused)
                    {
                        SetBarWidth(0);
                        m_TutorialMove.SetActive(true);
                        m_Points.transform.parent.gameObject.SetActive(false);
                        m_321.gameObject.SetActive(false);
                        m_TutorialMove.SetActive(false);

                        m_TutorialEnemies.SetActive(true);

                        TutorialCounter++;
                    }
                    break;
                case 1:
                    if (Input.GetButtonUp("Fire1"))
                    {
                        m_TutorialEnemies.SetActive(false);
                        m_TutorialMove.SetActive(true);
                        TutorialCounter++;
                    }
                    break;
                case 2:
                    if (JoystickControls.IsMovingWithJoystick)
                    {

                        m_TutorialMove.SetActive(false);
                        m_Points.transform.parent.gameObject.SetActive(true);
                        GunsMinigameScript script = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>();
                        if (script != null)
                        if (!script.Go321Done)
                            m_321.gameObject.SetActive(true);
                        ProfilesManagementScript.Instance.CurrentProfile.TutorialCanonClashPlayed = true;
                        TutorialCounter++;
                    }
                    break;
            }
        }
    }

    public void ResetTutorial()
    {
        m_TutorialEnemies.SetActive(false);
        m_TutorialMove.SetActive(false);
        TutorialCounter = 0;
        ProfilesManagementScript.Instance.CurrentProfile.TutorialCanonClashPlayed = false;
    }

    public void SetReadyState(ReadyStates newState)
    {
		Debug.Log ("SetReadyState "+newState);
		m_321.gameObject.SetActive (newState <= ReadyStates.Go);
        switch (newState)
        {
            case ReadyStates.Ready3:
				text123.Text = "3";
                break;
			case ReadyStates.Ready2:
				text123.Text = "2";
                break;
			case ReadyStates.Ready1:
				text123.Text = "1";
                break;
			case ReadyStates.Go:
				text123.Text = "Go!";
                break;
        }
    }

    public void SetBarWidth(float width)
    {
        m_Bar.fillAmount = width;
    }

    public void SetAmmoType()
    {

    }

    void OnEnable()
    {
        if (!ProfilesManagementScript.Instance.CurrentProfile.TutorialCanonClashPlayed)
            TutorialCounter = 0;
        m_Points.transform.parent.gameObject.SetActive(true);
        m_321.gameObject.SetActive(true);
        m_321.gameObject.SetActive(false);
        m_TutorialEnemies.SetActive(false);
        if (UiPages.GetPage(Pages.JoystickPage) != null)
            UiPages.GetPage(Pages.JoystickPage).SetActive(true);
    }

    void OnDisable()
    {
        if (UiPages.GetPage(Pages.JoystickPage) != null)
            UiPages.GetPage(Pages.JoystickPage).SetActive(false);
    }

	public void SetWaveComplete(int wave)
	{
		awesome.SetActive(wave > 0);
		if(wave <= 0) return;
		awesome.GetComponent<Animation>().GetComponent<Animation>().Play("AwesomeScale");
		if (wave >= 10)
		{
			completeMessage.Text = "All Waves Complete!";
		}
		else
		{
			completeMessage.Text = "Wave "+wave+" Complete";
		}
	}

	public IEnumerator ShowWaveComplete(int wave)
	{		
		SetWaveComplete(wave);
		//	UiPages.GetPage(Pages.GunMinigamePage).GetComponent<GunMinigamePageControls>().Paused = true;
		//	UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Paused = true;
		//	uiParticles = GameObject.FindGameObjectWithTag("uiparticles");
		//	uiParticles.GetComponent<ParticleSystem>().particleSystem.Play();
		//GameObject.FindGameObjectWithTag("wave321go").GetComponent<UiImage>().sprite = clear3;
		SetReadyState(ReadyStates.Ready3);
		GameObject.FindGameObjectWithTag("wave321go").GetComponent<Animation>().GetComponent<Animation>().Play("321goScale");
		yield return new WaitForSeconds (1.0f);
		//GameObject.FindGameObjectWithTag("wave321go").GetComponent<UiImage>().sprite = clear2;
		SetReadyState(ReadyStates.Ready2);
		GameObject.FindGameObjectWithTag("wave321go").GetComponent<Animation>().GetComponent<Animation>().Play("321goScale");
		yield return new WaitForSeconds (1.0f);
		//GameObject.FindGameObjectWithTag("wave321go").GetComponent<UiImage>().sprite = clear1;
		SetReadyState(ReadyStates.Ready1);
		GameObject.FindGameObjectWithTag("wave321go").GetComponent<Animation>().GetComponent<Animation>().Play("321goScale");
		yield return new WaitForSeconds (1.0f);
		//GameObject.FindGameObjectWithTag("wave321go").GetComponent<UiImage>().sprite = clearGo;
		SetReadyState(ReadyStates.Go);
		GameObject.FindGameObjectWithTag("wave321go").GetComponent<Animation>().GetComponent<Animation>().Play("321goScale");
		yield return new WaitForSeconds (1.0f);
		GameObject.FindGameObjectWithTag("awesomeUI").GetComponent<Animation>().GetComponent<Animation>().Play("AwesomeScaleDown");
		GameObject.FindGameObjectWithTag("wave321go").GetComponent<Animation>().GetComponent<Animation>().Play("321goScaleDown");
		yield return new WaitForSeconds (1.0f);
		SetReadyState(ReadyStates.None);
		//GameObject.FindGameObjectWithTag("wave321go").GetComponent<UiImage>().sprite = blankSprite;
		
		SetWaveComplete(-1);
		//	uiParticles.GetComponent<ParticleSystem>().particleSystem.Clear();
		//	uiParticles.GetComponent<ParticleSystem>().particleSystem.Stop();
		//	UiPages.GetPage(Pages.GunMinigamePage).GetComponent<GunMinigamePageControls>().Paused = false;
        //	UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Paused = false;
	}

}
