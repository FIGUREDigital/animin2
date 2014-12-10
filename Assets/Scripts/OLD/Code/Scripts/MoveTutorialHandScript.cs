using UnityEngine;
using System.Collections;

public class MoveTutorialHandScript : MonoBehaviour {

	float Lerp = 0;
	float Cooldown;
	int SwipesPerformed;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Cooldown > 0)
		{
			Cooldown -= Time.deltaTime;
			//this.GetComponent<Image>().enabled = false;
			Lerp = 1;
			if(Cooldown <= 0)
			{
				Cooldown = 0;
			}
		}
		else if(Cooldown == 0)
		{
			Cooldown = -1;
			Lerp = 0;
			if(SwipesPerformed >= 2)
				this.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			Lerp += Time.deltaTime * 0.6f;
			if(Lerp >= 1)
			{
				Lerp = 1;
				Cooldown = 0.6f;
				SwipesPerformed++;
			}
			//this.GetComponent<Image>().enabled = true;

            this.transform.localPosition = new Vector3(Mathf.Lerp(-121.8f, 120.6f, Lerp), -28f, 0);

		
		}
	}
}
