using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Object storing data used to display areas and rivers by the AreaDisplay class.
/// </summary>

public class DisplayPoly {

 	public List<DisplayNode> displayNodes = new List<DisplayNode>();
	public List<int> boundingBox = new List<int>();
	public Color color;
	public PolyType type;


	public DisplayPoly(List<DisplayNode> displayNodes) {
		this.displayNodes = displayNodes;
	}
 }

public enum PolyType {Area, River}