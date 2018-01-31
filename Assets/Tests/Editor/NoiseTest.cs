using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NoiseTest {

    [Test]
    public void NoiseReturnsCorrectTopLeftCornerValue() {
        //TO DISPLAY PARAMATER NAMES: float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        float[,] noiseMap = Noise.GenerateNoiseMap(121, 121, 0, 25, 4, 0.5F, 2, new Vector2(0F, 0F));

        Assert.True(Mathf.Approximately(noiseMap[0, 0], 0.7286969F));
    }
}
