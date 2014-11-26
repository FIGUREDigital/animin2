using UnityEngine;
using System.Collections;

public enum PopupItemType
{
	Food = 0,
	Item,
	Medicine,
	Token,
	Count
}

public enum MenuFunctionalityUI
{
	None = 0,
	Mp3Player,
	Clock,
	Lightbulb,
	EDMBox,
	Juno,
	Piano,
	Alarm,
}

public enum SpecialFunctionalityId
{
	None,
	Liquid,
	Injection,

}

public class UIPopupItemScript : MonoBehaviour 
{
	public int Points;
	public PopupItemType Type;
	//public GameObject Model3D;
	public bool NonInteractable;
	public MenuFunctionalityUI Menu;
	public SpecialFunctionalityId SpecialId;
	public InventoryItemId Id;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void Awake(){
		//Physics.IgnoreCollision
	}
	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log ("BLARRRRRRRRRGH : "+collisionInfo.collider.name);
	}
}
