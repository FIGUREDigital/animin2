using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Gradient))]
public class PulseGradient : MonoBehaviour {
	public float pulseSpeed;
	Gradient grad;
	public Color color1;
	public Color color2;
	Color startcol1;
	Color startcol2;
	float ratio;
	Graphic graphic;

	// Use this for initialization
	void Awake() 
	{
		grad = GetComponent<Gradient> ();
		graphic = GetComponent<Graphic> ();
		startcol1 = grad.vertex1;
		startcol2 = grad.vertex2;
		ratio = 0;	
	}

	void OnDisable()
	{
		SetColor(0);
	}

	void SetColor(float ratio)
	{
		grad.vertex1 = Color.Lerp (startcol1, color1, ratio);
		grad.vertex2 = Color.Lerp (startcol2, color2, ratio);
	}

	// Update is called once per frame
	void Update () 
	{
		ratio = (1.0f + Mathf.Sin (Time.realtimeSinceStartup * pulseSpeed)) * 0.5f;
		SetColor(ratio);
		graphic.SetVerticesDirty ();
	}
}
