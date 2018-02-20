using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Priority_Queue;

/// <summary>
/// Generates the visual map to the scene and handles the pieces of said visual map.
/// </summary>

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

    public int mapSliceSize = 200;
    [Range(0, 24)]
    public int levelOfDetail;

    [Range(1, 1000)]
    public int regionsSmoothCount = 100;
	public TerrainType[] regions;

	public GameObject visual;

	private MapData mapData;
	private MapMetadata mapMetadata;
	private List<MapDisplay> displays;
    private const string mapDataPath = "Assets/Resources/20x20.txt";
    //private const string mapDataPath = "Assets/Resources/grandcanyon.txt";

    private int currentZoomValue  = 0;
    public  int displayUpdateRate = 4;
    public Vector2 mapViewerPosition = Vector2.zero;
    SimplePriorityQueue<UnupdatedDisplay> unupdatedDisplays = new SimplePriorityQueue<UnupdatedDisplay>();

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

    public void FixedUpdate()
    {
        int displaysUpdated = 0;
        
        while (displaysUpdated < displayUpdateRate && unupdatedDisplays.Any())
        {
            UpdateDisplay(unupdatedDisplays.Dequeue());
            displaysUpdated++;
        }
    }

    private void UpdateDisplay(UnupdatedDisplay ud)
    {
        MapDisplay display = ud.display;
        display.UpdateLOD(ud.lod);
        display.visualMap.SetActive(true);
    }

    public void UpdateZoomLevel(int newVal)
    {
        currentZoomValue = newVal;
        UpdateLOD();
    }

    public void UpdateLOD() {
        unupdatedDisplays.Clear();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        int newLod = Mathf.Max(levelOfDetail - currentZoomValue, 0);
		foreach(MapDisplay display in displays) {
            Bounds renderBounds = display.meshRenderer.bounds;
            Vector3 center = renderBounds.center;
            float distanceToCamera = Vector2.Distance(new Vector2(mapViewerPosition.x, mapViewerPosition.y - 0.35F), new Vector2(center.x, center.z));
            int distanceBasedLod = newLod + (int)distanceToCamera * 2;
            if (GeometryUtility.TestPlanesAABB(planes, renderBounds)) {
                unupdatedDisplays.Enqueue(new UnupdatedDisplay(distanceBasedLod, display), distanceBasedLod);
            }
            else
            {
                display.visualMap.SetActive(false);
            }
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

public struct UnupdatedDisplay {
    public int lod;
    public MapDisplay display;

    public UnupdatedDisplay(int lod, MapDisplay display)
    {
        this.lod = lod; this.display = display;
    }
}