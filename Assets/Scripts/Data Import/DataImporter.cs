using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataImporter : MonoBehaviour {

    Dictionary<string, MapDataFrame> mapDataFrames = new Dictionary<string, MapDataFrame>();
    
    public MapMetadata GetMapMetaDataByMapName(string mapName) {
        if(!mapDataFrames.ContainsKey(mapName)) {
            mapDataFrames.Add(mapName, new MapDataFrame());
        }
        if(mapDataFrames[mapName].mapMetaData == null) {
            MapDataFrame dataFrame = mapDataFrames[mapName];
            dataFrame.mapMetaData = MapDataImporter.ReadMetadata(GetFilePathByName(mapName, PathDataType.test));
            mapDataFrames[mapName] = dataFrame;
        }
        return mapDataFrames[mapName].mapMetaData;
    }

    private string GetFilePathByName(string mapName, PathDataType pathDataType) {
        return "test";
    }
}

public struct MapDataFrame {
    public string name;
    public MapMetadata mapMetaData;
    public MapData mapData;
    public ASCIIGridMetadata asciiGridMetaData;
    public MapData asciiMapData;
    public TrailData trailData;
}

public enum PathDataType {
    test, test2
}
