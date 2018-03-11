using System;
using System.Collections.Generic;
using UnityEngine;

public enum BinaryDataType {
    // http://www.harrisgeospatial.com/docs/ENVIHeaderFiles.html
    Int16 = 2, Int32 = 3, Int64 = 14,
    UInt8 = 1, UInt16 = 12, UInt32 = 13,  UInt64 = 15,
    Single = 4, Double = 5
}

/// <summary>
/// Container for binary file metadata.
/// http://www.harrisgeospatial.com/docs/ENVIHeaderFiles.html
/// </summary>
public class BinaryFileMetadata : MapMetadata {

    private const string rowsKey = "lines", columnsKey = "samples", dataTypeKey = "data type", byteOrderKey = "byte order", 
        mapInfoKey = "map info";
    private const int byteOrderLittleEndian = 0;

    private Dictionary<string, string> data;
    private bool processingDone = false;
    private int rows, columns;
    private float minHeight = 0.0f, maxHeight = 0.0f;
    private float cellsize;
    private double lowerLeftCornerX, lowerLeftCornerY;
    private BinaryDataType dataType;


    public BinaryFileMetadata() {
        data = new Dictionary<string, string>();
    }

    public void Add(string key, string value) {
        key = key.Trim(); // Sanitize keys
        if(!data.ContainsKey(key)) data[key] = "";
        data[key] += value;
        processingDone = false;
    }

    public void Process() {
        rows = int.Parse(data[rowsKey]);
        columns = int.Parse(data[columnsKey]);
        if(int.Parse(data[byteOrderKey]) != byteOrderLittleEndian) throw new System.ArgumentException(
            "Unsupported byte order " + byteOrderKey + "! (We currently only support little endian byte order)"
            );
        dataType = ProcessDataType();
        ProcessMapInfo();
        processingDone = true;
    }

    private BinaryDataType ProcessDataType() {
        int dataTypeId = int.Parse(data[dataTypeKey]);
        if(!Enum.IsDefined(typeof(BinaryDataType), dataTypeId)) {
            throw new System.ArgumentException(
                dataTypeId + " is not a supported data type."
            );
        }
        return (BinaryDataType) dataTypeId;
    }

    private void ProcessMapInfo() {
        BinaryFileMapInfo info = BinaryFileMapInfo.Parse(data[mapInfoKey]);
        cellsize = CoordinateConverter.LatLonDegreesToDefaultMeters(info.pixelSize);
        CoordinateConverter conv = new CoordinateConverter(cellsize);
        int deltaX = 1 - info.refX; // Lower left corner is map point (1, columns)
        int deltaY = rows - info.refY; // (In this case indexing starts at 0)
        lowerLeftCornerX = conv.TransformCoordinateByDistance(deltaX, info.refEasting);
        lowerLeftCornerY = conv.TransformCoordinateByDistance(deltaY, info.refNorthing);
    }

    private void CheckIfProcessed() {
        if(!processingDone) throw new System.FieldAccessException(
            "Tried to access BinaryFileMetadata data before doing metadata.Process()"
            );
    }

    public void SetMinHeight(float minHeight) {
        this.minHeight = minHeight;
    }

    public void SetMaxHeight(float maxHeight) {
        this.maxHeight = maxHeight;
    }

    public int GetRows() {
        CheckIfProcessed();
        return rows;
    }

    public int GetColumns() {
        CheckIfProcessed();
        return columns;
    }

    public BinaryDataType GetDataType() {
        CheckIfProcessed();
        return dataType;
    }

    public float GetCellsize() {
        CheckIfProcessed();
        return cellsize;
    }

    public double GetLowerLeftCornerX(){
        CheckIfProcessed();
        return lowerLeftCornerX;
    }

    public double GetLowerLeftCornerY() {
        CheckIfProcessed();
        return lowerLeftCornerY;
    }

    public float GetMaxHeight() {
        return maxHeight;
    }

    public float GetMinHeight() {
        return minHeight;
    }
}

class BinaryFileMapInfo {
    private const char separator = ',';
    private static readonly char[] charsToTrim = new char[]{' ', '\n', '{', '}'};
    private const float pixelSizeTolerance = 0.1f;

    public string projection;
    public int refX, refY;
    public double refEasting, refNorthing;
    public double pixelSize;

    public static BinaryFileMapInfo Parse(string info) {

        string[] fields = info.Split(separator);
        for(int i = 0; i < fields.GetLength(0); i++) {
            fields[i] = fields[i].Trim(charsToTrim);
        }

        BinaryFileMapInfo result = new BinaryFileMapInfo();
        // Field indices from http://www.harrisgeospatial.com/docs/ENVIHeaderFiles.html
        result.projection = fields[0];
        result.refX = int.Parse(fields[1]);
        result.refY = int.Parse(fields[2]);
        result.refEasting = double.Parse(fields[3]);
        result.refNorthing = double.Parse(fields[4]);
        double pixelSizeX = double.Parse(fields[5]);
        double pixelSizeY = double.Parse(fields[6]);
        if(Mathf.Abs((float) (1 - pixelSizeX / pixelSizeY)) > pixelSizeTolerance) {
            throw new System.ArgumentException("Pixel sizes differ too much!");
        }
        result.pixelSize = (pixelSizeX + pixelSizeY) / 2;

        return result;
    }
}