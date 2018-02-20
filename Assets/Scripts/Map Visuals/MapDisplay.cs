using UnityEngine;
using System.Collections;


/// <summary>
/// Draws a piece of map data.
/// </summary>

public class MapDisplay : MonoBehaviour {

	private const int lowLod = 20;

	public GameObject visualMap;
	private Renderer textureRender;
	private MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public DisplayReadySlice mapData;
	private TerrainType[] regions;

	private Texture2D texture;
	private Mesh mesh;
	private Mesh lowLodMesh;
	public MapDisplayStatus status;

	public GameObject CreateVisual(GameObject visual) {
		visualMap     = Instantiate(visual) as GameObject;
		textureRender = visualMap.GetComponent(typeof(Renderer)) as Renderer;
		meshFilter    = visualMap.GetComponent(typeof(MeshFilter)) as MeshFilter;
		meshRenderer  = visualMap.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        return visualMap;
	}

	private Color[] CalculateColourMap(MapData mapData) {
		int width  = mapData.GetWidth();
		int height = mapData.GetHeight();
		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				float currentHeight = mapData.GetSquished(x, y);
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * width + x] = regions [i].colour;
						break;
					}
				}
			}
		}
		return colourMap;
	}

	public void SetMapData(DisplayReadySlice mapData) {
		this.mapData = mapData;
		int originalLod = mapData.lod;
		mapData.lod = lowLod;
		lowLodMesh = GenerateMesh();
		mapData.lod = originalLod;
	}

	public void SetRegions(TerrainType[] regions) {
		this.regions = regions;
	}

	public void SetStatus(MapDisplayStatus newStatus) {
		this.status = newStatus;
	}

	private Mesh GenerateMesh() {
		return MeshGenerator.GenerateTerrainMesh(mapData).CreateMesh();
	}

	private Texture2D GenerateTexture() {
		if (regions != null)
			return TextureGenerator.TextureFromColourMap(CalculateColourMap(mapData), mapData.GetWidth(), mapData.GetHeight());
		else
			return TextureGenerator.TextureFromHeightMap(mapData);
	}

	public void DrawMap() {
		if(texture == null) texture = GenerateTexture();
		switch(this.status) {
			case MapDisplayStatus.VISIBLE:
				if(mesh == null) mesh = GenerateMesh();
				this.visualMap.SetActive(true);
				DrawMesh(mesh, texture, mapData.GetScale());
				break;
			case MapDisplayStatus.LOW_LOD:
				this.visualMap.SetActive(true);
				DrawMesh(lowLodMesh, texture, mapData.GetScale());
				break;
			case MapDisplayStatus.HIDDEN:
				this.visualMap.SetActive(false);
				break;
		}
	} 

	public void UpdateLOD(int lod) {
		if(mapData.lod != lod) {
			mapData.lod = lod;
			mesh = GenerateMesh();
		}
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
