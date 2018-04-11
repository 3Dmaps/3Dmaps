using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Reads satellite image from file and turns it into a Texture2D object.
/// </summary>

public static class SatelliteImageImporter {

	public static SatelliteImage ReadSatelliteImage(string path, int width, int height) {
		SatelliteImage satelliteImage = new SatelliteImage();
		satelliteImage.width = width;
		satelliteImage.height = height;

		byte[] imageData;

		try {
			imageData = File.ReadAllBytes (path);
		} catch (Exception e) {
			Debug.Log (e.Message);
			Debug.Log ("Satellite image at '" + path + "' not found!");

			satelliteImage.texture = null;
			return satelliteImage;
		}
			
		Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		texture.filterMode = FilterMode.Point;
		texture.LoadImage (imageData); 

		satelliteImage.texture = texture;

		return satelliteImage;
	}
}
