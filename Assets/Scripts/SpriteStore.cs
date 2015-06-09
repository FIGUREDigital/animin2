using UnityEngine;
using System.Collections;


public class SpriteStore : MonoBehaviour 
{
	public Sprite[] Sprites = new Sprite[(int)InventoryItemId.Count];
	public Sprite[] MedelSprites = new Sprite[(int)AchievementMedels.Count];

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Sprite GetSprite(InventoryItemId s)
	{
		int i = (int)s;
		return Sprites[i];
	}
	public Sprite GetMedel(AchievementMedels s)
	{
		return MedelSprites[(int)s];
	}
}
