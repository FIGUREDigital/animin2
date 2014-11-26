using UnityEngine;
using System.Collections;

public class DetectDragIconScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnDragStart ()
	{
		InterfaceItemLinkToModelScript refScript = this.GetComponent<InterfaceItemLinkToModelScript>();
		if(refScript.ItemID == InventoryItemId.None) return;

		UIGlobalVariablesScript.Singleton.DragableUI3DObject.GetComponent<CameraModelScript>().SpriteRef = this.gameObject;
		//InterfaceItemLinkToModelScript popScript = refScript.Reference.GetComponent<InterfaceItemLinkToModelScript>();



		GameObject resourceLoaded = (GameObject)Resources.Load(InventoryItemData.Items[(int)refScript.ItemID].PrefabId);
		GameObject child = (GameObject)GameObject.Instantiate(resourceLoaded);

		child.GetComponent<BoxCollider>().enabled = false;

		child.transform.parent = UIGlobalVariablesScript.Singleton.DragableUI3DObject.transform;
		child.transform.position = Vector3.zero;
		child.transform.rotation = Quaternion.identity;
		child.transform.localScale = Vector3.one;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localPosition = Vector3.zero;

		child.transform.localRotation = Quaternion.Euler(0, 180, 0);

		if (refScript.ItemID == InventoryItemId.woodSword)
						child.transform.localScale *= 2.5f;
		else if (refScript.ItemID == InventoryItemId.woodFrame)
						child.transform.localScale *= 3f;
	}
}
