﻿using UnityEngine;
using System.Collections;

public class TestVuforiaScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	

		GameObject obj = GameObject.Find("TextureBufferCamera");
		
		//GameObject hole = GameObject.Find("insideHole");
		
		Texture texture = obj.GetComponent<Camera>().targetTexture;
		this.GetComponent<Renderer>().material.mainTexture = texture;


		this.GetComponent<Renderer>().material.SetTextureScale (
			"_MainTex", 
			new Vector2(0.05f, 0.05f)
			);

		this.GetComponent<Renderer>().material.SetTextureOffset (
			"_MainTex", 
			new Vector2(0.5f, 0.5f)
			);
		
		//GUI.DrawTexture(new Rect(0, 0, 200, 200), texture);
	}
}
