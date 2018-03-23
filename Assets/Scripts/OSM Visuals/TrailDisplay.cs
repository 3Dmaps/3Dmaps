using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Produces a representation of a trail by positioning each node 
/// in a trail correctly relative to the map created from a MapData
/// and drawing the path with LineRenderer.
/// </summary>

public class TrailDisplay : MonoBehaviour {

	public MapData mapData;
	public Color trailColor;
    public List<Vector3> nodePositions;
    public float lineWidthMultiplier = 0.01f;
	public float lineHeightAdjustment = 0.005f;
  
    public Material material;

	public void DisplayNodes(List<DisplayNode> nodeList)
    {
        nodePositions = new List<Vector3>();
        foreach (DisplayNode node in nodeList) {
			GenerateNode (node);
		}
        if (nodePositions.Count > 1) {
            GenerateLine();
        }                
    }    


    public void GenerateNode (DisplayNode node) {
		if (!PositionService.IsWithinBounds(node.x, node.y, mapData)) {
			return;
		}
		Vector3 nodePosition = PositionService.GetUnityPosition(node, lineHeightAdjustment, mapData);

        this.nodePositions.Add(nodePosition);
    }


    public void GenerateLine()
    {
        GameObject newLine = new GameObject();
        newLine.transform.SetParent(this.transform);
        
        LineRenderer lineRenderer = newLine.AddComponent<LineRenderer>();
        lineRenderer.positionCount = this.nodePositions.Count;
        lineRenderer.SetPositions(this.nodePositions.ToArray());
        lineRenderer.widthMultiplier = lineWidthMultiplier;
        lineRenderer.useWorldSpace = false;

        Material[] materials = new Material[1];
        materials[0] = new Material(material);
        
        newLine.GetComponent<Renderer>().sharedMaterials = materials;
        newLine.GetComponent<Renderer>().sharedMaterial.color = trailColor;        
                     
    }
}

public class DisplayNode {
	public int x;
	public int y;

	public DisplayNode(int x, int y) {
		this.x = x;
		this.y = y;
	}
}