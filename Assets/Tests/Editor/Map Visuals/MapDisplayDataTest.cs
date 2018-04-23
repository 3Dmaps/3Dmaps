using System;
using NUnit.Framework;
using UnityEngine;

public class MapDisplayDataTest {

    DisplayReadySlice data;

    [SetUp]
    public void Setup() {
        data = MapData.ForTesting(new float[5, 5] {
            {0.0F, 0.1F, 0.2F, 0.3F, 0.4F},
            {1.0F, 1.1F, 1.2F, 1.3F, 1.4F},
            {2.0F, 2.1F, 2.2F, 2.3F, 2.4F},
            {3.0F, 3.1F, 3.2F, 3.3F, 3.4F},
            {4.0F, 4.1F, 4.2F, 4.3F, 4.4F},
        }).AsSlice().AsDisplayReadySlice(5);
    }

    [Test]
    public void MapDisplayData_SetMapDataInitializesMesh() {
        MapDisplayData dispData = new MapDisplayData();
        Assert.True(dispData.GetMesh() == null, "There was a mesh before there was any data!");
        dispData.SetStatus(MapDisplayStatus.VISIBLE); dispData.SetMapData(data);
        Assert.True(dispData.GetMesh() == dispData.lowLodMesh, "Mesh was not initialized!");
    }

    [Test]
    public void MapDisplayData_UpdateLODUpdatesMesh() {
        MapDisplayData dispData = new MapDisplayData(data);
        dispData.SetStatus(MapDisplayStatus.VISIBLE); dispData.UpdateLOD(2);
        Assert.True(dispData.GetMesh() == dispData.Mesh, "Mesh was not updated!");
    }

    [Test]
    public void MapDisplayData_ReturnsCorrectMeshDependingOnStatus() {
        MapDisplayData dispData = new MapDisplayData(data);
        dispData.UpdateLOD(2); dispData.SetStatus(MapDisplayStatus.VISIBLE);
        Assert.True(dispData.GetMesh() == dispData.Mesh, "Mesh was not updated!");
        dispData.SetStatus(MapDisplayStatus.HIDDEN);
        Assert.True(dispData.GetMesh() == dispData.lowLodMesh, "Mesh was not updated!");
    }

    [Test]
    public void MapDisplayData_FixesLeftRightNormalEdgeCorrectly() {
        Vector3[] n1 = new Vector3[]{
            new Vector3(0, 1, 0), new Vector3(0, 2, 0),
            new Vector3(0, 1, 0), new Vector3(0, 0, 1),
        };
        Vector3[] n2 = new Vector3[]{
            new Vector3(0, 3, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
            new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
            new Vector3(0, 0, 2), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
        };
        MapDisplayData.FixNormals(n1, 2, 2, n2, 3, 3, NeighborType.LeftRight);
        Action<Vector3, float, float, float> checkVector = (v3, x, y, z) => {
            Assert.True(Mathf.Approximately(v3.x, x), "Incorrect X, should be: " + x + ", was: " + v3.x);
            Assert.True(Mathf.Approximately(v3.y, y), "Incorrect Y, should be: " + y + ", was: " + v3.y);
            Assert.True(Mathf.Approximately(v3.z, z), "Incorrect Z, should be: " + z + ", was: " + v3.z);
        };
        checkVector(n1[1], 0, 1, 0); checkVector(n1[3], 0, Mathf.Sqrt(2)/2, Mathf.Sqrt(2)/2);
        checkVector(n2[0], 0, 1, 0); checkVector(n2[3], 0, Mathf.Sqrt(2)/2, Mathf.Sqrt(2)/2);
    }

    public void MapDisplayData_FixesTopBottomNormalEdgeCorrectly() {
        Vector3[] n1 = new Vector3[]{
            new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
            new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
            new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0),
            new Vector3(1, 0, 0), new Vector3(3, 0, 1), new Vector3(2, 0, 0), new Vector3(0, 0, 1),
        };
        Vector3[] n2 = new Vector3[]{
            new Vector3(0, 1, 0), new Vector3(2, 0, 0),
            new Vector3(0, 1, 0), new Vector3(0, 1, 0),
        };
        MapDisplayData.FixNormals(n1, 4, 4, n2, 2, 2, NeighborType.TopBottom);
        Action<Vector3, float, float, float> checkVector = (v3, x, y, z) => {
            Assert.True(Mathf.Approximately(v3.x, x), "Incorrect X, should be: " + x + ", was: " + v3.x);
            Assert.True(Mathf.Approximately(v3.y, y), "Incorrect Y, should be: " + y + ", was: " + v3.y);
            Assert.True(Mathf.Approximately(v3.z, z), "Incorrect Z, should be: " + z + ", was: " + v3.z);
        };
        checkVector(n1[12], Mathf.Sqrt(2)/2, Mathf.Sqrt(2)/2, 0); checkVector(n1[13], 1, 0, 0); checkVector(n1[14], 1, 0, 0);
        checkVector(n2[0], Mathf.Sqrt(2)/2, Mathf.Sqrt(2)/2, 0); checkVector(n2[1], 1, 0, 0);
    }

	[Test]
	public void SatelliteImageCorrectlySlicedAndFlippedToForTopLeftSlice() {
		MapDisplayData dispData = new MapDisplayData();
		data.SetX (0);
		data.SetY (0);
		dispData.SetMapData(data);

		SatelliteImage image = new SatelliteImage ();
		Color[] textureMap = new Color[20*20];
		Texture2D texture = new Texture2D (20, 20);
		texture.SetPixels (textureMap);
		image.texture = texture;
		image.width = 10;
		image.height = 10;

		texture.SetPixel (0, 19, Color.red);
		texture.SetPixel (9, 10, Color.blue);

        SatelliteImageService.satelliteImage = image;

		Color[] colorMap = dispData.CalculateColourMap(data, 0);
		Assert.True (colorMap [0] == Color.red);
		Assert.True (colorMap [99] == Color.blue);
	}


	[Test]
	public void SatelliteImageCorrectlySlicedAndFlippedToForBottomRightSlice() {
		MapDisplayData dispData = new MapDisplayData();
		data.SetX (5);
		data.SetY (5);
		dispData.SetMapData(data);

		SatelliteImage image = new SatelliteImage ();
		Color[] textureMap = new Color[20*20];
		Texture2D texture = new Texture2D (20, 20);
		texture.SetPixels (textureMap);
		image.texture = texture;
		image.width = 10;
		image.height = 10;

		texture.SetPixel (10, 9, Color.red);
		texture.SetPixel (19, 0, Color.blue);

        SatelliteImageService.satelliteImage = image;

		Color[] colorMap = dispData.CalculateColourMap (data, 0);
		Assert.True (colorMap [0] == Color.red);
		Assert.True (colorMap [99] == Color.blue);
	}
}