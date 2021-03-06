﻿using System;
using UnityEngine;

/// <summary>
/// Static class that returns a SatelliteImage object containing current satellite image.
/// </summary>

public static class SatelliteImageService {

	public static SatelliteImage satelliteImage;
	public static bool useSatelliteImage = true;

	public static SatelliteImage getSatelliteImage() {
		return satelliteImage;
	}

	public static bool UseSatelliteImage() {
		return satelliteImage != null && satelliteImage.hasSatelliteImage() && useSatelliteImage;
	}

	public static void ToggleUseSatelliteImage() {
		useSatelliteImage = !useSatelliteImage;
	}
}

public class SatelliteImage {
	public Texture2D texture;
	public int width;
	public int height;

	public bool hasSatelliteImage() {
		if (texture != null) {
			return true;
		}
		return false;
	}

	public double getScale() {
		return (double)texture.width / width;
	}
}
