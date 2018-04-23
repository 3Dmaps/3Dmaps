using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Switches Trail color names to corresponding Color objects.  
/// </summary>

public static class ColorHandler {

	public const int maxColorValue = 255;

	private static readonly Color purple = new Color(0.75F, 0F, 0.75F, 0F);
	private static readonly Color orange = new Color(1F, 0.3F, 0F, 0F);
	private static readonly Color skiRouteColor = new Color(0F, 0.8F, 1F, 0F);
	private static readonly Dictionary<string, Color> colors = new Dictionary<string, Color>{
		{"red", Color.red},
		{"yellow", Color.yellow},
		{"blue", Color.blue},
		{"green", Color.green},
		{"purple", purple},
		{"orange", orange},
		{"crosscountryski", skiRouteColor},
		{"crosscountryski_easy", skiRouteColor},
        {"crosscountryski_novice", skiRouteColor},
        {"bicycle", orange},
		{"poiLine", Color.red},
	};
	private static readonly Dictionary<string, Color> areaColors = new Dictionary<string, Color>{
		{"meadow", Color.green},
		{"lake", Color.blue},
	};

	private static float ColorIntToFloat(int value) {
		return (float) value / maxColorValue;
	}

	public static Color ParseColor(string colorString) {
		int[] parts = colorString.Split(new char[]{' '}).Select(s => int.Parse(s)).ToArray();
		return new Color(
			ColorIntToFloat(parts[0]), 
			ColorIntToFloat(parts[1]),
			ColorIntToFloat(parts[2])
			);
	}

	public static Color SelectColor(string colorName) {
		if (colorName != null && colors.ContainsKey(colorName)) {
			return colors[colorName];
		} else {
			return Color.white;
		}
	}
	public static Color SelectAreaColor(string areaType) {
		if (areaType != null && areaColors.ContainsKey(areaType)) {
			return areaColors[areaType];
		} else {
			return Color.white;
		}
	}
}
