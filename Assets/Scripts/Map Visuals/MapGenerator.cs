using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Generates the visual map to the scene and handles the pieces of said visual map.
/// </summary>

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

    public int mapSliceSize = 200;
    [Range(0, 24)]
    public int levelOfDetail;
    public float meshHeightMultiplier;

    [Range(1, 1000)]
    public int regionsSmoothCount = 100;
	public TerrainType[] regions;

	public GameObject visual;

	private MapData mapData;
	private MapMetadata mapMetadata;
	private List<MapDisplay> displays;
    private const string mapDataPath = "Assets/Resources/20x20.txt";
    //private const string mapDataPath = "Assets/Resources/grandcanyon.txt";

    public void Start()
    {
        SmoothRegions(regionsSmoothCount);
		mapMetadata = MapDataImporter.ReadMetadata(mapDataPath);
        mapData     = MapDataImporter.ReadMapData(mapDataPath, mapMetadata);
		displays	= new List<MapDisplay>();
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

		MapData actualMapData = drawMode == DrawMode.Mesh ? mapData : new NoiseMapData(mapSliceSize * 2);

		foreach(DisplayReadySlice slice in actualMapData.GetDisplayReadySlices(mapSliceSize, levelOfDetail)) {

			MapDisplay display = gameObject.AddComponent(typeof(MapDisplay)) as MapDisplay;
			GameObject visualObject = display.CreateVisual(visual);
            visualObject.transform.parent = this.transform;

			display.SetRegions(regions);
			display.SetMapData(slice);
			display.DrawMap();

			displays.Add(display);

		}
	}

	public void UpdateLOD(int zoomValue) {
		int newLod = Mathf.Max(levelOfDetail - zoomValue, 0);
		foreach(MapDisplay display in displays) {
			display.UpdateLOD(newLod);
		}
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;

    public TerrainType(string name, float height, Color colour) {
        this.name = name; this.height = height; this.colour = colour;
    }
}