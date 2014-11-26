using UnityEngine;
using System.Collections.Generic;

public class TestScript : MonoBehaviour {


	private bool IsDetectingSwipeRight;
	private int SwipesDetectedCount;
	private List<Vector3> SwipeHistoryPositions = new List<Vector3>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetMouseButton(0))
		{
			Debug.Log("DETECT SWIPE MECHANISM");
			
			if(SwipeHistoryPositions.Count == 0)
			{
				SwipeHistoryPositions.Add(Input.mousePosition);
				Debug.Log("11111111111");
			}
			else
			{
				Debug.Log("22222222222");
				if(IsDetectingSwipeRight)
				{
					if((Input.mousePosition.x - SwipeHistoryPositions[SwipeHistoryPositions.Count - 1].x) >= 15)
					{
						SwipeHistoryPositions.Add(Input.mousePosition);
						IsDetectingSwipeRight = !IsDetectingSwipeRight;
						SwipesDetectedCount++;
						Debug.Log("swipe moving right: " + SwipesDetectedCount.ToString());
					}
				}
				else
				{
					if((SwipeHistoryPositions[SwipeHistoryPositions.Count - 1].x - Input.mousePosition.x) >= 15)
					{
						SwipeHistoryPositions.Add(Input.mousePosition);
						IsDetectingSwipeRight = !IsDetectingSwipeRight;
						SwipesDetectedCount++;
						Debug.Log("swipe moving left: " + SwipesDetectedCount.ToString());
					}
				}
				
				if(SwipesDetectedCount >= 4)
				{
					Debug.Log("SWIPE DETECTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					IsDetectingSwipeRight = !IsDetectingSwipeRight;
					SwipesDetectedCount = 0;
				}
			}
			
			
			
			/*
			List<GameObject> TouchedUnttouchedSwipeList = new List<GameObject>();
			// Consider this object for swiped
			if(hadRayCollision && (hitInfo.collider.tag == "Items" || hitInfo.collider.tag == "Shit"))
			{
				if(!TouchedUnttouchedSwipeList.Contains(hitInfo.collider.gameObject))
				{
					TouchedUnttouchedSwipeList.Remove(hitInfo.collider.gameObject);
				}
				else
				{
					if(!TouchedUnttouchedSwipeList.Contains(hitInfo.collider.gameObject))
						TouchedUnttouchedSwipeList.Add(hitInfo.collider.gameObject);
				}
			}
			else
			{
				for(int i=0;i<TouchedUnttouchedSwipeList.Count;++i)
				{
					SwipeHitsPerObject[TouchedUnttouchedSwipeList[i]]++;
				}
				
				TouchedUnttouchedSwipeList.Clear();
			}*/
			
			
			
		}
		else
		{
			Debug.Log("NO MOUSE DOWN");
		}
	
	}
}
