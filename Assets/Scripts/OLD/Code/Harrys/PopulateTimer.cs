using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopulateTimer : MonoBehaviour {

	public int num;
	// Use this for initialization
	void Start () {

		GridLayoutGroup Grid = GetComponentInChildren<GridLayoutGroup> ();
		Text[] ChildLabels= Grid.GetComponentsInChildren<Text> ();

		GameObject template = ChildLabels [ChildLabels.Length - 1].gameObject;
		


		for (int i = ChildLabels.Length ; i < num ;i++) {


			GameObject go = GameObject.Instantiate(template) as GameObject;
			go.transform.parent = Grid.transform;
			go.transform.localPosition = template.transform.localPosition - new Vector3(0,Grid.minHeight,0);
			go.transform.localRotation = template.transform.localRotation;
			go.transform.localScale = template.transform.localScale;

			int id = int.Parse(template.name) + 1;
			string idName = id < 10 ? "0"+id.ToString():id.ToString();

			go.name = idName;
			go.GetComponent<Text>().text = idName;
			
			//Debug.Log ("Newlabel : ["+go.name+"]; Pos : ["+go.transform.localPosition+"];");

			template = go;
		}
	}
}