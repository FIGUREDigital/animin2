using UnityEngine;
using System.Collections;

public class SplatFadeScript : MonoBehaviour 
{
	private float Timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Timer += Time.deltaTime;

		if(Timer >= 10)
		{
			float alpha = this.renderer.material.color.a;
			alpha -= Time.deltaTime;
			this.renderer.material.color = new Color(1,1,1, alpha);

			if(alpha <= 0)
			{
				alpha = 0;
				//UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Remove(this.gameObject);
				Destroy(this.gameObject);
			}

		}
	
	}
}
