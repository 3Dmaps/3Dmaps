using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
/// This class changes the scale of target object by the multiplier  
/// </summary>
public class Zoom : MonoBehaviour {
	public GameObject target;
	public float multiplier = 0.8f;
    private static int zoomLevel = 0;
    public int zoomLimit = 5;

    public static int ZoomLevel
    {
        get
        {
            return zoomLevel;
        }

        set
        {
            zoomLevel = value;
        }
    }


	public void ZoomTarget() {
        if (zoomingAllowed()) {
            Vector3 scale = target.transform.localScale;
            scale = scale * multiplier;
            target.transform.localScale = scale;
        }        
     }

    private bool zoomingAllowed() {
        if (multiplier < 1 && (ZoomLevel + zoomLimit) > 0)
        {
            ZoomLevel--;
            return true;
        }
        if (multiplier > 1 && ZoomLevel < zoomLimit)
        {
            ZoomLevel++;
            return true;
        }
        return false;
    }
    
}
