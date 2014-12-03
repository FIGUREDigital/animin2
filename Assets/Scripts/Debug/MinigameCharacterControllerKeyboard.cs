using UnityEngine;
using System.Collections;

public class MinigameCharacterControllerKeyboard : MonoBehaviour {

    MinigameAnimationControllerScript CharacterAnimationRef;
    CharacterControllerScript CharacterController;

	// Use this for initialization
	void Start () {
        CharacterController = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>();
        CharacterAnimationRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>();
	}
	
    // Update is called once per frame
    /*
	void Update () {
        Vector3 vel = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            vel += new Vector3(0, 0, 1);
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            vel += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            vel += new Vector3(-1, 0, 0);
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            vel += new Vector3(1, 0, 0);
        }
        vel *= 3;
        CharacterController.MovementDirection = vel;
        if(vel != Vector3.zero)CharacterController.RotateToLookAtPoint(CharacterController.gameObject.transform.position + vel);

        if (!CharacterAnimationRef.IsRunning)CharacterAnimationRef.IsRunning = vel != Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterController.PressedJumb = true;
        }
    }
        */
}
