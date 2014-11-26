using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{

    public PortalId Id;
    private float Timer;

    // Use this for initialization
    void Start()
    {

    }

    public void Show(PortalStageId stageId, bool isJumbingIn)
    {
        this.gameObject.SetActive(true);


        this.transform.GetChild(0).GetComponent<PortalAnimationScript>().IsShowing = true;
        this.transform.GetChild(0).GetComponent<PortalAnimationScript>().isJumpingIn = isJumbingIn;

        if (stageId == PortalStageId.ARscene)
        {
            this.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
        }
        else if (stageId == PortalStageId.NonARScene)
        {
            this.transform.parent = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;
        }
        //this.transform.rotation = Quaternion.identity;
        this.transform.localPosition = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition;
        if (isJumbingIn)
        {

            //[PTLP] HARRY'S MINE : PORTALPOSITION
            
        }
        if (stageId == PortalStageId.NonARScene)
        {

            this.transform.localPosition += new Vector3(0, 0.3f, 0);
        }
        //this.transform.position = Vector3.zero;
        this.transform.localPosition += new Vector3(0, 0, 0.5f);

        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= 3)
        {
            this.transform.GetChild(0).GetComponent<PortalAnimationScript>().IsHiding = true;
        }

        /*if (Input.GetButtonDown("Fire1")) 
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.collider == this.collider)
                {
                    switch(Id)
                    {
                        case PortalId.Garden:
                        {
                            UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(false);
                            UIGlobalVariablesScript.Singleton.GardenSceneRef.SetActive(true);
                            break;
                        }

                        case PortalId.ExitGarden:
                        {
                        UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(true);
                            UIGlobalVariablesScript.Singleton.GardenSceneRef.SetActive(false);
                            break;
                        }
                    }
                }
            }
        }*/
    }
}

public enum PortalId
{
    Garden = 0,
    ExitGarden,
}

public enum PortalStageId{
    ARscene,
    NonARScene,
    MinigameCuberRunners,

}