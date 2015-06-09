using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DetectBroomScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [SerializeField]
	private GameObject BroomPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
    }


    public void OnBeginDrag(PointerEventData eventData)
	{
        Debug.Log("Drag Broom");

		GameObject child = GameObject.Instantiate(BroomPrefab) as GameObject;
		child.name = "Broom";

		CameraModelScript.Instance.SetDragging (child);
		child.transform.position = Vector3.zero;
		child.transform.rotation = Quaternion.identity;
		child.transform.localScale *= 8;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localPosition = Vector3.zero;
    }
    public void OnDrag(PointerEventData data)
    {		
		Debug.Log ("Main Camera: "+Camera.main.name);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Broom Hit : ["+hit.collider.gameObject.name+"];");
        }
        if(hit.collider.gameObject.CompareTag("Ground")) // Gross, check the floor layer
        {
            return;
        }
        UIPopupItemScript script = hit.collider.gameObject.GetComponent<UIPopupItemScript>();
        if (script != null)
        {
            ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory(script.Id, 1);
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Remove(hit.collider.gameObject);
            UnityEngine.Object.Destroy(hit.collider.gameObject);
        }
    }
    public void OnEndDrag(PointerEventData data)
    {		
		CameraModelScript.Instance.SetDragging (null);
    }
}
