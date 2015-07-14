using UnityEngine;
using System.Collections;

public class EDMCheck : MonoBehaviour
{

    enum InstrumentType
    {
        EDM,
        Juno,
        Piano}

    ;

    [SerializeField]
    private InstrumentType m_Instrument;

    [SerializeField]
    private int m_ID;

    private ToggleableButtonScript m_ToggleScript;

    void Start()
    {
        m_ToggleScript = GetComponent<ToggleableButtonScript>();
    }

    void OnEnable()
    {
        if (m_ToggleScript == null)
            m_ToggleScript = GetComponent<ToggleableButtonScript>();
        if (m_ToggleScript != null)
        {
            int checkID = 0;
            if (m_Instrument == InstrumentType.Juno)
                checkID = 16;
            else if (m_Instrument == InstrumentType.EDM)
                checkID = 8;
            else if (m_Instrument == InstrumentType.Piano)
                checkID = 0;
            checkID += m_ID;
            if (EDMMixerScript.Singleton.KeysOn[checkID])
            {
                m_ToggleScript.SetOn();
//                Debug.Log("Turning On");
            }
            else
            {
                m_ToggleScript.SetOff();
//                Debug.Log("Turning Off");
            }
        }
        else
        {
            Debug.Log("Something has gone wrong");
        }
    }
}
