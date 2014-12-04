using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CubeMinigamesPageControls : MonoBehaviour {

    MinigameCollectorScript MinigameScript;
    MinigameAnimationControllerScript CharacterAnimationRef;
    CharacterControllerScript CharacterControllerRef;

    [SerializeField]
    private GameObject[] m_Hearts, m_Stars;
    public GameObject[] Hearts { get { return m_Hearts; } }
    public GameObject[] Stars { get { return m_Stars; } }

    [SerializeField]
    private GameObject m_JoystickFront, m_JoystickBack;

    [SerializeField]
    private Text m_LevelCounter, m_Points;
    public Text LevelCounter { get { return m_LevelCounter; } }
    public Text PointLabel { get { return m_Points; } }

    [SerializeField]
    private GameObject m_PauseButton, m_PauseMenu;

    private bool isButtonDown = false;
    private int fingerID = -1;

    private bool m_Paused;
	// Use this for initialization
    void Start () {
        MinigameScript = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>();
        CharacterControllerRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>();
        CharacterAnimationRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>();
    }
	
	// Update is called once per frame
    void Update () {

        if (m_Paused)
            return;

        if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef==null || UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().Paused)
            return;
        Debug.DrawLine(m_JoystickBack.transform.position, Input.mousePosition, Color.blue);
        Vector3 mousePosition = Vector3.zero;

        isButtonDown = false;
        #if UNITY_EDITOR
        if(Input.GetMouseButton(0))
        {
            isButtonDown = true;
            mousePosition = Input.mousePosition;
        }
        #endif
        for(int i=0;i<Input.touchCount;++i)
        {
            if(fingerID == -1)
            {
                if(isHittingThumbBack(Input.GetTouch(i).position))
                {
                    fingerID = Input.GetTouch(i).fingerId;
                    isButtonDown = true;
                    mousePosition = Input.GetTouch(i).position;
                }
            }
            else
            {
                if(Input.GetTouch(i).fingerId == fingerID)
                {
                    isButtonDown = true;
                    mousePosition = Input.GetTouch(i).position;
                }
            }
        }
        if(!isButtonDown) fingerID = -1;

        float movementSpeed = 0;

        bool fingerTouchValid = false;

        if (isButtonDown)
        {
            if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef != null && UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>() != null)
            if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().TutorialId == MinigameCollectorScript.TutorialStateId.ShowMovement)
                UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().AdvanceTutorial();

            if (UIGlobalVariablesScript.Singleton.GunGameScene != null && UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>() != null)
            if (UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().TutorialID == GunsMinigameScript.TutorialStateId.ShowMove)
                UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().AdvanceTutorial();

            Vector3 diff = mousePosition - m_JoystickBack.transform.position;
            float maxRadius = m_JoystickBack.GetComponent<UnityEngine.UI.Image>().sprite.bounds.extents.y * 10;
            if (diff.magnitude >= maxRadius)
                diff = diff.normalized * maxRadius;

            m_JoystickFront.transform.position = m_JoystickBack.transform.position + diff;

            CharacterControllerRef.MovementDirection = Camera.main.transform.right * diff.x;//new Vector3(VJRnormals.x, 0, VJRnormals.y);
            CharacterControllerRef.MovementDirection += Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.transform.forward.z)) * diff.y;
            CharacterControllerRef.MovementDirection.y = 0;

            movementSpeed = diff.magnitude / maxRadius;
            if(movementSpeed < 0.5f)
            {
                //Debug.Log("Walking");
                CharacterAnimationRef.IsRunning = false;
                CharacterAnimationRef.IsWalking = true;
            }
            else
            {
                //Debug.Log("Running");
                CharacterAnimationRef.IsRunning = true;
                CharacterAnimationRef.IsWalking = false;
            }
            CharacterControllerRef.walkSpeed =  movementSpeed * 5.0f;
            CharacterControllerRef.RotateToLookAtPoint(CharacterControllerRef.transform.position + CharacterControllerRef.MovementDirection * 6);
        }
        else
        {
            //Debug.Log("Not Moving");
            m_JoystickFront.transform.localPosition = m_JoystickBack.transform.localPosition;
            CharacterAnimationRef.IsRunning = false;
            CharacterAnimationRef.IsWalking = false;
            CharacterControllerRef.MovementDirection = Vector3.zero;
        }

	}
    private bool isHittingThumbBack(Vector3 position){

        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position =  position;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll( pe, hits );

        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject == m_JoystickBack)
            {
                return true;
            }
        }
        return false;
    }

    public void JumpButton(){
        if (m_Paused)
            return;
        CharacterControllerRef.PressedJumb = true;
    }
    public void SetPause(bool On){
        m_Paused = On;
        m_PauseMenu.SetActive(On);
        m_PauseButton.SetActive(!On);
        UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().Paused = On;
    }
    public void ExitGame(){
        MinigameScript.ExitMinigame(false);
    }
    public void ToggleSound(){
        //ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled = !ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled;


        AudioListener audio = FindObjectOfType<AudioListener> ();
        if(audio != null)
        {
            audio.enabled = !audio.enabled;
        }
    }
}
