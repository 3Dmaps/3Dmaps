using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imports data
/// </summary>
public static class DataImporter {

    private static Dictionary<string, MapDataFrame> mapDataFrames = new Dictionary<string, MapDataFrame>();
    
    public static BinaryFileMetadata GetBinaryMapMetaData(string mapName) {
        CreateDataFrame(mapName);
        if (mapDataFrames[mapName].binaryMapMetaData == null) {
            MapDataFrame dataFrame = mapDataFrames[mapName];
            dataFrame.binaryMapMetaData = BinaryFileImporter.ReadMetadata(GetFilePathByName(mapName, PathDataType.height) + ".hdr");
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].binaryMapMetaData;
    }

    public static MapData GetBinaryMapData(string mapName) {
        CreateDataFrame(mapName);
        MapDataFrame dataFrame = mapDataFrames[mapName];
        if (dataFrame.mapData == null) {
            dataFrame.mapData = BinaryFileImporter.ReadMapData(GetFilePathByName(mapName, PathDataType.height) + ".bin", GetBinaryMapMetaData(mapName));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].mapData;
    }

    public static ASCIIGridMetadata GetASCIIMapMetaData(string mapName) {
        CreateDataFrame(mapName);
        if (mapDataFrames[mapName].asciiMapMetaData == null) {
            MapDataFrame dataFrame = mapDataFrames[mapName];
            dataFrame.asciiMapMetaData = ASCIIGridImporter.ReadMetadata(GetFilePathByName(mapName, PathDataType.height) + ".txt");
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].asciiMapMetaData;
    }

    public static MapData GetASCIIMapData(string mapName) {
        CreateDataFrame(mapName);
        MapDataFrame dataFrame = mapDataFrames[mapName];
        if (dataFrame.mapData == null) {
            dataFrame.mapData = ASCIIGridImporter.ReadMapData(GetFilePathByName(mapName, PathDataType.height) + ".txt", GetASCIIMapMetaData(mapName));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].mapData;
    }

    public static OSMData GetOSMData (string mapName) {
        CreateDataFrame(mapName);
        MapDataFrame dataFrame = mapDataFrames[mapName];
        if(dataFrame.osmData == null) {
            dataFrame.osmData = OSMDataImporter.ReadOSMData(GetFilePathByName(mapName, PathDataType.trail) + ".xml");
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].osmData;
    }

	public static SatelliteImage GetSatelliteImage (string mapName, int width, int height) {
		return SatelliteImageImporter.ReadSatelliteImage (GetFilePathByName (mapName, PathDataType.height) + "_satellite.png", width, height);
	}

    private static void CreateDataFrame(string mapName) {
        if (!mapDataFrames.ContainsKey(mapName)) {
            mapDataFrames.Add(mapName, new MapDataFrame());
        }
    }

    private static string GetFilePathByName(string mapName, PathDataType pathDataType) {
        switch (pathDataType) {
            case PathDataType.height:
                break;
            case PathDataType.trail:
                mapName = mapName + "_trails";
                break;
            default:
                break;
        }
        #if UNITY_EDITOR
            return Application.dataPath + "/StreamingAssets/" + mapName;
        #endif

        #if UNITY_IPHONE
            return Application.dataPath + "/Raw/" + mapName;
        #endif

        #if UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/" + mapName;
        #endif
        #pragma warning disable CS0162 // Unreachable code detected
            return mapName;
        #pragma warning restore CS0162 // Unreachable code detected
    }
}

public struct MapDataFrame {
    public string name;

    public BinaryFileMetadata binaryMapMetaData;
    public ASCIIGridMetadata asciiMapMetaData;
    public MapData mapData;
    public OSMData osmData;
}

public enum PathDataType {
    height, trail
}
