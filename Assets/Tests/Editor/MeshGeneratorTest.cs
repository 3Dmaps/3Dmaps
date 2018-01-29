using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MeshGeneratorTest {

    [Test]
    public void MeshGeneratorTopLeftVerticeAltitudeCorrect() {
        float[,] heightMap = new float[3, 3];
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                if (x == 0)
                {
                    heightMap[x, y] = 2;
                }
                else
                {
                    heightMap[x, y] = 3;
                }

            }
        }

        float heightMultiplier = 1F;
        int levelOfDetail = 0;

        MeshData meshdata = MeshGenerator.GenerateTerrainMesh(heightMap, heightMultiplier, levelOfDetail);
        Assert.True(meshdata.vertices[0].y == 2, "Meshdata vertice [0] altitude (y) not correct.");

    }

}
