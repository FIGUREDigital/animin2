using UnityEngine;
//using System.Collections;


public enum GameState { NULL, GAME_MENU, LEADERBOARD, MULTIPLAYER_MENU, USERNAME, GAME }
public enum GameType { SOLO, NETWORK }
public enum MultiplayerType { FRIEND_START, FRIEND_JOIN, RANDOM }


public class GameController : MonoBehaviour 
{
	public static 	GameController		instance;					// SINGLETON, STORE REFERENCE

	private  		GameState			__gameState;
	private  		GameType			__gameType;
	private  		MultiplayerType		__multiplayerType;
	private  		string				__username;
	private  		string				__friendUsername;



	// CORE

	public void Awake() {
		instance = this;

		__username = "shaun";

		SetFriendUsername("shaun");
		
		if (Application.loadedLevelName == "shaun") __gameState = GameState.GAME_MENU;
	}

//	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//	{
//		if (stream.isWriting)
//		{
//
//		}
//		else
//		{
//
//		}
//	}

	public void OnEnable() {}

	public void Start () {}

	public void Update () {}

	public void SetSinglePlayer()
	{
		SetGameType(GameType.SOLO);
		EnterGame();
	}


	public void SetMultiplayerJoinRandom()
	{
		Debug.Log("STARTING MULTIPLAYER");

		SetGameType(GameType.NETWORK);
		SetMultiplayerType(MultiplayerType.RANDOM);
		EnterGame();
	}

	public void SetMultiplayerStartFriendGame()
	{
		SetGameType(GameType.NETWORK);
		SetMultiplayerType(MultiplayerType.FRIEND_START);
		EnterGame();
	}

	public void SetMultiplayerJoinFriend()
	{
		SetGameType(GameType.NETWORK);
		SetMultiplayerType(MultiplayerType.FRIEND_JOIN);
		EnterGame();
	}

	public void OnGUI() {
		//Debug.Log("mannnnss	:	"	+	__gameState);

		///*
		/*if (__gameState == GameState.GAME_MENU) {
			float buttonWidth = Screen.width * 0.2F;
			float buttonHeight = Screen.height * 0.1F;
			if (GUI.Button(new Rect(Screen.width * 0.5F - (buttonWidth * 0.5F), Screen.height * 0.5F - (buttonHeight * 2.0F), buttonWidth, buttonHeight), "SINGLE PLAYER")) {
				SetGameType(GameType.SOLO);
				EnterGame();
			}
			if (GUI.Button(new Rect(Screen.width * 0.5F - (buttonWidth * 0.5F), Screen.height * 0.5F - (buttonHeight * 0.5F), buttonWidth, buttonHeight), "MULTI PLAYER")) {
				SetGameType(GameType.NETWORK);
				SetMultiplayerType(MultiplayerType.RANDOM);
				EnterGame();
			}
			if (GUI.Button(new Rect(Screen.width * 0.5F - (buttonWidth * 0.5F), Screen.height * 0.5F + (buttonHeight * 1.0F), buttonWidth, buttonHeight), "LEADERBOARD")) {
			}
		}

		if (Application.loadedLevelName == "shaun") {
			if (__gameState == GameState.GAME) {
				float buttonWidth = Screen.width * 0.1F;
				float buttonHeight = Screen.height * 0.05F;

				if (__gameType == GameType.NETWORK) {
					if (PhotonNetwork.connectionStateDetailed == PeerState.Joined) {
						if (GUI.Button(new Rect(Screen.width * 0.5F - (buttonWidth * 0.5F), 0, buttonWidth, buttonHeight), "Exit")) StopGame();
						GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
					}
				} else {
					if (GUI.Button(new Rect(Screen.width * 0.5F - (buttonWidth * 0.5F), 0, buttonWidth, buttonHeight), "Exit")) StopGame();
				}
			}
		}*/
		//*/
	}


	// PUBLIC

	public GameState gameState { get { return __gameState;} }
	public GameType gameType { get { return __gameType;} }
	public MultiplayerType multiplayerType { get { return __multiplayerType;} }
	public string username { get { return __username;} }
	public string friendUsername { get { return __friendUsername;} }

	public void SetGameState(GameState state) { __gameState = state; }
	public void SetGameType(GameType type) { __gameType = type; }
	public void SetMultiplayerType(MultiplayerType type) { __multiplayerType = type; }
	public void SetFriendUsername(string username) { __friendUsername = username; }

	public void StartGame() {
		/*if (Application.loadedLevelName == "shaun") {
			ShaunScene.instance.LoadModel();
		} else {*/
		UIClickButtonMasterScript.HandleClick(UIFunctionalityId.StartSelectedMinigame, null);
		//}
	}

	public void StopGame() {

//		if (PhotonNetwork.connected) 
//			PhotonNetwork.Disconnect();
		
		/*if (Application.loadedLevelName == "shaun") {
			__gameState = GameState.GAME_MENU;
			ShaunScene.instance.DestroyModel();
		} else {*/
			__gameState = GameState.NULL;
		//}
	}


	// PRIVATE

	private void EnterGame() {
		__gameState = GameState.GAME;

		if (__gameType == GameType.SOLO) {
			/*if (Application.loadedLevelName == "shaun") {
				StartGame();
			} else {*/
				//UIClickButtonMasterScript.HandleClick(UIFunctionalityId.PlayMinigameGunFighters, null);
			//}
		} else 
		{
			Debug.Log("PhotonNetwork.ConnectUsingSettings();");
			//PhotonNetwork.ConnectUsingSettings("0.1");
		}
	}

//	[RPC]
//	public void ReceiveTest(float level)
//	{
////		Debug.Log("received: level: " + level.ToString());
//	}
//
//	public void SendTest(float level)
//	{
//		GetComponent<PhotonView>().RPC("ReceiveTest", PhotonTargets.All, level);
//	}
}