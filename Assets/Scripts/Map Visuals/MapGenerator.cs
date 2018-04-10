using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

/// <summary>
/// Generates the visual map to the scene and handles the pieces of said visual map.
/// </summary>

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
    public enum MapName {canyonTestHigh, canyonTestLow, testData, canyonTestBinary};

    public DrawMode drawMode;

	public int maxZoomValue = 10;
	public int minZoomValue = 0;

    public int mapSliceSize = 200;
    [Range(0, 24)]
    public int levelOfDetail;

    [Range(1, 1000)]
    public int regionsSmoothCount = 100;
    public TerrainType[] regions;

    public GameObject visual;

    public MapData mapData;
    private MapMetadata mapMetadata;
    private List<MapDisplay> displays;
    public MapName mapName;

    private DisplayUpdater displayUpdater = new DisplayUpdater();
    private int currentZoomValue = 0;
    public int displayUpdateRate = 4;
    public Vector2 mapViewerPosition = Vector2.zero;

    public void Start()
    {
        regions = new MapRegionSmoother().SmoothRegions(regions,regionsSmoothCount);

        string mapFileName = GetMapFileNameFromEnum(mapName);
        MapDataType mapDataType = GetMapFileTypeFromEnum(mapName);

        // A quick fix to enable binary map reading. Needs to be done better.
        if (mapDataType == MapDataType.Binary) {
            mapData = DataImporter.GetBinaryMapData(mapFileName);
        } else if (mapDataType == MapDataType.ASCIIGrid) {
            mapData = DataImporter.GetASCIIMapData(mapFileName);
        } else {
            Debug.LogError("Error! Importin map data from file " + mapFileName + " failed.");
        }

        displays = new List<MapDisplay>();

        GenerateMap();
        OSMGenerator osmGenerator = GameObject.FindObjectOfType<OSMGenerator>();
        if (osmGenerator != null) {
            try {
                osmGenerator.GenerateOSMObjects(this, mapFileName);
            } catch(System.Exception e) {
                Debug.Log("Did not generate trails: " + e);
            }
        }
    }

    public void UpdateTextures() {
        foreach (MapDisplay display in displays) {
            
            display.UpdateMapTexture();
        }
    }

    public void GenerateMap() {

        MapData actualMapData = drawMode == DrawMode.Mesh ? mapData : new NoiseMapData(mapSliceSize * 2);

        foreach (DisplayReadySlice slice in actualMapData.GetDisplayReadySlices(mapSliceSize, levelOfDetail)) {

			      GameObject child = new GameObject ();
			      child.transform.parent = this.transform;
            MapDisplay display = child.AddComponent(typeof(MapDisplay)) as MapDisplay;
            GameObject visualObject = display.CreateVisual(visual);
			      visualObject.transform.parent = child.transform;

            display.SetRegions(regions);
            display.SetMapData(slice);
            display.SetStatus(MapDisplayStatus.VISIBLE);
            display.DrawMap();
            displays.Add(display);
        }
    }

    public void FixedUpdate() {
        int displaysUpdated = 0;

        while (displaysUpdated < displayUpdateRate && !displayUpdater.IsEmpty()) {
            displayUpdater.UpdateNextDisplay();
            displaysUpdated++;
        }
    }


    public void UpdateZoomLevel(int newVal) {
		if (newVal > maxZoomValue) {
			currentZoomValue = maxZoomValue;
		} else if (newVal < minZoomValue) {
			currentZoomValue = minZoomValue;
		} else {
			currentZoomValue = newVal;
		}

		UpdateLOD();
    }

    public void UpdateLOD() {
        displayUpdater.Clear();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        int newLod = Mathf.Max(levelOfDetail - currentZoomValue, 0);
        foreach (MapDisplay display in displays) {
            Bounds renderBounds = display.meshRenderer.bounds;
			renderBounds.Expand (3.0F);
            Vector3 center = renderBounds.center;
            float distanceToCamera = Vector2.Distance(new Vector2(mapViewerPosition.x, mapViewerPosition.y - 0.35F), new Vector2(center.x, center.z));
            int distanceBasedLod = newLod + (int)distanceToCamera * 2;
            if (GeometryUtility.TestPlanesAABB(planes, renderBounds)) {
                displayUpdater.Add(new UnupdatedDisplay(distanceBasedLod, display), distanceBasedLod);
                if (display.GetStatus() == MapDisplayStatus.HIDDEN) {
                    display.SetStatus(MapDisplayStatus.LOW_LOD);
                    display.DrawMap();
                }
            } else {
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

            case MapName.canyonTestBinary:
                return "CanyonTestBinary";

            default:
                Debug.LogError("Error! Invalid map file name value!");
                return "";
        }
    }

    private MapDataType GetMapFileTypeFromEnum(MapName mapName) {
        switch (mapName) {
            case MapName.canyonTestHigh:
                return MapDataType.ASCIIGrid;

            case MapName.canyonTestLow:
                return MapDataType.ASCIIGrid;

            case MapName.testData:
                return MapDataType.ASCIIGrid;

            case MapName.canyonTestBinary:
                return MapDataType.Binary;

            default:
                Debug.LogError("Error! Invalid map file name value!");
                return MapDataType.Binary;
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

    public UnupdatedDisplay(int lod, MapDisplay display) {
        this.lod = lod; this.display = display;
    }
}
