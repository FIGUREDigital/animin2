using UnityEngine;
using System.Collections;

public class BlinkingScript : MonoBehaviour
{
    [SerializeField]
    private Texture EyesOpenTexture, BlinkingTexture;

    [SerializeField]
    private Renderer[] TexturesToSwap;

    private bool m_IsBlinking;
    private float m_BlinkTimer;


    private AnimationControllerScript AnimController{
        get {
            if (m_AnimationController == null)
                m_AnimationController = GetComponentInParent<AnimationControllerScript>();
            return m_AnimationController;
        }
    }
    private AnimationControllerScript m_AnimationController;

    private bool m_HasSlept;

    void Update()
    {
        if (EyesOpenTexture == null || BlinkingTexture == null)
            return;

        if (AnimController != null && !m_HasSlept)
        {
            if (AnimController.IsSleeping&& !m_IsBlinking)
            {
                Debug.Log("EYES CLOSED"); 
                Blink(true);
            }
            else if (!AnimController.IsSleeping && m_IsBlinking)
            {
                Debug.Log("EYES OPEN"); 
                m_HasSlept = true;
                Blink(false);
            }
            return;
        }

        Debug.Log("I SHOULDN'T GET HERE IF I AM SLEEPING"); 


        m_BlinkTimer += Time.deltaTime;
        if (!m_IsBlinking)
        {
            if (m_BlinkTimer >= 1f)
            {
                m_BlinkTimer = 0;
                if (UnityEngine.Random.value <= 0.5f)
                {
                    Blink(true);
                }
            }
        }
        else
        {
            if (m_BlinkTimer >= 0.2f)
            {
                Blink(false);
                ReplaceTexture(EyesOpenTexture);
            }

        }
    }
    void Blink(bool on){
        m_IsBlinking = on;
        ReplaceTexture(on ? BlinkingTexture : EyesOpenTexture);
    }
    void ReplaceTexture(Texture tex)
    {
        for (int i = 0; i < TexturesToSwap.Length; i++)
        {
            TexturesToSwap[i].material.mainTexture = tex;
        }
    }
}
