using System;
using System.Collections.Generic;
using UnityEngine;

public class OscillationUpDownScript : MonoBehaviour
{
	private float Timer;
	private float Speed = 10;
	
	void Start()
	{
		Timer = UnityEngine.Random.Range(0.0f, 9.0f);
		Speed = UnityEngine.Random.Range(6.0f, 8.0f);
	}
	
	void Update()
	{
		Timer += Time.deltaTime * Speed;
		if(Timer >= 90)
		{
			Timer = 90;
			Speed *= -1;
		}
		else if(Timer <= 0)
		{
			Timer = 0;
			Speed *= -1;
		}
		
		this.transform.localPosition = this.transform.localPosition + new Vector3(0, Mathf.Sin(Timer) * 0.004f, 0);
	}
}


public class QuickScaleOscillationScript : MonoBehaviour
{
	private float Timer;
	private float Speed = 1;
	private float SavedScaleY;
	
	void Start()
	{
		SavedScaleY = transform.localScale.y;
	}
	
	void Update()
	{
		Timer += Time.deltaTime * Speed * 7;
		Debug.Log("TIMER: " + Timer.ToString());
		if(Timer >= 1)
		{
			Timer = 1;
			Speed *= -1;
		}
		else if(Timer <= 0)
		{
			Timer = 0;
			Speed *= -1;
			Destroy(this);
		}

		Vector3 scale = this.transform.localScale;
		scale.y = Mathf.Lerp(SavedScaleY, 5.3f, Timer);
		
		this.transform.localScale = scale;
	}
}

public class EnemyRespawnScript : MonoBehaviour
{
	private float Timer;

	void Start()
	{
		Timer = 3;
	}
	
	void Update()
	{
		//this.transform.localPosition = this.transform.localPosition - new Vector3(0, Time.deltaTime * Speed, 0);
		Timer -= Time.deltaTime;
		if(Timer <= 0)
		{
			this.GetComponent<BoxCollider>().enabled = true;

			for(int i=0;i<this.transform.childCount;++i)
			{
				if(this.transform.GetChild(i).name == "Sphere")
					this.transform.GetChild(i).gameObject.SetActive(false);
				else
					this.transform.GetChild(i).gameObject.SetActive(true);
			}

			EvilCharacterPatternMovementScript movementScript = gameObject.GetComponent<EvilCharacterPatternMovementScript>();
			if(movementScript != null) 
				movementScript.enabled = true;

			this.gameObject.AddComponent<QuickScaleOscillationScript>();

			Destroy(this);
		}
	}
}

public class EnemyDeathAnimationScript : MonoBehaviour
{
	private float Timer;
	private float Speed;
	
	void Start()
	{
		Timer = 3;
		Speed = UnityEngine.Random.Range(0.3f, 0.4f);
		//this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * 0.07f, this.transform.localScale.z);
	
		for(int i=0;i<this.transform.childCount;++i)
		{
			if(this.transform.GetChild(i).name == "Sphere")
				this.transform.GetChild(i).gameObject.SetActive(true);
			else
				this.transform.GetChild(i).gameObject.SetActive(false);
		}
		//this.GetComponent<Animator>().SetBool("None", true );
		this.GetComponent<BoxCollider>().enabled = false;
	}
	
	void Update()
	{
		//this.transform.localPosition = this.transform.localPosition - new Vector3(0, Time.deltaTime * Speed, 0);

		Timer -= Time.deltaTime;
		if(Timer <= 0)
		{
			this.gameObject.AddComponent<EnemyRespawnScript>();
			Destroy(this);
			//this.gameObject.SetActive(false);
		}
	}
}


public class FlashMaterialColorScript : MonoBehaviour
{
	private float Timer;
	private float Speed;
	private int TimesFlashed;
	
	void Start()
	{
		Timer = 90;
		Speed = UnityEngine.Random.Range(3.0f, 4.0f);
	}
	
	void Update()
	{
		Timer += Time.deltaTime * Speed;
		if(Timer >= 1)
		{
			Timer = 1;
			Speed *= -1;
			TimesFlashed++;
		}
		else if(Timer <= 0)
		{
			Timer = 0;
			Speed *= -1;
		}

		float alpha = Mathf.Lerp(0.6f, 1.0f, Timer);

		for(int i=0;i<transform.childCount;++i)
		{
			transform.GetChild(i).GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
		}

		if(TimesFlashed == 5)
		{
			Destroy(this);
		}
		
		//this.transform.localPosition = this.transform.localPosition + new Vector3(0, Mathf.Lerp(0.4f, 1.0f, Timer / 90.0f), 0);
	}
}
