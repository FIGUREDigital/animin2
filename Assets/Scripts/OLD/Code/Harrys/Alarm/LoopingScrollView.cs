using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoopingScrollView : MonoBehaviour {
	/*
	public float swap_dist = 0.5f;

	public GameObject Collider;

	private ScrollRect m_Scroll;
	private GridLayoutGroupd m_Grid;
	private Text[] m_ChildLabels;

	private Vector3 m_GridInitPos;

	private int m_CurSecltected;
	public int CurrentlySelected{
		get{
			return m_CurSecltected;
		}
	}

	// Use this for initialization
	void Start () {
		m_Scroll = this.GetComponent<UIScrollView> ();
		m_Grid = GetComponentInChildren<UIGrid> ();

		m_GridInitPos = m_Grid.transform.position;
	}

	
	// Update is called once per frame
	void Update () {

		float min_y_dist = Collider.transform.position.y - m_Grid.transform.GetChild (0).position.y;
		m_CurSecltected = 0;

		float max_y = Collider.transform.position.y - m_Grid.transform.GetChild (0).position.y;
		int max_y_i = 0;

		float min_y = Collider.transform.position.y - m_Grid.transform.GetChild (0).position.y;
		int min_y_i = 0;

		//FOR LOOP--------------------------------------
		for (int i = 0; i < m_Grid.GetComponentsInChildren<Text>().Length; i++) {

			float y_dist = Collider.transform.position.y - m_Grid.transform.GetChild (i).position.y;

			if (Mathf.Abs(y_dist) < Mathf.Abs(min_y_dist)) {
				min_y_dist = y_dist;
				m_CurSecltected = i;
			}
			//Debug.Log ("A: Child : [" + i + ":"+ m_Children[i].text+"]; Position : [" + (m_Grid.transform.localPosition + m_Grid.GetChild (i).transform.localPosition+) + "]");

			if (y_dist > max_y) {
					max_y = y_dist;
					max_y_i = i;
			}
			if (y_dist < min_y) {
					min_y = y_dist;
					min_y_i = i;
			}
		}
		//END FOR LOOP----------------------------------



		if (!m_Scroll.isDragging) {
			//m_Scroll.Scroll (-min_y_dist*2);
			m_Scroll.MoveRelative(new Vector3(0,min_y_dist*100,0));
		}
		
		
		float cellH = m_Grid.cellHeight;
		Vector3 cellDim = new Vector3 (0, cellH, 0);

		if (max_y > swap_dist && m_Scroll.currentMomentum.y<0) {
			//Moving from top of list to bottom
			m_Grid.transform.GetChild (max_y_i).transform.localPosition = m_Grid.transform.GetChild (min_y_i).transform.localPosition + cellDim;
		}
		if (min_y < -swap_dist && m_Scroll.currentMomentum.y>0) {
			//Moving from bottom of list to top
			m_Grid.transform.GetChild (min_y_i).transform.localPosition = m_Grid.transform.GetChild (max_y_i).transform.localPosition - cellDim;
		}
	}
*/
}
