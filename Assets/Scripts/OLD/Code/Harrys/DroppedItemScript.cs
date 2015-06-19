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
	private ItemDefinition m_Item;

    CaringPageControls m_CaringPageControls;

	StateEnum State;
	// Use this for initialization
	void Start () {

		m_Item = GetComponent<ItemDefinition> ();
		
		m_VerticalSpeed = 0;

        m_CaringPageControls = UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>();

	}
	
	// Update is called once per frame
	void Update () {
		
		m_VerticalSpeed = m_VerticalSpeed + (Time.deltaTime * 600);
		this.transform.position -= new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);

//		bool isNonArScene = UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy;
		
		if (this.transform.position.y <= -350) {
			ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory(m_Item.Id,1);
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Remove(this.gameObject);

            m_CaringPageControls.DisappearAllItemUIs();

			UnityEngine.Object.Destroy(this.gameObject);
		}
	}
}
