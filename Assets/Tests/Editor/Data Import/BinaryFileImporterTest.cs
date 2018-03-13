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
        precision = 0.001F;
    }

    [Test]
    public void MetaDataFromSampleFileWorks() {
        Assert.True(Mathf.Abs((float)metadata.GetCellsize() - 10.0F) < precision, "Cellsize not correct.");
        Assert.True(metadata.GetColumns() == 10, "ncols not correct.");
        Assert.True(metadata.GetRows() == 13, "nrows not correct.");
        Assert.True(Mathf.Abs(metadata.GetMaxHeight() - 1960.91F) < precision, "maxheight not correct.");
        Assert.True(Mathf.Abs(metadata.GetMinHeight() - 1951.887F) < precision, "minheight not correct.");
        Assert.True(metadata.GetDataType() == BinaryDataType.Single, "Data type not correct.");
        Assert.True(Mathf.Abs((float)(metadata.GetLowerLeftCornerX() - (-12492736.0119444))) < precision, "Lower left corner X value not correct.");
        Assert.True(Mathf.Abs((float)(metadata.GetLowerLeftCornerY() - (4302859.27676796))) < precision, "Lower left corner Y value not correct.");

    }

    [Test]
    public void DataFromSampleFileWorks() {
        Assert.True(Mathf.Abs(mapdata.GetRaw(0, 0) - 1960.15F) < precision, "Altitude (0,0) not correct.");
        Assert.True(Mathf.Abs(mapdata.GetRaw(1, 0) - 1960.127F) < precision, "Altitude (1,0) not correct.");
        Assert.True(Mathf.Abs(mapdata.GetRaw(9, 12) - 1953.431F) < precision, "Altitude (9,12) not correct.");
    }

}
