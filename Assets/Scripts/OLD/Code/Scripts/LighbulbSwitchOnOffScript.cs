using UnityEngine;
using System.Collections;

public class LighbulbSwitchOnOffScript : MonoBehaviour 
{
	public Texture2D On;
	public Texture2D Off;

	new Light light;

	public void Start()
	{
		light = GetComponentInChildren<Light>();
	}

	public bool IsOn
	{
		get
		{
			return light != null ? light.gameObject.activeSelf : false;
		}
		set
		{			
			if(light)
			{
				light.gameObject.SetActive (value);
			}			
			ChangeTexture(this.gameObject);
		}
	}

	public void Switch()
	{
		if(light != null)
		{
			light.gameObject.SetActive(!light.gameObject.activeSelf);
		}		
		ChangeTexture(this.gameObject);
	}

	public void SetOn()
	{
		IsOn = true;
	}

	public void SetOff()
	{
		IsOn = false;
	}

	private void ChangeTexture(GameObject root)
	{
		Texture2D textured = IsOn ? On : Off;
		if(root.GetComponent<Renderer>() != null)
		{
			//Material mat = root.renderer.sharedMaterial;
			root.GetComponent<Renderer>().material.mainTexture = textured;
			//root.renderer.sharedMaterial = mat;
		}

		for(int i=0;i<root.transform.childCount;++i)
		{
			if(root.transform.GetChild(i).GetComponent<Renderer>() != null)
			{
				//Material mat = root.transform.GetChild(i).renderer.sharedMaterial;
				root.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = textured;
				//root.transform.GetChild(i).renderer.material = mat;
			}
		}
	}
}
