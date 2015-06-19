using UnityEngine;
using System.Collections;

public class BroomScript : MonoBehaviour
{
    public void ClearAllGroundObjects()
    {
        CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

        for (int i = 0; i < script.GroundItems.Count; ++i)
        {
            if (script.GroundItems[i].GetComponent<ItemDefinition>() != null)
            {
				if (script.GroundItems[i].GetComponent<ItemDefinition>().ItemType == PopupItemType.Token)
                {
                    continue;
                }
                else
                {
                    ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory(script.GroundItems[i].GetComponent<ItemDefinition>().Id, 1);
                }
            }
            Destroy(script.GroundItems[i]);
        }

        script.GroundItems.Clear();
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CleanPooPiss);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().HidePopupMenus(false);

        for (int i = 0; i < EDMMixerScript.Singleton.KeysOn.Length; ++i)
        {
            EDMMixerScript.Singleton.KeysOn[i] = false;
        }
    }

}
