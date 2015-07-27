using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class ScoringPage : Phi.SingletonScene<ScoringPage> 
{
	public GameObject visuals;
	public GameObject okButton;
	public Image barFill;
	public UIImagePro glow;
	public UIImagePro medal;
	public UIText starCount;
	public UIText score;
	public UIText highScore;
	public float duration = 4;
	public MedalScores[] medalScores;
	public MeshRenderer chestLid;
	public MeshRenderer chestBase;
	public GameObject prize;
	public Transform chest;
	public Transform win;
	
	[System.Serializable]
	public class MedalScores
	{
		public Minigame game;
		public int[] scores;
	}

	[System.Serializable]
	public class MedalVisual
	{
		public Color glow;
		public Color bar;
		public Sprite sprite;
		public ParticleSystem particles;
		public Material chestLidMaterial;
		public Material chestBaseMaterial;
		public ParticleSystem chestParticles;
	}

	override public void Init()
	{
		visuals.SetActive(false);
		#if UNITY_EDITOR
		string scene = UnityEditor.EditorApplication.currentScene;
		if (scene.EndsWith ("ScorePage.unity")) 
		{
			// Test the screen
			Camera cam = Phi.SingletonPrefab.instances["UICamera"].GetComponent<Camera>();
			cam.clearFlags = CameraClearFlags.Color;
			cam.backgroundColor = Color.black;
			Show (Minigame.Cuberunners, 800, 4, null);
		}
		#endif
	}


	public MedalVisual[] medalVisualDefs;

	public static void Show (Minigame game, int points, int stars, Action onFinish)
	{
		
		#if UNITYANALYTICS
		UnityEngine.Analytics.Analytics.CustomEvent ("Score", new Dictionary<string, object>{{"game",game.ToString ()},{"points", points},{"stars", stars}});
#endif

		if (Exists ()) 
		{
			Instance.StartCoroutine(Instance.DoShow (game, points, stars, onFinish));
		}
		else 
		{
			if(onFinish != null)
			{
				onFinish();
			}
		}
	}



	float scoreValue = 0;
	void SetScore(float v)
	{
		scoreValue = v;
		score.Text = string.Format("Score: <size=+45>{0:0}</size>", v);
		if (v > medalAt)
		{
			if (curMedal == 0)
			{
				ShowMedal(0.5f);			
			}

		}			
		barFill.fillAmount = (v - barMin) / (barMax - barMin);
		if (v > showingHighScore) 
		{
			SetHighScore(v);
		}
	}

	void ShowMedal(float duration)
	{
		// Show medal
		glow.gameObject.SetActive(true);
		glow.color = medalVisualDefs[curMedal].glow;
		medal.sprite = medalVisualDefs[curMedal].sprite;
		barFill.color = medalVisualDefs[curMedal].bar;
		glow.transform.DOScale (Vector3.one, duration).SetEase (Ease.OutBounce);
		StartCoroutine(ParticleBurst(curMedal));
		BetweenSceneData.Instance.chest = curMedal+1;
		// Setup next medal			
		curMedal++;
		SetupMedal(curMedal);
	}

	IEnumerator ParticleBurst(int medal)
	{
		yield return new WaitForSeconds (0.25f);
		medalVisualDefs [medal].particles.Emit (80);
	}

	void SetupMedal(int medal)
	{
		medalAt = float.MaxValue;
		if(ms != null && ms.scores.Length > medal)
		{
			medalAt = ms.scores[medal];
		}
	}

	void SetHighScore(float score)
	{
		float curHigh = 0;
		while (ProfilesManagementScript.Instance.CurrentProfile.highScores.Count <= (int)currentGame) 
		{
			ProfilesManagementScript.Instance.CurrentProfile.highScores.Add (0);
		}
		curHigh = ProfilesManagementScript.Instance.CurrentProfile.highScores[(int)currentGame];
		if (score < curHigh) {
			score = curHigh;
		} else {
			ProfilesManagementScript.Instance.CurrentProfile.highScores[(int)currentGame] = Mathf.RoundToInt(score);
		}
		showingHighScore = score;
		highScore.Text = string.Format("High Score: {0:0}", score);
	}

	float showingHighScore = 0;
	int curMedal = 0;
	float medalAt = 0;
	float barMax = float.MaxValue;
	float barMin = 0;
	MedalScores ms = null;
	Action onFinish;
	Minigame currentGame;

	IEnumerator DoShow (Minigame game, int points, int stars, Action onFinish)
	{
		currentGame = game;
		win.localScale = new Vector3 (0.001f, 0.001f, 0.001f);
		win.gameObject.SetActive (false);
		chest.localScale = Vector3.zero;
		this.onFinish = onFinish;
		visuals.SetActive (false);
		prize.SetActive (false);		
		yield return new WaitForSeconds (0.25f);
		SetScore (0);
		starCount.Text = "x<size=+60>" + stars + "</size>";
		glow.gameObject.SetActive (false);
		visuals.SetActive (true);
		SetHighScore (0);
#if SCALEON
		visuals.transform.localScale = new Vector3 (0.001f, 0.001f, 0.001f);
		visuals.transform.DOScale (Vector3.one, 0.5f).SetEase (Ease.OutBounce);
		#else
		visuals.transform.localPosition = new Vector3 (0, -1800, 0);
		visuals.transform.DOLocalMove (Vector3.zero, 0.5f).SetEase (Ease.OutBounce);
#endif
		yield return new WaitForSeconds (0.5f);
	
		ms = null;
		int i;
		for (i = 0; i < medalScores.Length; i++) 
		{
			if (medalScores[i].game == game)
			{
				ms = medalScores[i];
			}
		}

		barMax = 0;
		curMedal = 0;
		SetupMedal (0);
		
		barFill.color = medalVisualDefs[0].bar;
		glow.transform.localScale = Vector3.zero;
		i = 1;
		do {
			barMin = barMax;
			// Work out barMax;
			if (medalScores != null && ms.scores.Length > i) {
				barMax = ms.scores[i];
			} else {
				barMax = Mathf.Max (points, showingHighScore);
			}
			barFill.fillAmount = 0;
			if (barMax - barMin < 1000) {
				barMax = barMin + 1000;
			}
			DOTween.To (SetScore, scoreValue, Mathf.Min (barMax, points), 2.0f);
			yield return new WaitForSeconds(2.0f);
			if (points > barMax)
			{
				// Get ready for next bar.. hide medal		
				//glow.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic);
				//yield return new WaitForSeconds(0.55f);
				glow.transform.localScale = Vector3.zero;
				// And show
				ShowMedal(0.5f);
			}
			i++;
		} while (points > barMax && ms.scores.Length >= i);
		if (curMedal > 0) {
			switch (curMedal)
			{
			case 1:
				AudioController.Play ("MedalBronze");
				break;
			case 2:
				AudioController.Play ("MedalSilver");
				break;
			case 3:
				AudioController.Play ("MedalGold");
				break;
			}
			MedalVisual visual = medalVisualDefs [curMedal - 1];
			if (visual != null) {
				// Now show prize
				chestLid.sharedMaterial = visual.chestLidMaterial;
				chestBase.sharedMaterial = visual.chestBaseMaterial;
				prize.SetActive (true);				
				win.gameObject.SetActive (true);
				yield return new WaitForSeconds (0.25f);
				visual.chestParticles.Emit (50);
			}
		}
	}

	public void OnOk()
	{		
		visuals.transform.DOLocalMove (new Vector3 (0, -1800, 0), 0.25f).SetEase (Ease.InCubic).OnComplete (Hide);
	}
	public void Hide()
	{		
		visuals.SetActive (false);
		if(onFinish != null)
		{
			onFinish();
		}
	}
}
