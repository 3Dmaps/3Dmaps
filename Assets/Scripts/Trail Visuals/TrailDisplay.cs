﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Produces a representation of a trail by generating GameObjects for each node 
/// in a trail and positioning them correctly relative to the map created from a MapData.
/// </summary>

public class TrailDisplay : MonoBehaviour {

	public MapData mapData;
	public GameObject nodeGameObject;    
	public Color trailColor;
    public List<Vector3> nodePositions;

	// Use this for initialization
	void Start () {
	}

	public void DisplayNodes(List<DisplayNode> nodeList) {

        nodePositions = new List<Vector3>();
        foreach (DisplayNode node in nodeList) {
			GenerateNode (node);
		}
        generateLine();

        
    }

    public void generateLine()
    {
        GameObject newLine = new GameObject();
        newLine.transform.SetParent(this.transform);
        LineRenderer lineRenderer = newLine.AddComponent<LineRenderer>();
        lineRenderer.positionCount = this.nodePositions.Count;
        lineRenderer.SetPositions(this.nodePositions.ToArray());
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.useWorldSpace = false;


        if (newLine.GetComponent<Renderer>() != null)
        {
            newLine.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
            newLine.GetComponent<Renderer>().material.color = trailColor;
        }
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