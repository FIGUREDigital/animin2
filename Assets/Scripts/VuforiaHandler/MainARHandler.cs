using UnityEngine;
using System.Collections;

public enum GameScenes
{
	Caring,
	MinigameCubeRunner,
	MinigameCannon,

	Count,
};


public class MainARHandler : MonoBehaviour
{
	[SerializeField]
	private Camera ARCamera;

	private GameScenes m_CurrentGameScene;

	public GameScenes CurrentGameScene {
		get {
			return m_CurrentGameScene;
		}
		set {
			m_CurrentGameScene = value;
		}
	}
	private GameObject CurrentGameSceneGameObject;

	private bool isTracking;
	private AniminTrackableEventHandler mLastTrack;

	private Transform NonARCameraPositionRef;



	// Use this for initialization
	void Start ()
	{
		ChangeSceneToCaring ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isTracking && ARCamera != null && NonARCameraPositionRef!=null) {
			ARCamera.gameObject.transform.position = NonARCameraPositionRef.position;
			ARCamera.gameObject.transform.rotation = NonARCameraPositionRef.rotation;

			CurrentGameSceneGameObject.transform.parent = this.gameObject.transform;
		} else {
			CurrentGameSceneGameObject.transform.parent = mLastTrack.gameObject.transform;
		}
	}

	public void OnTrackableStateChanged (
		AniminTrackableEventHandler AniminTrackScript,
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		mLastTrack = AniminTrackScript;
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		      newStatus == TrackableBehaviour.Status.TRACKED ||
		      newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			OnTrackingFound ();
		} else {
			OnTrackingLost ();
		}
	}



	private void OnTrackingFound ()
	{
		if (!isTracking) {
			isTracking = true;
			Debug.Log ("OnTrackingFound : [" + mLastTrack.TrackableBehaviour.TrackableName + "];");
		}
	}



	private void OnTrackingLost ()
	{
		if (isTracking) {
			isTracking = false;
			Debug.Log ("OnTrackingLost : [" + mLastTrack.TrackableBehaviour.TrackableName + "];");
		}
	}

	public void ChangeSceneToCaring ()
	{
		LoadNewGameScene(GameScenes.Caring);
	}
	public void ChangeSceneToCubeRunner ()
	{
		LoadNewGameScene(GameScenes.MinigameCubeRunner);
	}
	public void ChangeSceneToCannon ()
	{
		LoadNewGameScene(GameScenes.MinigameCannon);
	}
	private void LoadNewGameScene(GameScenes newScene){
		m_CurrentGameScene = newScene;
		UnityEngine.Object.Destroy (CurrentGameSceneGameObject);
		switch (m_CurrentGameScene) {
		case GameScenes.Caring:
			CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/ScenePrefabs/Caring"));
			break;

		case GameScenes.MinigameCubeRunner:
			CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/TestDummies/Cube"));
			break;

		case GameScenes.MinigameCannon:
			CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/TestDummies/Gunner"));
			break;

		}
		NonARCameraPositionRef = CurrentGameSceneGameObject.GetComponent<NonARPosRef> ().NonARCameraPositionReference;
		Debug.Log ("NonARCameraPositionRef : [" + NonARCameraPositionRef + "];");
	}
}
