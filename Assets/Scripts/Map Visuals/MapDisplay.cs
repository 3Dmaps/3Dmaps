using UnityEngine;
using System.Collections;


/// <summary>
/// Combines the mesh with a texture.
/// </summary>

public class MapDisplay : MonoBehaviour {

	public GameObject visualMap;
	private Renderer textureRender;
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	public GameObject CreateVisual(GameObject visual) {
		visualMap     = Instantiate(visual) as GameObject;
		textureRender = visualMap.GetComponent(typeof(Renderer)) as Renderer;
		meshFilter    = visualMap.GetComponent(typeof(MeshFilter)) as MeshFilter;
		meshRenderer  = visualMap.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        return visualMap;
	}

	public void DrawTexture(Texture2D texture, float scale = 1f) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (scale, 1F, scale);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture, float scale = 1f) {
		meshFilter.sharedMesh             = meshData.CreateMesh ();
		Material material                 = new Material(meshRenderer.sharedMaterial);
		material.mainTexture              = texture;
		meshRenderer.sharedMaterial       = material;
		meshRenderer.transform.localScale = new Vector3(scale, 1F, scale);
	}

}
