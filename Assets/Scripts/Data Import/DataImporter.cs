using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataImporter : MonoBehaviour {

    Dictionary<string, MapDataFrame> mapDataFrames = new Dictionary<string, MapDataFrame>();
    
    public BinaryFileMetadata GetBinaryMapMetaData(string mapName) {
        CreateDataFrame(mapName);
        if (mapDataFrames[mapName].binaryMapMetaData == null) {
            MapDataFrame dataFrame = mapDataFrames[mapName];
            dataFrame.binaryMapMetaData = BinaryFileImporter.ReadMetadata(GetFilePathByName(mapName, PathDataType.height));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].binaryMapMetaData;
    }

    public MapData GetBinaryMapData(string mapName) {
        CreateDataFrame(mapName);
        MapDataFrame dataFrame = mapDataFrames[mapName];
        if (dataFrame.binaryMapData == null) {
            dataFrame.binaryMapData = BinaryFileImporter.ReadMapData(GetFilePathByName(mapName, PathDataType.height), GetBinaryMapMetaData(mapName));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].binaryMapData;
    }

    public ASCIIGridMetadata GetASCIIMapMetaData(string mapName) {
        CreateDataFrame(mapName);
        if (mapDataFrames[mapName].asciiMapMetaData == null) {
            MapDataFrame dataFrame = mapDataFrames[mapName];
            dataFrame.asciiMapMetaData = ASCIIGridImporter.ReadMetadata(GetFilePathByName(mapName, PathDataType.height));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].asciiMapMetaData;
    }

    public MapData GetASCIIMapData(string mapName) {
        CreateDataFrame(mapName);
        MapDataFrame dataFrame = mapDataFrames[mapName];
        if (dataFrame.binaryMapData == null) {
            dataFrame.binaryMapData = ASCIIGridImporter.ReadMapData(GetFilePathByName(mapName, PathDataType.height), GetASCIIMapMetaData(mapName));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].binaryMapData;
    }

    public TrailData GetTraiData (string mapName) {
        CreateDataFrame(mapName);
        MapDataFrame dataFrame = mapDataFrames[mapName];
        if(dataFrame.trailData == null) {
            dataFrame.trailData = TrailDataImporter.ReadTrailData(GetFilePathByName(mapName, PathDataType.trail));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].trailData;
    }

    private void CreateDataFrame(string mapName) {
        if (!mapDataFrames.ContainsKey(mapName)) {
            mapDataFrames.Add(mapName, new MapDataFrame());
        }
    }

    private string GetFilePathByName(string mapName, PathDataType pathDataType) {
        switch (pathDataType) {
            case PathDataType.height:
                mapName = mapName + ".txt";
                break;
            case PathDataType.trail:
                mapName = mapName + "_trails.xml";
                break;
            default:
                break;
        }
        #if UNITY_EDITOR
            return Application.dataPath + "/StreamingAssets/" + mapName;
        #endif

        #if UNITY_IPHONE
            return Application.dataPath + "/Raw/" + filename;
        #endif

        #if UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/" + filename;
        #endif
        #pragma warning disable CS0162 // Unreachable code detected
            return mapName;
        #pragma warning restore CS0162 // Unreachable code detected
    }
}

public struct MapDataFrame {
    public string name;

    public BinaryFileMetadata binaryMapMetaData;
    public MapData binaryMapData;

    public ASCIIGridMetadata asciiMapMetaData;
    public MapData asciiMapData;

    public TrailData trailData;
}

public enum PathDataType {
    height, trail
}
