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
		ItemLink item = hit.collider.gameObject.GetComponent<ItemLink>();
        if (item != null)
        {
			item.item.MoveTo(Inventory.Locations.Inventory, Vector3.zero);
        }
    }
    public void OnEndDrag(PointerEventData data)
    {		
		CameraModelScript.Instance.SetDragging (null);
    }
}
