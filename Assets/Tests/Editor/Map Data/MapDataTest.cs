using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MapDataTest {
    public MapData mapdata;
    private double precision = 0.00001;
    private double meterInDegrees = 0.0000092592592593;

    [SetUp]
    public void Setup() {
        DummyMetadata metadata = new DummyMetadata();
        metadata.cellsize = 2;
        metadata.minHeight = 1;
        metadata.maxHeight = 6;
        metadata.lowerLeftX = 0;
        metadata.lowerLeftY = 0;
        float[,] data = new float[2, 3] { { 1.0F, 2.0F, 3.0F }, { 4.0F, 5.0F, 6.0F } };
        this.mapdata = new MapData(data, metadata);
    }

    [Test]
    public void SetWorks() {
        mapdata.Set(0, 2, -1.0F);
        Assert.True(mapdata.GetRaw(0, 2) == -1.0F);
    }

    [Test]
    public void GetHeightWorks() {
        Assert.True(mapdata.GetHeight() == 3, "GetHeight() returns incorrect value.");
    }

    [Test]
    public void GetWidthWorks() {
        Assert.True(mapdata.GetWidth() == 2, "GetWidth() returns incorrect value.");
    }

    [Test]
    public void GetScaleWorks() {
        Assert.True(mapdata.GetScale() - 0.333333 < precision, "GetScale() returns incorrect value.");
    }

    [Test]
    public void GetTopLeftWorks() {
        Assert.True(mapdata.GetTopLeft().x == -0.5F, "GetTopLeft() returns incorrect vector x coordinate.");
        Assert.True(mapdata.GetTopLeft().y == 1F, "GetTopLeft() returns incorrect vector y coordinate.");
    }

    [Test]
    public void GetRawWorks() {
        Assert.True(mapdata.GetRaw(1, 0) == 4.0F, "GetRaw(1,0) returns incorrect value.");
    }

    [Test]
    public void GetHeightMultiplierWorks() {
        Assert.True(mapdata.GetHeightMultiplier() - 0.16666666F < precision, "GetHeightMultiplier() returns incorrect value.");
    }

    [Test]
    public void GetNormalizedWorks() {
        Assert.True(mapdata.GetNormalized(1, 2) - 0.83333333F < precision, "GetNormalized(1,2) returns incorrect value.");
    }

    [Test]
    public void GetSquishedWorks() {
        Assert.True(mapdata.GetSquished(1, 2) == 1F, "GetSquished(1,2) returns incorrect value.");
    }

    [Test]
    public void GetSlices_SliceHeightCorrect() {
        List<MapData> slices = mapdata.GetSlices(2);
        MapData slice = slices.ElementAt(0);
        int sliceHeight = slice.GetHeight();
        Assert.True(sliceHeight == 2, "Slice GetHeight() returns incorrect value.");
    }

    [Test]
    public void GetSlices_SliceWidthCorrect() {
        List<MapData> slices = mapdata.GetSlices(2);
        MapData slice = slices.ElementAt(0);
        int sliceWidth = slice.GetWidth();
        Assert.True(sliceWidth == 2, "Slice GetWidth() returns incorrect value.");
    }

    [Test]
    public void GetSlices_GetTopLeftCorrect() {
        List<MapData> slices = mapdata.GetSlices(2);
        MapData slice = slices.ElementAt(2);
        Vector2 sliceTopLeft = slice.GetTopLeft();
        Assert.True(Mathf.Approximately(sliceTopLeft.x, -0.5F), "Slice GetTopLeft() returns incorrect x value:" + sliceTopLeft.x);
        Assert.True(Mathf.Approximately(sliceTopLeft.y, 0F), "Slice GetTopLeft() returns incorrect y value:" + sliceTopLeft.y);
    }

    [Test]
    public void GetSlices_GetRawCorrect() {
        List<MapData> slices = mapdata.GetSlices(2);
        MapData slice = slices.ElementAt(0);
        float altitude = slice.GetRaw(1, 1);
        Assert.True(altitude == 5F, "Slice GetRaw(1,1) returns incorrect value.");
    }

    [Test]
    public void GetSlices_WithOffsetCorrect() {
        List<MapDataSlice> slices = mapdata.GetSlices(1, 2, 2, 3, 2, 2);
        Assert.True(Mathf.Approximately(6.0F, slices[0].GetRaw(0, 0)),
            "Slice with offset 0 GetRaw(0, 0) == " + slices[0].GetRaw(0, 0) + "; should be 6.0");
    }

    [Test]
    public void GetSlices_NeighborRelationsCorrect() {
        List<MapDataSlice> slices = mapdata.GetSlices(0, 0, 2, 3, 2, 2);
        Assert.True(slices[0].GetNeighbors().Count == 2);
        MapData rightNeighsBottomNeigh = null, bottomNeighsRightNeigh = null;
        foreach(MapNeighborRelation rel in slices[0].GetNeighbors()) {
            Assert.True(rel.IsFirstMember(slices[0]));
            MapDataSlice other = (MapDataSlice)rel.GetOther(slices[0]);
            Assert.True(other.GetNeighbors().Contains(rel));
            Assert.False(rel.IsFirstMember(other));
            if(rel.neighborType == NeighborType.LeftRight) {
                rightNeighsBottomNeigh = other.GetNeighbors()
                    .Where((nr) => nr.neighborType == NeighborType.TopBottom)
                    .Where((nr) => nr.IsFirstMember(other))
                    .First().GetOther(other);
            } else {
                bottomNeighsRightNeigh = other.GetNeighbors()
                    .Where((nr) => nr.neighborType == NeighborType.LeftRight)
                    .Where((nr) => nr.IsFirstMember(other))
                    .First().GetOther(other);
            }
        }
        Assert.True(rightNeighsBottomNeigh == bottomNeighsRightNeigh);
    }

    [Test]
    // Used to be "count and lods", now just tests the lods as the actual count of slices is done as with MapDataSlice and that's already tested
    public void GetDisplayReadySlices_LODsCorrect() {
        List<DisplayReadySlice> slices = mapdata.GetDisplayReadySlices(2, 1);
        foreach (DisplayReadySlice slice in slices) {
            Assert.True(1 == slice.lod, "LOD was incorrect for a slice! (should be 1, was " + slice.lod + ")");
        }
    }

    [Test]
    public void GetTopLeftAsLatLonPointCorrect() {
        MapPoint topLeftAsLatLon = mapdata.GetTopLeftLatLonPoint();
        Assert.True(topLeftAsLatLon.x - meterInDegrees < precision, "Top left x in lat-lon incorrect.");
        Assert.True(topLeftAsLatLon.y - (5 * meterInDegrees) < precision, "Top left y in lat-lon incorrect.");
    }

    [Test]
    public void GetTopLeftAsWebMercatorCorrect() {
        MapPoint topLeftAsWebMercator = mapdata.GetTopLeftAsWebMercator();
        Assert.True(topLeftAsWebMercator.x - 1 < precision, "Top left x in WebMercator incorrect.");
        Assert.True(topLeftAsWebMercator.y - 5 < precision, "Top left y in WebMercator incorrect.");
    }

    [Test]
    public void GetLatLonCoordinatesCorrect() {
        MapPoint pointAsLatLon = mapdata.GetLatLonCoordinates(new Vector2(1, -1));
        Assert.True(pointAsLatLon.x - (3 * meterInDegrees) < precision, "Map-relative point x from lat-lon incorrect.");
        Assert.True(pointAsLatLon.y - (3 * meterInDegrees) < precision, "Map-relative point y from lat-lon incorrect.");
    }

    [Test]
    public void GetWebMercatorCoordinatesCorrect() {
        MapPoint pointAsWebMercator = mapdata.GetWebMercatorCoordinates(new Vector2(1, -1));
        Assert.True(pointAsWebMercator.x - 3 < precision, "Map-relative point x from WebMercator incorrect.");
        Assert.True(pointAsWebMercator.y - 3 < precision, "Map-relative point y from WebMercator incorrect.");
    }

    [Test]
    public void Slice_GetWebMercatorCoordinatesCorrect() {
        DummyMetadata metadataTest = new DummyMetadata();
        metadataTest.cellsize = 2;
        metadataTest.minHeight = 1;
        metadataTest.maxHeight = 6;
        metadataTest.lowerLeftX = 0;
        metadataTest.lowerLeftY = 0;
        float[,] data = new float[4, 6] { { 1.0F, 2.0F, 3.0F, 1.0F, 2.0F, 3.0F }, { 4.0F, 5.0F, 6.0F, 4.0F, 5.0F, 6.0F }
        , { 1.0F, 2.0F, 3.0F, 1.0F, 2.0F, 3.0F }, { 4.0F, 5.0F, 6.0F, 4.0F, 5.0F, 6.0F }};
        MapData mapdataTest = new MapData(data, metadataTest);

        List<MapData> slices = mapdataTest.GetSlices(4);
        MapData slice = slices.ElementAt(1);
        MapPoint pointAsWebMercator = slice.GetWebMercatorCoordinates(new Vector2(0f, 0f));
        Assert.True(pointAsWebMercator.x - 7.21515218107713 < precision, "WebMercator x from map-relative coordinates using slice incorrect.");
        Assert.True(pointAsWebMercator.y - 11.3380962851156 < precision, "WebMercator y from map-relative coordinates using slice incorrect.");
    }

    [Test]
    public void GetMapSpecificCoordinatesFromLatLonCorrect() {
        Vector2 mapSpecificVector = mapdata.GetMapSpecificCoordinatesFromLatLon(new MapPoint(3 * meterInDegrees, 3 * meterInDegrees));
        Assert.True(mapSpecificVector.x - 0.5f < precision, "Map-relative point x from lat-lon incorrect.");
        Assert.True(mapSpecificVector.y < precision, "Map-relative point y from lat-lon incorrect.");
    }

    [Test]
    public void GetMapSpecificCoordinatesFromWebMercatorCorrect() {
        Vector2 mapSpecificVector = mapdata.GetMapSpecificCoordinatesFromWebMercator(new MapPoint(3, 3));
        Assert.True(mapSpecificVector.x - 0.5f < precision, "Map-relative point x from WebMercator incorrect.");
        Assert.True(mapSpecificVector.y < precision, "Map-relative point y from WebMercator incorrect.");
    }

    [Test]
    public void Slice_GetMapSpecificCoordinatesFromLatLonCorrect() {
        List<MapData> slices = mapdata.GetSlices(2);
        MapData slice = slices.ElementAt(0);
        Vector2 mapSpecificVector = slice.GetMapSpecificCoordinatesFromLatLon(new MapPoint(3 * meterInDegrees, 5 * meterInDegrees));

        Assert.True(mapSpecificVector.x - 0.5 < precision, "Map-relative point x from lat-lon using slice incorrect.");
        Assert.True(mapSpecificVector.y - 0.5 < precision, "Map-relative point y from lat-lon using slice incorrect.");
    }

    [Test]
    public void GetMapSpecificCoordinatesRelativeToTopLeftFromLatLonCorrect() {
        Vector2 mapSpecificVector = mapdata.GetMapSpecificCoordinatesRelativeToTopLeftFromLatLon(new MapPoint(3 * meterInDegrees, 1 * meterInDegrees));
        Assert.True(mapSpecificVector.x - 1 < precision, "Map top left corner relative point x from lat-lon incorrect.");
        Assert.True(mapSpecificVector.y - 2 < precision, "Map top left corner relative point y from lat-lon incorrect.");
    }

    [Test]
    public void GetMapSpecificCoordinatesRelativeToTopLeftFromWebMercatorCorrect() {
        Vector2 mapSpecificVector = mapdata.GetMapSpecificCoordinatesRelativeToTopLeftFromWebMercator (new MapPoint(3, 1));
        Assert.True(mapSpecificVector.x - 1 < precision, "Map top left corner relative point x from WebMercator incorrect.");
        Assert.True(mapSpecificVector.y - 2 < precision, "Map top left corner relative point y from WebMercator incorrect.");
    }
}
