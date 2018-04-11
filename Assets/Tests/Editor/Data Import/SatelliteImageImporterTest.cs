using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SatelliteImageImporterTest {

	public SatelliteImage satelliteImage;
	private float precision;

	[Test]
	public void SatelliteImageLoadingWorks() {
		satelliteImage = SatelliteImageImporter.ReadSatelliteImage ("Assets/Resources/satelliteTest.png", 827, 1024);
		Assert.True (satelliteImage.width == 827, "Wrong width!");
		Assert.True (satelliteImage.height == 1024, "Wrong height!");
		Assert.True (satelliteImage.hasSatelliteImage(), "A satellite image was not loaded!");
		Assert.True (satelliteImage.texture != null, "No texture was loaded!");
		Assert.True (satelliteImage.texture.GetPixels().Length == 827*1024, "Wrong number of pixels!");

	}

	[Test]
	public void LackOfImageFileCreatesEmptySatelliteImageObject() {
		satelliteImage = SatelliteImageImporter.ReadSatelliteImage ("Assets/Resources/wrongfile.png", 10, 10);
		Assert.True(!satelliteImage.hasSatelliteImage (), "A satellite image was erroneously loaded!");
	}
}