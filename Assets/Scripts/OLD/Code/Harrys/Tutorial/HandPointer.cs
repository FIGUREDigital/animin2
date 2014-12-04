using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandPointer : MonoBehaviour{


    private RectTransform m_Child;

    void Start(){
        m_Child = GetComponent<RectTransform>();
    }
    void Update(){



        float val = 1f+(0.25f * Mathf.Sin(5f * Time.timeSinceLevelLoad));
        m_Child.localScale = new Vector3(val, val, 0);
    }
}
