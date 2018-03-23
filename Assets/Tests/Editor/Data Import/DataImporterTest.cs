using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DataImporterTest {

    [Test]
    public void BinMetaWithValidName() {
        string dataName = "testData";
        Assert.True(DataImporter.GetBinaryMapMetaData(dataName) != null, "GetBinaryMapMetaData is null with valid name! Are we missing " + dataName + "?");
    }

    [Test]
    public void BinDataWithValidName() {
        string dataName = "testData";
        Assert.True(DataImporter.GetBinaryMapData(dataName) != null, "GetBinaryMapData is null with valid name! Are we missing " + dataName + "?");
    }

    [Test]
    public void ASCIIMetaWithValidName() {
        string dataName = "testData";
        Assert.True(DataImporter.GetASCIIMapData(dataName) != null, "GetASCIIMapData is null with valid name! Are we missing " + dataName + "?");
    }

    [Test]
    public void ASCIIDataWithValidName() {
        string dataName = "testData";
        Assert.True(DataImporter.GetASCIIMapMetaData(dataName) != null, "GetASCIIMapMetaData is null with valid name! Are we missing " + dataName + "?");
    }

    [Test]
    public void OSMWithValidName() {
        string dataName = "testData";
        Assert.True(DataImporter.GetOSMData(dataName) != null, "GetOSMData is null with valid name! Are we missing " + dataName + "?");
    }
}
