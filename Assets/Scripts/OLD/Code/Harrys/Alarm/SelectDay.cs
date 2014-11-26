using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectDay : MonoBehaviour {

	private int my_i;
	private bool m_clicked;

	// Use this for initialization
	void Start () {
		GridLayoutGroup grid = GetComponentInParent<GridLayoutGroup> ();
		if (grid == null)
						Debug.Log ("I didn't find Grid");
		for (int i=0; i <grid.GetComponentsInChildren<Text>().Length; i++) {
			//Debug.Log("Comparing : ["+this+":"+grid.GetComponentsInChildren<Text>()[i].gameObject+"];");
			if (this.gameObject == grid.GetComponentsInChildren<Text>()[i].gameObject){
				//Debug.Log("Found!");
				my_i = i;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnClick(){
		//Debug.Log ("Clicked: : ["+this.name+"]; i : ["+my_i+"];");


		Color col = (m_clicked?Color.white:Color.red);
		//GetComponent<Button> ().colors.normalColor = col;
		m_clicked = !m_clicked;
	}
}
