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

	private float[,] mapData;
	private Dictionary<string, float> mapMetadata;
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
		float[,] noiseMap = mapData;

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * mapChunkSize + x] = regions [i].colour;
						break;
					}
				}
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			float minH = (mapMetadata.ContainsKey(MapDataImporter.minheightKey) && mapMetadata[MapDataImporter.minheightKey] != mapMetadata[MapDataImporter.nodatavalueKey]) ? mapMetadata[MapDataImporter.minheightKey] : 0f;
			Debug.Log(minH);
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, levelOfDetail, minH), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
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