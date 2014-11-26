using UnityEngine;
using System.Collections;

public class HeadReferenceScript : MonoBehaviour {

	public Vector3 HeadRotationOffset;
	public GameObject HeadBoneToRotate;
	public GameObject Indicator;
	public GameObject ObjectCarryAttachmentBone;
	public ModelAnimationLayerId PickupAnimationLayer;

	[SerializeField]
	private GameObject m_LeftHand;
	public GameObject LeftHand { get { return m_LeftHand; } }
	[SerializeField]
	private GameObject m_RightHand;
	public GameObject RightHand { get { return m_RightHand; } }

	private GameObject m_HoldingObject;
	public GameObject HoldingObject {
		get { return m_HoldingObject;}
		set { m_HoldingObject = value;}
	}

	
	[SerializeField]
	private Vector2 m_ManualOffset = new Vector2(7f,10f);


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (HoldingObject != null) {
			Transform t = HoldingObject.transform;

			Vector3 avg = (LeftHand.transform.position + RightHand.transform.position)/2.0f;
			//Vector3 handsup = (LeftHand.transform.up + RightHand.transform.up)/2.0f;
			//Debug.Log ("Left.up : ["+LeftHand.transform.up+"]; Right.up["+RightHand.transform.up+"]; Avg.up["+handsup+"];");

			Vector3 cross = Vector3.Cross((RightHand.transform.position - LeftHand.transform.position),Vector3.up);
			Vector3 objpos = avg + (cross.normalized * m_ManualOffset.x);
			
			Debug.DrawLine( avg, objpos, Color.red);


			t.rotation = Quaternion.LookRotation( (objpos-avg), Vector3.up);


			MeshFilter filter = HoldingObject.GetComponent<MeshFilter>();
			if (filter != null){
				Vector3 offset = filter.sharedMesh.bounds.extents;

				Vector3 debugtmp = objpos;

				objpos -= new Vector3 (0,offset.y,0);


				//Debug.Log ("Before : ["+debugtmp+"]; After : ["+objpos+"]; Offset : ["+offset+"];");
				Debug.DrawLine(debugtmp, objpos, Color.blue);

			}
			
			t.position = objpos;
			if (t.localPosition.y < 0){
				t.localPosition = new Vector3 (t.localPosition.x,0,t.localPosition.z);
			}
		}
	}
}
