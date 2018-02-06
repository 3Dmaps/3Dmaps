using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MeshGeneratorTest {

    [Test]
    public void MeshGeneratorTopLeftVerticeAltitudeCorrect() {
        float[,] heightMap = new float[3, 3];
        for (int x = 0; x < heightMap.GetLength(0); x++) {
            for (int y = 0; y < heightMap.GetLength(1); y++) {
                heightMap[x, y] = (x == 0) ? 2 : 5;
            }
        }

        float heightMultiplier = 1F;
        int levelOfDetail = 0;

        MapData mapData = MapData.ForTesting(heightMap);

        MeshData meshdata = MeshGenerator.GenerateTerrainMesh(mapData, heightMultiplier, levelOfDetail);
        Assert.True(meshdata.vertices[2].y == 1, "Meshdata vertice [2] altitude (y) not correct.");
        Assert.True(meshdata.vertices[0].y == 0, "Meshdata vertice [0] altitude (y) not correct.");

    }

}
