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
	public GameObject nodeGameObject;    
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
		if (!IsWithinBounds(node.x, node.y)) {
			return;
		}

		float height = mapData.GetNormalized (node.x, node.y);

		float xFromCenter = node.x - mapData.GetWidth() / 2;
		float yFromCenter = (mapData.GetHeight() / 2) - node.y;

		Vector3 nodePosition = new Vector3 (((float) xFromCenter * mapData.GetScale()), height + lineHeightAdjustment, (float) yFromCenter * mapData.GetScale());
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


    // keeping this for illustration purposes and future needs
    public void GenerateNodeGameObject(Vector3 nodePosition) {
        GameObject newNode = Instantiate(nodeGameObject);
        if (newNode.GetComponent<Renderer>() != null)
        {
            newNode.GetComponent<Renderer>().material.color = trailColor;
        }

        newNode.transform.position = nodePosition;
        newNode.transform.SetParent(this.transform);
    }    


	public bool IsWithinBounds(int rawX, int rawY) {
		if (rawX < 0 || rawX > mapData.GetWidth() - 1) {
			return false;		
		}

		if (rawY < 0 || rawY > mapData.GetHeight() - 1) {
			return false;		
		}

		return true;
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