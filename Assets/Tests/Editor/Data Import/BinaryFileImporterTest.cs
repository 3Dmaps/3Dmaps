using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BinaryFileImporterTest {
    public BinaryFileMetadata metadata;
    public MapData mapdata;
    private float precision;

    [OneTimeSetUp]
    public void Setup() {
        metadata = (BinaryFileMetadata)MapDataImporter.ReadMetadata("Assets/Resources/MapData_BinaryTestData/testBinaryMapFile.hdr", MapDataType.Binary);
        mapdata = MapDataImporter.ReadMapData("Assets/Resources/MapData_BinaryTestData/testBinaryMapFile.bin", metadata, MapDataType.Binary);
        precision = 0.000001F;
    }

    [Test]
    public void MetaDataFromSampleFileWorks() {
        Assert.True(Mathf.Abs((float)metadata.GetCellsize() - 1.0F) < precision, "Cellsize not correct."); // FAILS CURRENTLY.
        Assert.True(metadata.GetColumns() == 11, "ncols not correct.");
        Assert.True(metadata.GetRows() == 11, "nrows not correct.");
        Assert.True(Mathf.Abs(metadata.GetMaxHeight() - 0) < precision, "maxheight not correct.");
        Assert.True(Mathf.Abs(metadata.GetMinHeight() - 0) < precision, "maxheight not correct.");
        Assert.True(metadata.GetDataType() == BinaryDataType.Single, "Data type not correct.");

        Debug.Log(metadata.GetCellsize());
        Debug.Log(metadata.GetColumns());
        Debug.Log(metadata.GetRows());
        Debug.Log(metadata.GetMaxHeight());
        Debug.Log(metadata.GetMinHeight());
        Debug.Log(metadata.GetDataType());

        Debug.Log(metadata.GetLowerLeftCornerX());
        Debug.Log(metadata.GetLowerLeftCornerY());

        Assert.True(Mathf.Abs((float)metadata.GetLowerLeftCornerX() - 12278539.8344981F) < precision, "Lower left corner X value not correct.");
        Assert.True(Mathf.Abs((float)metadata.GetLowerLeftCornerY() - (-1482374.13831618F)) < precision, "Lower left corner Y value not correct.");
        
    }

    [Test]
    public void DataFromSampleFileWorks() {
        Debug.Log(mapdata.GetRaw(0,0));
        Assert.True(Mathf.Abs(mapdata.GetRaw(0, 0) - 0F) < precision, "Altitude (0,0) not correct."); // FAILS CURRENTLY.
    }

}
