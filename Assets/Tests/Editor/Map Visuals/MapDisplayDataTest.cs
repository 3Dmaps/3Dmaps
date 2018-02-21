using System;
using NUnit.Framework;

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
        Assert.True(dispData.GetMesh() == dispData.mesh, "Mesh was not updated!");
    }

    [Test]
    public void MapDisplayData_ReturnsCorrectMeshDependingOnStatus() {
        MapDisplayData dispData = new MapDisplayData(data);
        dispData.UpdateLOD(2); dispData.SetStatus(MapDisplayStatus.VISIBLE);
        Assert.True(dispData.GetMesh() == dispData.mesh, "Mesh was not updated!");
        dispData.SetStatus(MapDisplayStatus.HIDDEN);
        Assert.True(dispData.GetMesh() == dispData.lowLodMesh, "Mesh was not updated!");
    }

}
