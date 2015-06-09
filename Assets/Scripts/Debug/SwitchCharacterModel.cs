using UnityEngine;
using System.Collections;

public class SwitchCharacterModel : MonoBehaviour {

    public void switchModel(int i){
        Debug.Log("Switch Model");
        ProfilesManagementScript.Instance.CurrentAnimin = ProfilesManagementScript.Instance.CurrentProfile.Characters[i];
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, !ProfilesManagementScript.Instance.CurrentAnimin.Hatched);
    }

    public void switchEvo(int i){
        if (!(i >= 0 && i <= 2))
            return;
        Debug.Log("Switch Evolution");
        ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId = (AniminEvolutionStageId)i;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, !ProfilesManagementScript.Instance.CurrentAnimin.Hatched);
    }
    public void AddZef(int amt){
        EvolutionManager.Instance.AddZef(amt);
    }
    public void AddAllItems(){


        for (int i = 0; i < (int)InventoryItemId.Count; i++){
            ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory((InventoryItemId)i, 1);
        }
    }
}
