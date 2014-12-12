using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour
{

    public PortalId Id;
    private float Timer;

    ParticleSystem ParticleSys
    {
        get
        {
            if (m_System == null)
                m_System = this.transform.GetChild(0).GetComponent<ParticleSystem>();
            return m_System;
        }
    }

    ParticleSystem m_System;

    // Use this for initialization
    void Start()
    {

    }

    public void Show(PortalStageId stageId, bool isJumbingIn)
    {
        this.gameObject.SetActive(true);

        ParticleSys.gameObject.SetActive(true);
        ParticleSys.Play();

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
            this.transform.localPosition += new Vector3(0, 0.2f, 0.1f);
        }
        else if (stageId == PortalStageId.ARscene)
        {
            this.transform.localPosition += new Vector3(0, 0.2f, 0.3f);
        }

        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= 3)
        {
            ParticleSys.Stop();
        }
    }
}

public enum PortalId
{
    Garden = 0,
    ExitGarden,
}

public enum PortalStageId
{
    ARscene,
    NonARScene,
    MinigameCuberRunners,

}