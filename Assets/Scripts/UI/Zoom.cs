using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
/// This class changes the scale of target object by the multiplier  
/// </summary>
public class Zoom : MonoBehaviour {
	public GameObject target;
	public float multiplier = 0.8f;
    public static int zoomLevel = 0;
    public int zoomLimit = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ZoomTarget() {
        if (zoomingAllowed()) {
            Vector3 scale = target.transform.localScale;
            scale = scale * multiplier;
            target.transform.localScale = scale;
        }        
     }

    private bool zoomingAllowed() {
        if (multiplier < 1 && (zoomLevel + zoomLimit) > 0)
        {
            zoomLevel--;
            return true;
        }
        if (multiplier > 1 && zoomLevel < zoomLimit)
        {
            zoomLevel++;
            return true;
        }
        return false;
    }
    
}
