using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Produces a representation of a trail by positioning each node 
/// in a trail correctly relative to the map created from a MapData
/// and drawing the path with LineRenderer. Also creates a label for
/// each trail. The UpdateLabelPosition method updates the position 
/// and visibility of labels during runtime based on camera position. 
/// </summary>

public class TrailDisplay : MonoBehaviour {

	public MapData mapData;
	public Color trailColor;
    public List<Vector3> nodePositions;
    public float lineWidthMultiplier = 0.01f;
	public float lineHeightAdjustment = 0.005f;
    public GameObject labelGameObject;
    public string trailName;
    
    public Material material;

	public List<TrailLabel> labels = new List<TrailLabel>();

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
        
        if (!trailName.Equals("")) {
            GenerateLabelGameObject(lineRenderer.GetPosition(1));        
        }         
    }
    public void GenerateLabelGameObject(Vector3 nodePosition) {
        
		GameObject newLabelObject = Instantiate(labelGameObject);
        newLabelObject.name = trailName;
        newLabelObject.transform.SetParent(this.transform);
		TextMesh text = newLabelObject.GetComponent<TextMesh>();
        text.text = trailName;
        newLabelObject.transform.position = nodePosition;
		newLabelObject.GetComponent<MeshRenderer> ().enabled = false;

		labels.Add(new TrailLabel(nodePositions, newLabelObject, trailName));
    } 
		
	public void UpdateLabelPositions() {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

		List<Bounds> activeBounds = new List<Bounds>();
		List<string> usedNames = new List<string>();

		foreach (TrailLabel label in labels) {
			label.labelObject.GetComponent<MeshRenderer> ().enabled = false;
			if (usedNames.Contains (label.name)) {
				continue;
			}
			for (int posNum = 0; posNum < label.unityPositions.Count; posNum += 10) {
				Vector3 position = label.unityPositions [posNum];

				Bounds bounds = label.labelObject.GetComponent<MeshRenderer> ().bounds;
				Bounds minBounds = new Bounds ();
				Bounds maxBounds = new Bounds ();
				minBounds.center = bounds.min - new Vector3(0.025f, 0.025f, 0.025f);
				maxBounds.center = bounds.max + new Vector3(0.025f, 0.025f, 0.025f);;

				if (GeometryUtility.TestPlanesAABB (planes, minBounds)
					&& GeometryUtility.TestPlanesAABB (planes, maxBounds)
					&& NoNearbyBounds(activeBounds, bounds)) {

					usedNames.Add (label.name);
					activeBounds.Add (bounds);
					label.labelObject.GetComponent<MeshRenderer> ().enabled = true;
					break;
				}
				label.labelObject.transform.localPosition = position;
			}					
		}
	}

	public bool NoNearbyBounds(List<Bounds> currentBounds, Bounds bounds) {
		foreach (Bounds oldBounds in currentBounds) {
			if (oldBounds.SqrDistance(bounds.center) < 0.002f) {
				return false;
			}
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