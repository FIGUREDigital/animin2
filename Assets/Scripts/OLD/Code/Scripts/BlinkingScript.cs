using UnityEngine;
using System.Collections;

public class BlinkingScript : MonoBehaviour
{
    [SerializeField]
    private Texture EyesOpenTexture, BlinkingTexture;

    public Texture Blink { get { return BlinkingTexture; } }

    [SerializeField]
    private Renderer[] TexturesToSwap;

    private bool m_IsBlinking;
    private float m_BlinkTimer;


    void Awake()
    {

    }

    void Update()
    {

        if (EyesOpenTexture == null || BlinkingTexture == null)
            return;

        m_BlinkTimer += Time.deltaTime;
        if (!m_IsBlinking)
        {
            if (m_BlinkTimer >= 1f)
            {
                m_BlinkTimer = 0;
                if (UnityEngine.Random.value <= 0.5f)
                {
                    m_IsBlinking = true;
                    ReplaceTexture(BlinkingTexture);
                }
            }
        }
        else
        {
            if (m_BlinkTimer >= 0.2f)
            {
                m_BlinkTimer = 0;
                m_IsBlinking = false;
                ReplaceTexture(EyesOpenTexture);
            }

        }
    }

    void ReplaceTexture(Texture tex)
    {
        for (int i = 0; i < TexturesToSwap.Length; i++)
        {
            TexturesToSwap[i].material.mainTexture = tex;
        }
    }
}
