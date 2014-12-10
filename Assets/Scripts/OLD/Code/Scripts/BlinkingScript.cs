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

    void Start(){
        m_BlinkTimer = 0;
        m_HasSlept = false;
    }

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

        if (AnimController != null)
        {
            if (AnimController.IsSleeping)
            {
                Debug.Log("SLEEPING EYES CLOSED"); 
                if (!m_IsBlinking)
                    Blink(true);
                return;
            }
            else if (!AnimController.IsSleeping && !m_HasSlept)
            {
                m_HasSlept = true;
                Blink(false);
            }
        }

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
        Debug.Log("Blink : ["+on+"];"); 
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
