using UnityEngine;
using System.Collections;

public class DroppedItemScript : MonoBehaviour {
	private enum StateEnum
	{
		None = 0,
		Begin,
		Falling,
		End,
	}
	private float m_VerticalSpeed;
	private ItemLink m_Item;

//    CaringPageControls m_CaringPageControls;

	StateEnum State;
	// Use this for initialization
	void Start () 
	{
		m_Item = GetComponent<ItemLink> ();		
		m_VerticalSpeed = 0;
//        m_CaringPageControls = UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>();

	}
	
	// Update is called once per frame
	void Update () {
		
		m_VerticalSpeed = m_VerticalSpeed + (Time.deltaTime * 600);
		this.transform.position -= new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);

//		bool isNonArScene = UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy;
		
		if (this.transform.position.y <= -350) 
		{
			m_Item.item.MoveTo(Inventory.Locations.Inventory, Vector3.zero);
			//ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory(m_Item.Id,1);
			//UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Remove(this.gameObject);

//            m_CaringPageControls.DisappearAllItemUIs();


			Destroy(this); // Remove this script we are no longer falling
		}
	}
}
