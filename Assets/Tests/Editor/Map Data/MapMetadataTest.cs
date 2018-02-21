using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapMetadataTest {
    public MapMetadata metadata;

    [SetUp]
    public void Setup() {
        this.metadata = new MapMetadata();
    }

    [Test]
    public void SetNColsWorks() {
        metadata.Set("ncols", "7");
        Assert.True(metadata.ncols == 7, "Ncols incorrect.");
    }

    [Test]
    public void SetNRowsWorks() {
        metadata.Set("nrows", "8");
        Assert.True(metadata.nrows == 8, "Nrows incorrect.");
    }

    [Test]
    public void SetCellsizeWorks() {
        metadata.Set("cellsize", "2.0");
        Assert.True(metadata.cellsize == 2.0F, "Cellsize incorrect.");
    }

    [Test]
    public void SetNODATA_valueWorks() {
        metadata.Set("NODATA_value", "-999.9");
        Assert.True(metadata.nodatavalue == -999.9F, "NODATA_value incorrect.");
    }

    [Test]
    public void SetMinheightWorks() {
        metadata.Set("minheight", "-2.2");
        Assert.True(metadata.minheight == -2.2F, "Minheight incorrect.");
    }

    [Test]
    public void SetMaxheightWorks() {
        metadata.Set("maxheight", "70.0");
        Assert.True(metadata.maxheight == 70.0F, "Maxheight incorrect.");
    }
}
