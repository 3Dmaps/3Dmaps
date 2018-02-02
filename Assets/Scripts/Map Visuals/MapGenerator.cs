using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generates the visual map to the scene.
/// </summary>

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

    public const int mapChunkSize = 121;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;
    public float meshHeightMultiplier;

	public bool autoUpdate;

	public TerrainType[] regions;

	public GameObject visual;

	private MapData mapData;
	private MapMetadata mapMetadata;
	private const string mapDataPath = "Assets/Resources/grandcanyon.txt";

    public void Start()
    {
		mapMetadata = MapDataImporter.ReadMetadata(mapDataPath);
		mapData = MapDataImporter.ReadMapData(mapDataPath, mapMetadata);
        GenerateMap();
    }

    public void GenerateMap() {

        //Just for demo. We can remove this and use real world height data.
		//float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		//float[,] noiseMap = mapData;
		foreach(MapData slice in mapData.GetSlices(255)) {
			int width = slice.GetWidth();
			int height = slice.GetHeight();

			Color[] colourMap = new Color[width * height];
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					float currentHeight = slice.GetSquished(x, y);
					for (int i = 0; i < regions.Length; i++) {
						if (currentHeight <= regions [i].height) {
							colourMap [y * width + x] = regions [i].colour;
							break;
						}
					}
				}
			}

			//MapDisplay display = FindObjectOfType<MapDisplay> ();
			MapDisplay display = gameObject.AddComponent(typeof(MapDisplay)) as MapDisplay;
			display.CreateVisual(visual);
			if (drawMode == DrawMode.NoiseMap) {
				display.DrawTexture (TextureGenerator.TextureFromHeightMap (slice), slice.GetScale());
			} else if (drawMode == DrawMode.ColourMap) {
				display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, width, height), slice.GetScale());
			} else if (drawMode == DrawMode.Mesh) {
				display.DrawMesh (MeshGenerator.GenerateTerrainMesh (slice, meshHeightMultiplier, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, width, height), slice.GetScale());
			}
		}
	}

	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}