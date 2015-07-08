using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterChoiceManager {

	#region Singleton

    private static CharacterChoiceManager s_Instance;

    public static CharacterChoiceManager Instance
	{
			get
			{
					if( s_Instance == null )
					{
                s_Instance = new CharacterChoiceManager();
					}
					return s_Instance;
			}
	}

	#endregion

    public List<CharacterChoiceItem> CharactersToPick;

    public bool Initialised;

    public void FindCharacterChoices (GameObject characterChoiceParentPanel) 
    {
        Debug.Log("Managing character choices since 1805");
        if (!Initialised)
        {
            CharactersToPick = new List<CharacterChoiceItem>();
            CharacterChoiceItem[] tempArray = characterChoiceParentPanel.GetComponentsInChildren<CharacterChoiceItem>();
            foreach (CharacterChoiceItem choice in tempArray)
            {
                CharactersToPick.Add(choice);
            }
            Initialised = true;
            InitialCharacterLock();
            Debug.Log("Character portrait count "+CharactersToPick.Count);
        }
	}

    public void InitialCharacterLock()
    {
        for (int i=0; i < CharactersToPick.Count; i++)
        {
            CharactersToPick[i].ChangeLockedState(false);
        }
    }

    public void UnlockCharacterPortrait(PersistentData.TypesOfAnimin typeToUnlock)
    {
        /*if (typeToUnlock == PersistentData.TypesOfAnimin.TboAdult)
        {
            for (int i = 0; i< CharactersToPick.Count; i++)
            {
                if (CharactersToPick[i].ThisCharacter == PersistentData.TypesOfAnimin.Tbo)
                {
                    CharactersToPick[i].ThisCharacter = PersistentData.TypesOfAnimin.TboAdult;
                    CharactersToPick[i].CharacterClickScript.Animin = PersistentData.TypesOfAnimin.TboAdult;
                }
            }
        }*/

//        for (int i = 0; i< CharactersToPick.Count; i++)
//        {
//            if (CharactersToPick[i].ThisCharacter == typeToUnlock)
//            {
//                CharactersToPick[i].ChangeLockedState(true);
//            }
//        }

    }
	
}
