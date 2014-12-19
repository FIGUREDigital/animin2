using UnityEngine;
using System.Collections;

public class SwitchCharacterModel : MonoBehaviour {

    public void switchModel(int i){
        Debug.Log("Switch Model");
        ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters[i];
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
    }

    public void switchEvo(int i){
        if (!(i >= 0 && i <= 2))
            return;
        Debug.Log("Switch Evolution");
        ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = (AniminEvolutionStageId)i;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
    }
    public void AddZef(int amt){
        EvolutionManager.Instance.AddZef(amt);
    }
    public void AddAllItems(){


        for (int i = 0; i < (int)InventoryItemId.Count; i++){
            ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory((InventoryItemId)i, 1);
        }
    }
}
