using UnityEngine;
using System.Collections;


/// <summary>
/// Draws a piece of map data.
/// </summary>

public class MapDisplay : MonoBehaviour {

	public GameObject visualMap;
	private Renderer textureRender;
	private MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	private MapDisplayData displayData;

	public GameObject CreateVisual(GameObject visual) {
		visualMap     = Instantiate(visual) as GameObject;
		textureRender = visualMap.GetComponent(typeof(Renderer)) as Renderer;
		meshFilter    = visualMap.GetComponent(typeof(MeshFilter)) as MeshFilter;
		meshRenderer  = visualMap.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        return visualMap;
	}

	public void SetMapData(DisplayReadySlice mapData) {
		if(this.displayData == null ) this.displayData = new MapDisplayData(mapData);
		else this.displayData.SetMapData(mapData);
	}

	public void SetRegions(TerrainType[] regions) {
		if(this.displayData == null) {
			this.displayData = new MapDisplayData();
			this.displayData.SetRegions(regions);
		} else displayData.SetRegions(regions);
	}

	public MapDisplayStatus GetStatus() {
		return displayData.status;
	}

	public void SetStatus(MapDisplayStatus newStatus) {
		displayData.SetStatus(newStatus);
	}

	public void DrawMap() {
		MapDisplayStatus status = displayData.PrepareDraw();
		if(status == MapDisplayStatus.HIDDEN) {
			this.visualMap.SetActive(false);
		} else {
			this.visualMap.SetActive(true);
			DrawMesh(displayData.GetMesh(), displayData.GetTexture(), displayData.GetScale());
		}
	}

    public void UpdateMapTexture() {
        Texture2D texture           = displayData.GenerateTexture();
		displayData.texture 		= texture;
        Material material           = new Material(meshRenderer.sharedMaterial);
        material.mainTexture        = texture;
        meshRenderer.sharedMaterial = material;
    }

	public void UpdateLOD(int lod) {
		displayData.UpdateLOD(lod);
	}

	public void DrawTexture(Texture2D texture, float scale = 1f) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (scale, 1F, scale);
	}

	public void DrawMesh(Mesh mesh, Texture2D texture, float scale = 1f) {
		meshFilter.sharedMesh             = mesh;
		Material material                 = new Material(meshRenderer.sharedMaterial);
		material.mainTexture              = texture;
		meshRenderer.sharedMaterial       = material;
		meshRenderer.transform.localScale = new Vector3(scale, 1F, scale);
	}

}

public enum MapDisplayStatus {
	HIDDEN, LOW_LOD, VISIBLE
}
