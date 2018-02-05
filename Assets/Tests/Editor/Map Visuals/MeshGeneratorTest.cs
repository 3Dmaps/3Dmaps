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

        MapMetadata mapMetaData = new MapMetadata();
        mapMetaData.Set("nrows", "3");
        mapMetaData.Set("ncols", "3");
        mapMetaData.Set("cellsize", "1");
        mapMetaData.Set("NODATA_value", "" + int.MinValue);
        mapMetaData.Set("minheight", "2");
        mapMetaData.Set("maxheight", "3");

        MapData mapData = new MapData(heightMap, mapMetaData);

        MeshData meshdata = MeshGenerator.GenerateTerrainMesh(mapData, heightMultiplier, levelOfDetail);
        Assert.True(meshdata.vertices[2].y == 1, "Meshdata vertice [2] altitude (y) not correct.");
        Assert.True(meshdata.vertices[0].y == 0, "Meshdata vertice [0] altitude (y) not correct.");

    }

}
