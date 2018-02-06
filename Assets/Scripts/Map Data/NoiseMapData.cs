using UnityEngine;
using System;
using System.Linq;

public class NoiseMapData : MapData {

    private const int seed = 0, octaves = 4;
    private const float scale = 70, persistance = 0.5f, lacunarity = 2;
    private static Vector2 offset = new Vector2(0, 0);
    private const float heightFactor = 1 / 20f;

    public NoiseMapData(int size) : base(Noise.GenerateNoiseMap (size, size, seed, scale, octaves, persistance, lacunarity, offset), new MapMetadata()) {
        metadata.Set(MapMetadata.minheightKey, data.Cast<float>().Min().ToString());
        metadata.Set(MapMetadata.maxheightKey, data.Cast<float>().Max().ToString());
        metadata.Set(MapMetadata.cellsizeKey, (1 / (heightFactor * size)).ToString());
    }

}