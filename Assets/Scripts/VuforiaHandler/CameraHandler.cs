using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour, ITrackableEventHandler
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void OnTrackableStateChanged(
				TrackableBehaviour.Status previousStatus,
				TrackableBehaviour.Status newStatus)
		{
		}

		private void OnTrackingFound()
		{
		}

		private void OnTrackingLost()
		{
		}
}
