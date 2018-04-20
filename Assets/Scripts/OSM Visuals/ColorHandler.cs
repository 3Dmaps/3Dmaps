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

        Color skiRouteColor = new Color(0F, 0.8F, 1F, 0F);
        Color orange = new Color(1F, 0.3F, 0F, 0F);
        
        colors.Add ("red", Color.red);
		colors.Add ("yellow", Color.yellow);
        colors.Add("blue", Color.blue);
        colors.Add("green", Color.green);
        colors.Add("purple", new Color(0.75F, 0F, 0.75F, 0F));
        colors.Add("orange", orange);
        colors.Add("crosscountryski", skiRouteColor);
        colors.Add("crosscountryski_easy", skiRouteColor );
        colors.Add("crosscountryski_novice", skiRouteColor);
        colors.Add("bicycle", orange);
		colors.Add("poiLine", Color.red);
        areaColors.Add ("meadow", Color.green);
		areaColors.Add ("lake", Color.blue);

	}

	public Color SelectColor(string colorName) {
		if (colorName != null && colors.ContainsKey(colorName)) {
			return colors[colorName];
		} else {
			return Color.white;
		}
	}
	public Color SelectAreaColor(string areaType) {
		if (areaType != null && areaColors.ContainsKey(areaType)) {
			return areaColors[areaType];
		} else {
			return Color.white;
		}
	}
}
