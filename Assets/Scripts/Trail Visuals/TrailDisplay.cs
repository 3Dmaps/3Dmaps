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
  

	public void DisplayNodes(List<DisplayNode> nodeList)
    {
        nodePositions = new List<Vector3>();
        foreach (DisplayNode node in nodeList) {
			GenerateNode (node);
		}
        GenerateLine();        
    }    


    public void GenerateNode (DisplayNode node) {
		int rawX = node.x + ((mapData.GetWidth() - 1) / 2);
		int rawY = mapData.GetHeight() - 1 - (node.y + ((mapData.GetHeight() - 1) / 2));

		if (!IsWithinBounds(rawX, rawY)) {
			return;
		}
			
		float height = mapData.GetNormalized (rawX, rawY);
		Vector3 nodePosition = new Vector3 (((float) node.x * mapData.GetScale()), height, (float) node.y * mapData.GetScale());
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
        materials[0] = new Material(Shader.Find("Unlit/Color"));
        
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