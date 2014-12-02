using UnityEngine;
using System.Collections;

public class BroomScript : MonoBehaviour 
{
	public void ClearAllGroundObjects()
	{
		CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript> ();

		for (int i = 0; i < script.GroundItems.Count; ++i) {
				if (script.GroundItems [i].GetComponent<UIPopupItemScript> () != null) {
						if (script.GroundItems [i].GetComponent<UIPopupItemScript> ().Type == PopupItemType.Token) {
								continue;
						} else {
								ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory (script.GroundItems [i].GetComponent<UIPopupItemScript> ().Id, 1);
						}
				}
				Destroy (script.GroundItems [i]);
		}

		script.GroundItems.Clear ();
		UIGlobalVariablesScript.Singleton.SoundEngine.Play (GenericSoundId.CleanPooPiss);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript> ().HidePopupMenus ();

		for (int i = 0; i < EDMMixerScript.Singleton.KeysOn.Length; ++i) {
				EDMMixerScript.Singleton.KeysOn [i] = false;
		}
}

}
