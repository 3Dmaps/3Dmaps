﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

/// <summary>
/// Generates the visual map to the scene and handles the pieces of said visual map.
/// </summary>

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
    public enum MapName {canyonTestHigh, canyonTestLow, testData};

    public DrawMode drawMode;

    public int mapSliceSize = 200;
    [Range(0, 24)]
    public int levelOfDetail;

    [Range(1, 1000)]
    public int regionsSmoothCount = 100;
	public TerrainType[] regions;

	public GameObject visual;

	public MapData mapData;
	private List<MapDisplay> displays;

    public MapName mapName;

    private DisplayUpdater displayUpdater = new DisplayUpdater(); 
    private int currentZoomValue  = 0;
    public  int displayUpdateRate = 4;
    public Vector2 mapViewerPosition = Vector2.zero;

    public void Start()
    {
        regions     = new MapRegionSmoother().SmoothRegions(regions,regionsSmoothCount);

        string mapFileName = GetMapFileNameFromEnum(mapName);
        mapData = DataImporter.GetASCIIMapData(mapFileName);
		displays = new List<MapDisplay>();
        GenerateMap();

        OSMGenerator osmGenerator = GameObject.FindObjectOfType<OSMGenerator>();
        if (osmGenerator != null) {
            try {
                osmGenerator.GenerateTrails(this, mapFileName);
            } catch(System.Exception e) {
                Debug.Log("Did not generate trails: " + e);
            }
        }   
    }

    public void GenerateMap() {

		MapData actualMapData = drawMode == DrawMode.Mesh ? mapData : new NoiseMapData(mapSliceSize * 2);

		foreach(DisplayReadySlice slice in actualMapData.GetDisplayReadySlices(mapSliceSize, levelOfDetail)) {

			MapDisplay display = gameObject.AddComponent(typeof(MapDisplay)) as MapDisplay;
			GameObject visualObject = display.CreateVisual(visual);
            visualObject.transform.parent = this.transform;

			display.SetRegions(regions);
			display.SetMapData(slice);
            display.SetStatus(MapDisplayStatus.VISIBLE);
			display.DrawMap();
			displays.Add(display);
		}
	}

    public void FixedUpdate()
    {
        int displaysUpdated = 0;
        
        while (displaysUpdated < displayUpdateRate && !displayUpdater.IsEmpty())
        {
            displayUpdater.UpdateNextDisplay();
            displaysUpdated++;
        }
    }


    public void UpdateZoomLevel(int newVal)
    {
        currentZoomValue = newVal;
        UpdateLOD();
    }

    public void UpdateLOD() {
        displayUpdater.Clear();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        int newLod = Mathf.Max(levelOfDetail - currentZoomValue, 0);
		foreach(MapDisplay display in displays) {
            Bounds renderBounds = display.meshRenderer.bounds;
            Vector3 center = renderBounds.center;
            float distanceToCamera = Vector2.Distance(new Vector2(mapViewerPosition.x, mapViewerPosition.y - 0.35F), new Vector2(center.x, center.z));
            int distanceBasedLod = newLod + (int)distanceToCamera * 2;
            if (GeometryUtility.TestPlanesAABB(planes, renderBounds)) {
                displayUpdater.Add(new UnupdatedDisplay(distanceBasedLod, display), distanceBasedLod);
                if(display.GetStatus() == MapDisplayStatus.HIDDEN) {
                    display.SetStatus(MapDisplayStatus.LOW_LOD);
                    display.DrawMap();
                }
            }
            else
            {
                display.SetStatus(MapDisplayStatus.HIDDEN);
                display.DrawMap();
            }
		}
	}

    private string GetMapFileNameFromEnum(MapName mapName) {
        switch (mapName) {
            case MapName.canyonTestHigh:
                return "CanyonTestHigh";

            case MapName.canyonTestLow:
                return "CanyonTestLow";

            case MapName.testData:
                return "testData";

            default:
                Debug.LogError("Error! Invalid map file name value!");
                return "";
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
