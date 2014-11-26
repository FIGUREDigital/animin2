using UnityEngine;
using System.Collections;

public class DetectBroomScript : MonoBehaviour {

	public GameObject BroomPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDragStart ()
	{
		//UIGlobalVariablesScript.Singleton.DragableUI3DObject.GetComponent<CameraModelScript>().SpriteRef = this.gameObject;


		GameObject child = GameObject.Instantiate(BroomPrefab) as GameObject;
		child.name = "Broom";

		//child.GetComponent<SphereCollider>().enabled = false;
		
		//child.GetComponent<BoxCollider>().enabled = false;
		
		child.transform.parent = UIGlobalVariablesScript.Singleton.DragableUI3DObject.transform;
		child.transform.position = Vector3.zero;
		child.transform.rotation = Quaternion.identity;
		//child.transform.localScale = Vector3.one;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localPosition = Vector3.zero;

	}
}
