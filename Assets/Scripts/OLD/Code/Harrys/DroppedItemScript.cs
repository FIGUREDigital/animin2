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
	private UIPopupItemScript m_ItemScript;

	StateEnum State;
	// Use this for initialization
	void Start () {

		m_ItemScript = GetComponent<UIPopupItemScript> ();
		m_ItemScript.NonInteractable = true;
		
		m_VerticalSpeed = 0;

        DisappearUI(UIGlobalVariablesScript.Singleton.StereoUI);
        DisappearUI(UIGlobalVariablesScript.Singleton.PianoUI);
        DisappearUI(UIGlobalVariablesScript.Singleton.JunoUI);
        DisappearUI(UIGlobalVariablesScript.Singleton.EDMBoxUI);
        DisappearUI(UIGlobalVariablesScript.Singleton.AlarmUI);
        DisappearUI(UIGlobalVariablesScript.Singleton.LightbulbUI);
	}
	
	// Update is called once per frame
	void Update () {
		
		m_VerticalSpeed = m_VerticalSpeed + (Time.deltaTime * 600);
		this.transform.position -= new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);

		bool isNonArScene = UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy;
		
		if (this.transform.position.y <= -350) {
			ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(m_ItemScript.Id,1);
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Remove(this.gameObject);



			UnityEngine.Object.Destroy(this.gameObject);
		}
	}
    private void DisappearUI(GameObject go){
        if (go != null && go.activeInHierarchy)
            go.SetActive(false);
    }
}
