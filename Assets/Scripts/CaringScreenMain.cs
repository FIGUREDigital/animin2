using UnityEngine;
using System.Collections;

public class CaringScreenMain : MonoBehaviour {

		[SerializeField]
		private Transform m_NonARCameraPositionReference;

		public Transform NonARCameraPositionReference {
				get {
						return m_NonARCameraPositionReference;
				}
		}

}
