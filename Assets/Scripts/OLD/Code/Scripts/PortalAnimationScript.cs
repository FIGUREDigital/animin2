using UnityEngine;
using System.Collections;

public class PortalAnimationScript : MonoBehaviour
{

    public float Speed = 1;
    public bool IsShowing;
    public bool IsHiding;

    public bool isJumpingIn;

    MeshRenderer[] m_Renderers;

    // Use this for initialization
    void Start()
    {

        m_Renderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < m_Renderers.Length; i++)
        {
            m_Renderers[i].material.color = new Color(1, 1, 1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //this.transform.localRotation = Quaternion.Euler(0, 0, Time.time * Speed);
        //		Debug.Log(renderer.material.color.ToString());
        if (IsShowing)
        {
            //Debug.Log("IsShowing = true");
            //this.transform.localScale = new Vector3(160,160,160);
            float alpha = m_Renderers[0].material.color.a + Time.deltaTime * 2;
            if (alpha >= 1)
            {
                alpha = 1;
                IsShowing = false;
            }
            for (int i = 0; i < m_Renderers.Length; i++)
            {
                m_Renderers[i].material.color = new Color(1, 1, 1, alpha);
            }
        }
        else if (IsHiding)
        {
            //Debug.Log("IsHiding = true");
            float alpha = m_Renderers[0].material.color.a - Time.deltaTime;
            if (alpha <= 0)
            {
                alpha = 0;
                IsHiding = false;
                this.gameObject.transform.parent.gameObject.SetActive(false);
            }
            for (int i = 0; i < m_Renderers.Length; i++)
            {
                m_Renderers[i].material.color = new Color(1, 1, 1, alpha);
            }

            //this.transform.localScale = Vector3.Lerp(new Vector3(160,160,160), new Vector3(20, 20, 20), 1 - alpha);
        }

        if (isJumpingIn)
        {
            //WARNING. HARDCODED STUFF. I tried to make it track the camera, but the AR Doesn't like it. That means if it ever moves, we're done for. Or we have to reset it.
            this.transform.eulerAngles = new Vector3(-22.67542f, 180, 0);
            Debug.DrawLine(this.transform.position, new Vector3(-22.67542f, 180, 0), Color.red);
        }
        else
        {
            if( UIGlobalVariablesScript.Singleton.ARCamera != null )
            {
                transform.LookAt( UIGlobalVariablesScript.Singleton.ARCamera.transform );
            }
            
        }


        Transform outer = this.transform.Find("Portal");
        if (outer != null)
        {
            outer.Rotate(outer.up, 15f, Space.World);
        }
    }
}