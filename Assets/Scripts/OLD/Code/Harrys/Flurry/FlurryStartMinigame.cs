using UnityEngine;
using System.Collections;

public class FlurryStartMinigame : MonoBehaviour {


	[SerializeField]
	private Minigame MiniGame;
	// Use this for initialization
	void Start () {
		FlurryLogger.Instance.StartMinigame(MiniGame);
		UnityEngine.Object.Destroy (this.gameObject);
	}
}
