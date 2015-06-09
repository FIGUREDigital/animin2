using UnityEngine;
using System.Collections.Generic;

public class FlashObjectScript : MonoBehaviour 
{
	private enum StateId
	{
		Intro,
		FlashUp1,
		FlashUp2,
		FlashDown1,
		FlashDown2,
		Outro,
	}

	private Shader FlashShader;
	private Dictionary<Material, Shader> savedShader = new Dictionary<Material, Shader>();
	private StateId State;
	private float Lerp;
	//private Dictionary<GameObject, Shader> savedObjects; 


	private void SaveShaders(GameObject gameObject)
	{
		if(gameObject.GetComponent<Renderer>() != null)
		{
			if(gameObject.GetComponent<Renderer>().material != null)
			{
				if(!savedShader.ContainsKey(gameObject.GetComponent<Renderer>().material))
					savedShader.Add(gameObject.GetComponent<Renderer>().material, gameObject.GetComponent<Renderer>().material.shader);
			}
		}
		
		for(int i=0;i<gameObject.transform.childCount;++i)
		{
			SaveShaders(gameObject.transform.GetChild(i).transform.gameObject);
		}
	}

	private void RestoreShaders(GameObject gameObject)
	{
		if(gameObject.GetComponent<Renderer>() != null)
		{
			if(gameObject.GetComponent<Renderer>().material != null)
			{
				gameObject.GetComponent<Renderer>().material.shader = savedShader[gameObject.GetComponent<Renderer>().material];
				//if(!savedShader.ContainsKey(gameObject.renderer.material))
				//	savedShader.Add(gameObject.renderer.material, gameObject.renderer.material.shader);
			}
		}
		
		for(int i=0;i<gameObject.transform.childCount;++i)
		{
			RestoreShaders(gameObject.transform.GetChild(i).transform.gameObject);
		}
	}

	private void RecursiveSetShader(GameObject gameObject, Shader shader)
	{
		if(gameObject.GetComponent<Renderer>() != null)
		{
			if(gameObject.GetComponent<Renderer>().material != null)
			{
				//if(gameObject.renderer.material.shader != savedShader)
				//	savedShader = gameObject.renderer.material.shader;

				gameObject.GetComponent<Renderer>().material.shader = shader;
			}
		}

		for(int i=0;i<gameObject.transform.childCount;++i)
		{
			RecursiveSetShader(gameObject.transform.GetChild(i).transform.gameObject, shader);
		}
	}

	private void SetBlendFactor(GameObject gameObject)
	{
		if(gameObject.GetComponent<Renderer>() != null)
		{
			if(gameObject.GetComponent<Renderer>().material != null)
			{
				gameObject.GetComponent<Renderer>().material.SetFloat("_BlendFactor", Lerp);
			}
		}
		
		for(int i=0;i<gameObject.transform.childCount;++i)
		{
			SetBlendFactor(gameObject.transform.GetChild(i).transform.gameObject);
		}
	}

	void Update() 
	{
		switch(State)
		{
		case StateId.Intro:
			FlashShader = Shader.Find("Custom/Flash Select");
			if(FlashShader == null) Debug.Log("THE FUCKING SHADER IS NULL");
			SaveShaders(this.gameObject);
			RecursiveSetShader(this.gameObject, FlashShader);
			Lerp = 0;
			State = StateId.FlashUp1;
			SetBlendFactor(this.gameObject);
			break;

		case StateId.FlashUp1:
		case StateId.FlashUp2:

			Lerp += Time.deltaTime * 9;
			if(Lerp >= 1) 
			{
				Lerp = 1;
				State = StateId.FlashDown1;
			}

			SetBlendFactor(this.gameObject);
			break;

		case StateId.FlashDown1:
		case StateId.FlashDown2:
			Lerp -= Time.deltaTime * 9;
			if(Lerp <= 0) 
			{
				Lerp = 0;
				State = StateId.Outro;
			}
			
			SetBlendFactor(this.gameObject);
			break;

		case StateId.Outro:
			RestoreShaders(this.gameObject);
			//RecursiveSetShader(this.gameObject, savedShader);
			Destroy(this);
			break;
		}

		
	}
}