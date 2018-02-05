using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapDataImporterTest : IPrebuildSetup {

    public MapMetadata metadata;
    public MapData mapdata;

    public void Setup()
    {
        this.metadata = MapDataImporter.ReadMetadata("Assets/Resources/testData.txt");
        this.mapdata = MapDataImporter.ReadMapData("Assets/Resources/testData.txt", metadata);
    }

    [Test]
    public void MetaDataFromSampleFileWorks() {
        Setup();
        Assert.True(metadata.cellsize == 1.0F, "Cellsize not correct.");
        Assert.True(metadata.ncols == 3, "ncols not correct.");
        Assert.True(metadata.nrows == 3, "nrows not correct.");
        Assert.True(metadata.maxheight == 5.0F, "maxheight not correct.");
        Assert.True(metadata.minheight == 1.0F, "minheight not correct.");
        Assert.True(metadata.nodatavalue == -999.9F, "nodatavalue not correct.");
    }

    [Test]
    public void DataFromSampleFileWorks()
    {
        
    }

}
