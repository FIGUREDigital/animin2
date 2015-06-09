using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinigamesPageControls : MonoBehaviour 
{
	public UnityEngine.UI.Image miniGame1;
	public UnityEngine.UI.Image miniGame2;
	public UnityEngine.UI.Image miniGame3;
	public UnityEngine.UI.Image miniGame4;
		
	public void Minigame1Button()
	{
		if (ProfilesManagementScript.Instance.CurrentAnimin.CrystalCount >= 1)
		{
			MainARHandler.Instance.SetDefaultZoom();
        	MainARHandler.Instance.ChangeSceneToCubeRunner();
			ZoomBehaviour.canZoom = false;
		}
	}
	public void Minigame2Button()
    {
		if (ProfilesManagementScript.Instance.CurrentAnimin.CrystalCount >= 2)
		{
			GunsMinigameScript.destroyedEnemyCount = 0;
			GunsMinigameScript.waveCount = 1;
			GunsMinigameScript.enemyCount = 0;
        	MainARHandler.Instance.ChangeSceneToCannon();
			ZoomBehaviour.canZoom = true;
		}
	}

	void Update ()
	{
		Color c = Color.white;
		c.a = ProfilesManagementScript.Instance.CurrentAnimin.CrystalCount >= 1 ? 1 : 0.5f;
		miniGame1.color = c;
		
		c.a = ProfilesManagementScript.Instance.CurrentAnimin.CrystalCount >= 2 ? 1 : 0.5f;
		miniGame2.color = c;

		c.a = 0.5f;
		
		miniGame3.color = c;
		
		miniGame4.color = c;
	}
}
