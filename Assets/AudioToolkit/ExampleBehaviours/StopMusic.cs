using UnityEngine;
using System.Collections;

public class StopMusic : MonoBehaviour
{
    public float fadeDuration = 0.5f;
	void Start()
	{
        AudioController.StopMusic(fadeDuration);
	}
}
