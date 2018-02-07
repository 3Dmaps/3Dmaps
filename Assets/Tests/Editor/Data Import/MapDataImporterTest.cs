using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapDataImporterTest {

    public MapMetadata metadata;
    public MapData mapdata;

    [OneTimeSetUp]
    public void Setup() {
        this.metadata = MapDataImporter.ReadMetadata("Assets/Resources/testData.txt");
        this.mapdata = MapDataImporter.ReadMapData("Assets/Resources/testData.txt", metadata);
    }

    [Test]
    public void MetaDataFromSampleFileWorks() {
        Assert.True(metadata.cellsize == 1.0F, "Cellsize not correct.");
        Assert.True(metadata.ncols == 3, "ncols not correct.");
        Assert.True(metadata.nrows == 3, "nrows not correct.");
        Assert.True(metadata.maxheight == 5.0F, "maxheight not correct.");
        Assert.True(metadata.minheight == 1.0F, "minheight not correct.");
        Assert.True(metadata.nodatavalue == -999.9F, "nodatavalue not correct.");
    }

    [Test]
    public void DataFromSampleFileWorks() {
        Assert.True(mapdata.GetRaw(2, 0) == 2.0F, "Altitude (2,0) not correct.");
        Assert.True(mapdata.GetRaw(2, 2) == 5.0F, "Altitude (2,2) not correct.");
    }

    [Test]
    public void DataFromSampleFileReturnsNullWhenNcolsIsZero() {
        MapMetadata metadata2 = MapDataImporter.ReadMetadata("Assets/Resources/testData2_badDataForTesting.txt");
        MapData mapdata2 = MapDataImporter.ReadMapData("Assets/Resources/testData2_badDataForTesting.txt", metadata2);
        Assert.True(mapdata2 == null, "MapDataImporter did not return null when source data ncols was 0.");
    }
}
