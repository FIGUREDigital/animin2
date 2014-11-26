using UnityEngine;
using System.Collections;

public class BlinkRef : MonoBehaviour {

    [SerializeField]
    private Texture BlinkingTexture;

    public Texture Blink
    {
        get
        {
            return BlinkingTexture;
        }
    }
}
