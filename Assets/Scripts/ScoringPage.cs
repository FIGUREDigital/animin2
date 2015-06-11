using UnityEngine;
using System.Collections;

public class ScoringPage : MonoBehaviour {

	[System.Serializable]
	public class MedalVisual
	{
		public Color glow;
		public Color bar;
		public Sprite sprite;
	}

	public MedalVisual[] medalVisualDefs;

	// Use this for initialization
	void Start () {
	
	}

}
