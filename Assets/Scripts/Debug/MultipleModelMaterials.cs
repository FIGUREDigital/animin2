using UnityEngine;
using System.Collections;

public class MultipleModelMaterials : MonoBehaviour {

	[SerializeField]
	new MeshRenderer renderer;
	
	[SerializeField]
	ParticleSystem particles;
	[SerializeField]
	int index = 0;
	
	[SerializeField]
	Material[] materials;

	public int Index
	{
		set
		{
			index = value;
			SetMat();
		}
	}

	void OnValidate()
	{
		SetMat ();
	}
	// Use this for initialization
	void SetMat ()
	{		
		index = index % materials.Length;
		if(renderer != null)
		{
			renderer.material = materials[index];
		}
		if(particles != null)
		{
			particles.GetComponent<Renderer>().material = materials[index];
		}
	}
	void Awake()
	{
		SetMat();
	}
}
