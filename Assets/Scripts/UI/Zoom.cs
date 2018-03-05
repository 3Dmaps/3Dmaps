using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
/// This class changes the scale of target object by the multiplier
/// If the target object has a MapGenerator attached, this will notify it of the zoom change.
/// </summary>

public class Zoom : MonoBehaviour
{
    public GameObject target;
    private MapGenerator generator;
    public float zoomInmultiplier = 0.8f;
    public float zoomOutmultiplier = 1.25f;
    private int currentZoomLevel = 0;
    public int zoomLimitMax = 5;
    public int zoomLimitMin = -5;

    public void Start() {
        generator = target.GetComponent<MapGenerator>();
    }

    public void ZoomTarget(int zoomValue)
    {
        if (currentZoomLevel + zoomValue <= zoomLimitMax && currentZoomLevel + zoomValue >= zoomLimitMin)
        {
            float multiplier = zoomValue < 0 ? zoomInmultiplier : zoomOutmultiplier;
            Vector3 scale = target.transform.localScale;
            scale = scale * multiplier;
            target.transform.localScale = scale;
            currentZoomLevel += zoomValue;
            if(generator != null) generator.UpdateZoomLevel(currentZoomLevel);
        }
    }
}
