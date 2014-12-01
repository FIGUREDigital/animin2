using UnityEngine;
using System.Collections;

public class SwitchCharacterModel : MonoBehaviour {

    public void switchModel(int i){
        ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters[i];
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
    }

    public void switchEvo(int i){
        if (!(i >= 0 && i <= 2))
            return;
        ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = (AniminEvolutionStageId)i;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
    }

}
