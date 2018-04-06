using System.IO;
using UnityEngine;

public static class BinaryFileImporter {

    private const string fileStart = "ENVI", separator = "=";
    private const float lowestPossibleHeight = -10000f;

    public static BinaryFileMetadata ReadMetadata(string path) {
        BinaryFileMetadata metadata = new BinaryFileMetadata();
        using (StreamReader input = new StreamReader(path)) {
            if (input.ReadLine() != fileStart) throw new System.ArgumentException(
                 "Binary file header should start with 'ENVI'!"
                 );
            string line, key = null;
            while ((line = input.ReadLine()) != null) {
                if (line.Contains(separator)) {
                    string[] keyValue = line.Split(separator.ToCharArray());
                    if (keyValue.Length != 2) throw new System.ArgumentException(
                         "Invalid header data; multiple separators in a line"
                         );
                    key = keyValue[0];
                    metadata.Add(key, keyValue[1]);
                } else {
                    metadata.Add(key, line); // Extends the key with more data
                }
            }
        }
        metadata.Process();
        return metadata;
    }

    public static MapData ReadMapData(string path, BinaryFileMetadata metadata) {
        float[,] data = new float[metadata.GetColumns(), metadata.GetRows()];
        float min = float.MaxValue, max = float.MinValue;
        using (BinaryReader input = new BinaryReader(File.Open(path, FileMode.Open))) {
            for (int y = 0; y < metadata.GetRows(); y++) {
                for (int x = 0; x < metadata.GetColumns(); x++) {
                    float next = ReadNext(input, metadata);
                    data[x, y] = next;
                    if (next >= lowestPossibleHeight) {
                        min = Mathf.Min(min, next);
                        max = Mathf.Max(max, next);
                    }
                }
            }
        }
        metadata.SetMinHeight(min);
        metadata.SetMaxHeight(max);
        ReplaceNoDataValuesWithMinHeight(metadata, data);

        return new MapData(data, metadata);
    }

    private static void ReplaceNoDataValuesWithMinHeight(BinaryFileMetadata metadata, float[,] data) {
        for (int x = 0; x < metadata.GetColumns(); x++) {
            for (int y = 0; y < metadata.GetRows(); y++) {
                if (data[x, y] < lowestPossibleHeight) {
                    data[x, y] = metadata.GetMinHeight();
                }
            }
        }
    }

    private static float ReadNext(BinaryReader reader, BinaryFileMetadata metadata) {
        switch (metadata.GetDataType()) {
            case BinaryDataType.Single: return reader.ReadSingle();
            case BinaryDataType.Double: return (float)reader.ReadDouble();
            case BinaryDataType.Int16: return reader.ReadInt16();
            case BinaryDataType.Int32: return reader.ReadInt32();
            case BinaryDataType.Int64: return reader.ReadInt64();
            case BinaryDataType.UInt8: return reader.ReadByte();
            case BinaryDataType.UInt16: return reader.ReadUInt16();
            case BinaryDataType.UInt32: return reader.ReadUInt32();
            case BinaryDataType.UInt64: return reader.ReadUInt64();
            default: throw new System.ArgumentException("Unsupported BinaryDataType " + metadata.GetDataType());
        }
    }
}
