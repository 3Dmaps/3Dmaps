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

	private const float labelEdgeLimit = 0.025f;
	private const float labelMinDistance = 0.0010f;

	public MapData mapData;
	public Color trailColor;
    public List<Vector3> nodePositions;
    public float lineWidthMultiplier = 0.01f;
	public float lineHeightAdjustment = 0.005f;
    public GameObject labelGameObject;
    public string trailName;
    
    public Material material;

	private List<TrailLabel> labels = new List<TrailLabel>();
	private List<TrailLabel> activeLabels = new List<TrailLabel>();
	public List<string> usedNames = new List<string>();

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

		foreach (TrailLabel label in labels) {
			
			if (label.state == LabelState.visible && !LabelIsVisibleToCamera (label, planes)) {
				StartCoroutine (FadeOutGradually (label));
				activeLabels.Remove(label);
				usedNames.Remove (label.name);
				continue;
			}

			if (label.state == LabelState.outOfSight) {
				label.labelObject.GetComponent<MeshRenderer> ().enabled = false;

				if (usedNames.Contains (label.name)) {
					continue;
				}
				for (int posNum = 0; posNum < label.unityPositions.Count; posNum += 10) {
					Vector3 position = label.unityPositions [posNum];

					if (LabelIsVisibleToCamera (label, planes)) {
						usedNames.Add (label.name);
						activeLabels.Add (label);

						StartCoroutine (FadeInGradually (label));
						break;
					}
					label.labelObject.transform.localPosition = position;
				}
			}
		}
	}

	public bool LabelIsVisibleToCamera(TrailLabel label, Plane[] planes) {
		Bounds bounds = label.labelObject.GetComponent<MeshRenderer> ().bounds;
		Bounds minBounds = new Bounds ();
		Bounds maxBounds = new Bounds ();
		minBounds.center = bounds.min - new Vector3 (labelEdgeLimit, labelEdgeLimit, labelEdgeLimit);
		maxBounds.center = bounds.max + new Vector3 (labelEdgeLimit, labelEdgeLimit, labelEdgeLimit);

		if (GeometryUtility.TestPlanesAABB (planes, minBounds)
			&& GeometryUtility.TestPlanesAABB (planes, maxBounds)
			&& NoNearbyBounds(bounds)) {
			return true;
		}
		return false;
	}

	public bool NoNearbyBounds(Bounds newBounds) {
		foreach (TrailLabel label in activeLabels) {
			Bounds existingBounds = label.labelObject.GetComponent<MeshRenderer> ().bounds;
			if (existingBounds.Equals(newBounds)) {
				continue;
			}
			if (existingBounds.SqrDistance (newBounds.center) < labelMinDistance) {
				return false;
			} else if (existingBounds.SqrDistance(newBounds.min) < labelMinDistance) {
				return false;
			} else if (existingBounds.SqrDistance(newBounds.max) < labelMinDistance) {
				return false;
			}
		}
		return true;
	}
		
	IEnumerator FadeOutGradually(TrailLabel label) {
		label.state = LabelState.fadingOut;
		MeshRenderer labelRenderer = label.labelObject.GetComponent<MeshRenderer> ();
		labelRenderer.enabled = true;

		float opacity = 1;
		Color fadeColor = labelRenderer.material.color;

		while (opacity > 0) {
			opacity -= Time.deltaTime * 8;
			fadeColor.a = opacity;
			labelRenderer.material.color = fadeColor;
			yield return null;
		}

		label.state = LabelState.outOfSight;
	}

	IEnumerator FadeInGradually(TrailLabel label) {
		label.state = LabelState.fadingIn;
		MeshRenderer labelRenderer = label.labelObject.GetComponent<MeshRenderer> ();
		labelRenderer.enabled = true;

		float opacity = 0;
		Color fadeColor = labelRenderer.material.color;

		while (opacity < 1) {
			opacity += Time.deltaTime * 16;
			fadeColor.a = opacity;
			labelRenderer.material.color = fadeColor;
			yield return null;
		}
		label.state = LabelState.visible;	
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