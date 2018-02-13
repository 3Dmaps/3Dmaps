using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
/// This class changes the main camera's field of vision.  
/// </summary>

public class Zoom : MonoBehaviour
{    
    public float zoomInmultiplier = 0.8f;
    public float zoomOutmultiplier = 1.25f;
    private int currentZoomLevel = 0;
    public int zoomLimitMax = 4;
    public int zoomLimitMin = -4;   


    public void CameraZoom(int zoomValue)
    {
        
        if (currentZoomLevel + zoomValue <= zoomLimitMax && currentZoomLevel + zoomValue >= zoomLimitMin)
        {            
            float multiplier = zoomValue < 0 ? zoomOutmultiplier : zoomInmultiplier;            
            Camera.main.fieldOfView = Camera.main.fieldOfView * multiplier;             
            currentZoomLevel += zoomValue;
        }
    }
}
