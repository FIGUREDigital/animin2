using UnityEngine;
using System.Collections;

public class SetPrerenderARCamera : MonoBehaviour {

    void OnPreRender()
    {
        // make sure the correct backface culling is set for this camera
        GL.SetRevertBackfacing(false);
    }
}
