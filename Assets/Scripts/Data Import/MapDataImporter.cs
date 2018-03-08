using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum MapDataType {
    ASCIIGrid, Binary
}

/// <summary>
/// Reads height data and related metadata from a file of specified format
/// The data is stored in a MapData object and the metadata in a MapMetadata object
/// </summary>

public static class MapDataImporter {

    public static MapMetadata ReadMetadata(string path, MapDataType type = MapDataType.ASCIIGrid) {
        switch (type) {
            case MapDataType.ASCIIGrid:
                return ASCIIGridImporter.ReadMetadata(path);
            default:
                throw new System.NotImplementedException("Type " + type + " not implemented!");
        }
    }

    public static MapData ReadMapData(string path, MapMetadata metadata, MapDataType type = MapDataType.ASCIIGrid) {
        switch (type) {
            case MapDataType.ASCIIGrid:
                return ASCIIGridImporter.ReadMapData(path, (ASCIIGridMetadata)metadata);
            default:
                throw new System.NotImplementedException("Type " + type + " not implemented!");
        }
    }

}
