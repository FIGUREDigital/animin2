using UnityEngine;
using System.Collections;

public class DestroyedBarrelScript : MonoBehaviour 
{
	private float Timer = 4;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		Timer -= Time.deltaTime;

		if(Timer <= 0)
		{
			bool destroy = false;
			if(renderer != null) 
			{
				float alpha = renderer.material.color.a;
				alpha -= Time.deltaTime *  3;
				if(alpha <= 0) 
				{
					alpha = 0;
					destroy = true;
				}
				renderer.material.shader = Shader.Find("Custom/ItemShader");
				renderer.material.color = new Color(
					renderer.material.color.r,
					renderer.material.color.g,
					renderer.material.color.b,
					alpha);
			}
			
			for(int a=0;a<transform.childCount;++a)
			{
				if(transform.GetChild(a).renderer == null) continue;
				
				
				float alpha = transform.GetChild(a).renderer.material.color.a;
				alpha -= Time.deltaTime * 3;
				if(alpha <= 0) 
				{
					alpha = 0;
					destroy = true;
				}

				transform.GetChild(a).renderer.material.shader = Shader.Find("Custom/ItemShader");

				transform.GetChild(a).renderer.material.color = new Color(
					transform.GetChild(a).renderer.material.color.r,
					transform.GetChild(a).renderer.material.color.g,
					transform.GetChild(a).renderer.material.color.b,
					alpha);
			}
			
			if(destroy)
			{
				//UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Remove(this.gameObject);
				Destroy(this.gameObject);
			}
		}
		
	
	}
}
