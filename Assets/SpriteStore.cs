using UnityEngine;
using System.Collections;


public class SpriteStore : MonoBehaviour 
{
	public Sprite[] Sprites = new Sprite[(int)InventoryItemId.Count-1];

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Sprite GetSprite(InventoryItemId s)
	{
		return Sprites[(int)s];
	}
}
