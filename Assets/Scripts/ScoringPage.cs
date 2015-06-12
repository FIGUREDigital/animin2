using UnityEngine;
using System.Collections;
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
			Show (Minigame.Cuberunners, 20000, 4, null);
		}
		#endif
	}


	public MedalVisual[] medalVisualDefs;

	public static void Show (Minigame game, int points, int stars, Action onFinish)
	{
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
		showingHighScore = score;
		highScore.Text = string.Format("High Score: {0:0}", score);
	}

	float showingHighScore = 0;
	int showMedal = 0;
	int curMedal = 0;
	float medalAt = 0;
	float barMax = float.MaxValue;
	float barMin = 0;
	MedalScores ms = null;

	IEnumerator DoShow (Minigame game, int points, int stars, Action onFinish)
	{
		SetHighScore (18000);
		visuals.SetActive (false);
		visuals.transform.localScale = new Vector3 (0.001f, 0.001f, 0.001f);
		yield return new WaitForSeconds (0.25f);
		visuals.SetActive (true);
		SetScore (0);
		starCount.Text = "x<size=+60>" + stars + "</size>";
		glow.gameObject.SetActive (false);
		visuals.SetActive (true);
		visuals.transform.DOScale (Vector3.one, 0.5f).SetEase (Ease.OutBounce);
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
				glow.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InCubic);
				yield return new WaitForSeconds(0.55f);
				// And show
				ShowMedal(0.5f);
			}
			i++;
		} while (ms.scores.Length >= i);
		if(onFinish != null)
		{
			onFinish();
		}
	}

	public void OnOk()
	{

	}
}
