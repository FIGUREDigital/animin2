﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class JoystickPageControls : MonoBehaviour
{
    public bool Paused;

    private MinigameCollectorScript MinigameScript;
    private MinigameAnimationControllerScript CharacterAnimationRef;
    private CharacterControllerScript CharacterControllerRef;

    [SerializeField]
    private GameObject m_JoystickFront, m_JoystickBack, m_JumpButton;

    private bool isButtonDown = false;
    private int fingerID = -1;

    // Use this for initialization
    void Start(){
        Init();
    }
    void OnEnable(){
        Init();
    }
    void Init () {
        //MinigameScript = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>();
        CharacterControllerRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>();
        CharacterAnimationRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>();

        if (MainARHandler.Get.CurrentGameScene == GameScenes.MinigameCubeRunner)
            m_JumpButton.SetActive(true);
        else if (MainARHandler.Get.CurrentGameScene == GameScenes.MinigameCannon)
            m_JumpButton.SetActive(false);
        
    }
	
    void Update () {

        if (Paused) return;
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
        Debug.Log("Pressed Jump");
        CharacterControllerRef.PressedJumb = true;
    }
}