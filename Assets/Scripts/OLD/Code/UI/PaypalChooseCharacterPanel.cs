using UnityEngine;
using System.Collections;

public class PaypalChooseCharacterPanel : MonoBehaviour {

    public PersistentData.TypesOfAnimin AniminType;	
    public decimal Price = 4.99m;
	
	// Update is called once per frame
	void OnClick () 
    {
        UIGlobalVariablesScript.Singleton.LaunchWebview(AniminType, Price);
        this.gameObject.transform.parent.gameObject.SetActive(false);
	}

    void OnEnable()
    {
        if (AniminType == PersistentData.TypesOfAnimin.Tbo)
        {
            this.gameObject.SetActive(false);
        }


		for (int i =0; i < ProfilesManagementScript.StateData.UnlockedAnimins.Count; i++ )
        {
			if (AniminType ==  ProfilesManagementScript.StateData.UnlockedAnimins[i])
            {
                this.gameObject.SetActive(false);
            }

        }
    }
}
