using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 
///  
/// </summary>

public class DisplayPoly {

	private List<DisplayNode> boundings = new List<DisplayNode>();
	private List<Color> colors = new List<Color> ();
    private int maxX, minX, maxY, minY = 0;

    public void DisplayNodeBoundingBox(List<DisplayNode> dpoints)   {

    minX = dpoints.Max(p=> p.x);
    minY = dpoints.Min(p => p.y);
    maxX = dpoints.Max(p => p.x);
    maxY = dpoints.Max(p => p.y);

}


}