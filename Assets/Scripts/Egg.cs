using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {
	
	[SerializeField]
	new Collider collider;
	
	[SerializeField]
	Renderer eggModel;

	[SerializeField]
	new ParticleSystem particleSystem;
	
	[SerializeField]
	int numParticlesPerTap;

	public bool Tap()
	{
		ProfilesManagementScript.Instance.CurrentAnimin.EggTaps++;
		particleSystem.Emit(numParticlesPerTap);
		
		// After enough taps remove the egg
		if (ProfilesManagementScript.Instance.CurrentAnimin.Hatched)
		{
			eggModel.enabled = false;
			collider.enabled = false;
			StartCoroutine(RemoveEgg(5));
			return true;
		}
		return false;
	}

	IEnumerator RemoveEgg(float delay)
	{
		yield return new WaitForSeconds( delay );
		if(gameObject)
		{
			Destroy(gameObject);
		}
	}

}
