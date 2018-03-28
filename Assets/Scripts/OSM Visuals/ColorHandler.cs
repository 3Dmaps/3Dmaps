using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switches Trail color names to corresponding Color objects.  
/// </summary>

public class ColorHandler {

	private Dictionary<string, Color> colors;
	private Dictionary<string, Color> areaColors;

	public ColorHandler() {
		colors = new Dictionary<string, Color> ();
		areaColors = new Dictionary<string, Color> ();

		colors.Add ("red", Color.red);
		colors.Add ("yellow", Color.yellow);
		areaColors.Add ("meadow", Color.green);
		areaColors.Add ("lake", Color.blue);
	}

	public Color SelectColor(string colorName) {
		if (colorName != null && colors.ContainsKey(colorName)) {
			return colors[colorName];
		} else {
			return Color.grey;
		}
	}
	public Color SelectAreaColor(string areaType) {
		if (areaType != null && areaColors.ContainsKey(areaType)) {
			return areaColors[areaType];
		} else {
			return Color.grey;
		}
	}
}
