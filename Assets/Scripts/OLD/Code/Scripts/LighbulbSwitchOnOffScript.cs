using UnityEngine;
using System.Collections;

public class LighbulbSwitchOnOffScript : MonoBehaviour 
{
	public Texture2D On;
	public Texture2D Off;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	public void Switch()
	{
		for(int i=0;i<transform.childCount;++i)
		{
			if(transform.GetChild(i).GetComponent<Light>() != null)
			{
				if(transform.GetChild(i).gameObject.activeSelf)
				{
					SetOff();
				}
				else
				{
					SetOn();
				}
				return;
			}
		}
	}

	public void SetOn()
	{
		ChangeTexture(this.gameObject, On);

		for(int i=0;i<transform.childCount;++i)
		{
			if(transform.GetChild(i).GetComponent<Light>() != null)
			{
				transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}

	public void SetOff()
	{
		ChangeTexture(this.gameObject, Off);

		for(int i=0;i<transform.childCount;++i)
		{
			if(transform.GetChild(i).GetComponent<Light>() != null)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	private void ChangeTexture(GameObject root, Texture2D textured)
	{
		if(root.renderer != null)
		{
			//Material mat = root.renderer.sharedMaterial;
			root.renderer.material.mainTexture = textured;
			//root.renderer.sharedMaterial = mat;
		}

		for(int i=0;i<root.transform.childCount;++i)
		{
			if(root.transform.GetChild(i).renderer != null)
			{
				//Material mat = root.transform.GetChild(i).renderer.sharedMaterial;
				root.transform.GetChild(i).renderer.material.mainTexture = textured;
				//root.transform.GetChild(i).renderer.material = mat;
			}
		}
	}
}
