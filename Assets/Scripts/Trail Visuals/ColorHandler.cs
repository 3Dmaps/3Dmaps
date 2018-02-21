using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHandler {

	private Dictionary<string, Color> colors;

	public ColorHandler() {
		colors = new Dictionary<string, Color> ();

		colors.Add ("red", Color.red);
		colors.Add ("yellow", Color.yellow);
	}

	public Color SelectColor(string colorName) {
		if (colors.ContainsKey(colorName)) {
			return colors[colorName];
		} else {
			return Color.grey;
		}
	}
}
