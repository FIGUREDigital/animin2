using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ParentalGateway : MonoBehaviour 
{
	private GatewayButton[] buttons;
	private GameObject prevScreen;
	private GameObject nextScreen;

	[SerializeField]
	GameObject EquationScreen;
	
	[SerializeField]
	GameObject PasswordScreen;

	[SerializeField]
	Text Question;


    private const int Min = 100;
    private const int Max = 500;
    private int GetRandRange {
        get{
            return Random.Range(Min,Max);
        }
    }

	private bool mLogPurchase;

	public void Open(GameObject prev, GameObject next, bool LogPurchase = false)
	{
		mLogPurchase = LogPurchase;
		prevScreen = prev;
		nextScreen = next;
		prevScreen.SetActive(false);
		this.gameObject.SetActive(true);

		bool passwordSet = (PlayerPrefs.GetString ("ParentalPassword") != null && PlayerPrefs.GetString ("ParentalPassword") != "");
		EquationScreen.SetActive (!passwordSet);
		PasswordScreen.SetActive (passwordSet);

		buttons = GetComponentsInChildren<GatewayButton> ();

        int result = GetRandRange;
		int first = Random.Range (0, result);
		int second = result - first;

		int correct = Random.Range (0, buttons.Length);
		for (int i =0; i < buttons.Length; i++){
			if (i == correct){
				buttons[i].GetComponentInChildren<Text>().text = second.ToString();
				buttons[i].Active = true;
			} else {
                buttons[i].GetComponentInChildren<Text>().text = (GetRandRange.ToString());
				buttons[i].Active = false;
			}
		}
		Question.text = (first.ToString () + " + X = " + result.ToString ());
	}

	public void Pass()
	{
		if(mLogPurchase)
		{
			FlurryLogger.Instance.CharacterPurchasedWeb();
		}
		Destroy(this.gameObject, 0);
		nextScreen.SetActive(true);
	}

	public void Fail()
	{
		Destroy(this.gameObject, 0);
		prevScreen.SetActive(true);
	}
}
