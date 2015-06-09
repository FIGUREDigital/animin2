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
			if(GetComponent<Renderer>() != null) 
			{
				float alpha = GetComponent<Renderer>().material.color.a;
				alpha -= Time.deltaTime *  3;
				if(alpha <= 0) 
				{
					alpha = 0;
					destroy = true;
				}
				GetComponent<Renderer>().material.shader = Shader.Find("Custom/ItemShader");
				GetComponent<Renderer>().material.color = new Color(
					GetComponent<Renderer>().material.color.r,
					GetComponent<Renderer>().material.color.g,
					GetComponent<Renderer>().material.color.b,
					alpha);
			}
			
			for(int a=0;a<transform.childCount;++a)
			{
				if(transform.GetChild(a).GetComponent<Renderer>() == null) continue;
				
				
				float alpha = transform.GetChild(a).GetComponent<Renderer>().material.color.a;
				alpha -= Time.deltaTime * 3;
				if(alpha <= 0) 
				{
					alpha = 0;
					destroy = true;
				}

				transform.GetChild(a).GetComponent<Renderer>().material.shader = Shader.Find("Custom/ItemShader");

				transform.GetChild(a).GetComponent<Renderer>().material.color = new Color(
					transform.GetChild(a).GetComponent<Renderer>().material.color.r,
					transform.GetChild(a).GetComponent<Renderer>().material.color.g,
					transform.GetChild(a).GetComponent<Renderer>().material.color.b,
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
