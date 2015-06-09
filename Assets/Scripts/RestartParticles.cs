using UnityEngine;
using System.Collections;

public class RestartParticles : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		ParticleSystem ps = GetComponent<ParticleSystem>();
		ps.Stop();
		ps.Clear();
		ps.Play();
	}

}
