using UnityEngine;
using System.Collections;
/*
public class SwipeDragScript : UIDragDropItem
{
	
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>
	
	//public GameObject prefab;
	
	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>
	
	protected override void OnDragDropStart ()
	{
		//this.gameObject.GetComponent<BoxCollider>().enabled = false;
		
		base.OnDragDropStart ();
	}
	
	protected override void OnDragDropRelease (GameObject surface)
	{
		Debug.Log("SWIPE DROP!!!!!!");
		if(surface != null) Debug.Log(surface.name);
		else Debug.Log("the surface is null");


		CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

		if(surface != null && (surface.tag == "Items" || surface.tag == "Shit") && script.GroundItems.Contains(surface))
		{
			Debug.Log("clearing");
			script.GroundItems.Remove(surface);
			Destroy(surface);

		}

		
		base.OnDragDropRelease(surface);
	}
}
*/