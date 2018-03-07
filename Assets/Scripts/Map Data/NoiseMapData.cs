using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Generates and contains randomly generated height data
/// </summary>

public class NoiseMapData : MapData {

    private const int seed = 0, octaves = 4;
    private const float scale = 70, persistance = 0.5f, lacunarity = 2;
    private static Vector2 offset = new Vector2(0, 0);
    private const float heightFactor = 1 / 20f;

    public NoiseMapData(int size) : base(Noise.GenerateNoiseMap (size, size, seed, scale, octaves, persistance, lacunarity, offset), new DummyMetadata()) {
        ((DummyMetadata) metadata).minHeight = data.Cast<float>().Min();
        ((DummyMetadata) metadata).maxHeight = data.Cast<float>().Max();
        ((DummyMetadata) metadata).cellsize = (1 / (heightFactor * size));
    }

}
