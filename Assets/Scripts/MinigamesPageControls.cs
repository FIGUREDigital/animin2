using UnityEngine;
using System.Collections;

public class MinigamesPageControls : MonoBehaviour {

	public void Minigame1Button()
	{
        MainARHandler.Instance.ChangeSceneToCubeRunner();
	}
	public void Minigame2Button()
    {
        MainARHandler.Instance.ChangeSceneToCannon();
	}
}
