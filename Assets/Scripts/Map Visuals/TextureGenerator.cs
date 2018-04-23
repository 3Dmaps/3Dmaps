using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generates textures for the mesh. 
/// TextureFromHeightMap: Black &amp; white texture created from height values.
/// TextureFromColourMap: Creates a texture from colour map.
/// </summary>

public static class TextureGenerator {

    private const float colorLerpValue = 0.4f;
	private static AreaDisplay areaDisplay;
	private static TerrainType[] regions;

	public static void SetRegions(TerrainType[] r) {
		regions = r;
	}

	public static Color GetRegionColour(float currentHeight) {
		int min = 0, max = regions.Length - 1, mid = 0;
		while(min < max) {
			mid = (min + max) / 2;
			float h = regions[mid].height;
			if(regions[mid].height <= currentHeight) {
				min = mid + 1;
			} else {
				max = mid - 1;
			}
		}
		return regions[mid].colour;
    }

	public static Color[] ColorMapForSatelliteImage(MapData mapData) {
		SatelliteImage satelliteImage = SatelliteImageService.getSatelliteImage ();
		double scale = satelliteImage.getScale ();

		int textureWidth = (int) (mapData.GetWidth() * scale);
		int textureHeight = (int) (mapData.GetHeight() * scale);
		Color[] colourMap = new Color[textureWidth * textureHeight];
		if(!satelliteImage.hasSatelliteImage()) return colourMap;

        MapDataSlice slice = mapData.AsSlice();
		for (int y = 0; y < textureHeight; y++) {
			int sliceY = (int)(slice.GetY () * scale);
			int flippedY = satelliteImage.texture.height - (int)(sliceY + y) - 1;
			Array.ConstrainedCopy (satelliteImage.texture.GetPixels ((int)(slice.GetX () * scale), flippedY, textureWidth, 1), 0, colourMap, (y * textureWidth), textureWidth); 
		}

		return colourMap;
	}

	public static Color[] ColorMapForHeightAndAreas(MapData mapData, int lod = 0) {
		lod = lod == 0 ? 1 : lod * 2;
		int width  = mapData.GetWidth();
		int height = mapData.GetHeight();
		MapDataSlice slice = mapData.AsSlice();
		Color[] colourMap = new Color[width * height];
		if(areaDisplay == null) {
			areaDisplay = GameObject.FindObjectOfType<AreaDisplay>();
		}

		for (int y = 0; y < height; y += lod) {
			for (int x = 0; x < width; x += lod) {
				float currentHeight = mapData.GetSquished(x, y);
                float scaledPosX = (slice.GetX() + x);
                float scaledPosY = (slice.GetY() + y);
                Color areaColor = areaDisplay.GetPointColor(scaledPosX, scaledPosY);
				Color regionColor = GetRegionColour(currentHeight);

				if (areaColor != Color.clear) {
					regionColor = Color.Lerp(areaColor, regionColor, colorLerpValue);
				}

				for(int actualY = y; actualY < y + lod && actualY < height; actualY++) {
					for(int actualX = x; actualX < x + lod && actualX < width; actualX++) {
						colourMap[actualY * width + actualX] = regionColor;
					}
				}
			}
		}
		return colourMap;
	}

	public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colourMap);
		texture.Apply ();
		return texture;
	}


	public static Texture2D TextureFromHeightMap(MapData mapData) {
		int width = mapData.GetWidth();
		int height = mapData.GetHeight();

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, mapData.GetSquished(x, y));
			}
		}

		return TextureFromColourMap (colourMap, width, height);
	}

}
