using UnityEngine;
using System.Collections;

public class LoopingSFX : MonoBehaviour {

    public static AudioSource Instance{
        get { return m_Instance; }
    }
    private static AudioSource m_Instance;

	// Use this for initialization
	void Start () {
        m_Instance = this.GetComponent<AudioSource>();
	}
}
