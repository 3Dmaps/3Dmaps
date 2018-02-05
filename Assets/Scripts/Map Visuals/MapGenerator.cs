using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Generates the visual map to the scene.
/// </summary>

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

    public const int mapChunkSize = 121;
    [Range(0, 6)]
    public int levelOfDetail;
    public float meshHeightMultiplier;

    [Range(1, 1000)]
    public int regionsSmoothCount = 100;
	public TerrainType[] regions;

	public GameObject visual;

	private MapData mapData;
	private MapMetadata mapMetadata;
	private const string mapDataPath = "Assets/Resources/grandcanyon.txt";

    public void Start()
    {
        SmoothRegions(regionsSmoothCount);
		mapMetadata = MapDataImporter.ReadMetadata(mapDataPath);
		mapData     = MapDataImporter.ReadMapData(mapDataPath, mapMetadata);
        GenerateMap();
    }

    private void SmoothRegions(int amount)
    {
        Array.Sort<TerrainType>(regions, (x, y) => x.height.CompareTo(y.height));
        TerrainType[] smoothedRegions = new TerrainType[regions.Length * amount - amount + 1];
        for (int i = 0; i < regions.Length - 1; i++)
        {
            TerrainType current = regions[i];
            TerrainType next    = regions[i + 1];

            for (int j = 0; j <= amount; j++)
            {
                float percentage = j == 0 ? 0 : (float)j / (float)amount;
                TerrainType smoothed = new TerrainType();
                smoothed.name   =  i + "_Smoothed_" + j;
                smoothed.height = current.height + ((next.height - current.height) * percentage);
                smoothed.colour = Color.Lerp(current.colour, next.colour, percentage);
                smoothedRegions[i * amount + j] = smoothed;
            }
        }
        regions = smoothedRegions;
    }

    public void GenerateMap() {

        // GenerateNoiseMap returns noise, if need it create a new MapData with fake MapMetaData
		//float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

		foreach(MapData slice in mapData.GetSlices(121)) {
			int width  = slice.GetWidth();
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

			MapDisplay display = gameObject.AddComponent(typeof(MapDisplay)) as MapDisplay;
			GameObject visualObject = display.CreateVisual(visual);
            visualObject.transform.parent = this.transform;
			if (drawMode == DrawMode.NoiseMap) {
				display.DrawTexture (TextureGenerator.TextureFromHeightMap (slice), slice.GetScale());
			} else if (drawMode == DrawMode.ColourMap) {
				display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, width, height), slice.GetScale());
			} else if (drawMode == DrawMode.Mesh) {
				display.DrawMesh (MeshGenerator.GenerateTerrainMesh (slice, meshHeightMultiplier, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, width, height), slice.GetScale());
			}
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}