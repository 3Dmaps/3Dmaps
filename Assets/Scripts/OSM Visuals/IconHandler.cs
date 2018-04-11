using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switches Point of Interest icon names to corresponding Icon objects.  
/// </summary>

public class IconHandler : MonoBehaviour {

	public List<Icon> iconList; 
	private Dictionary<string, Icon> icons;

	public void generateIconDictionary() {
		icons = new Dictionary<string, Icon> ();
		foreach(Icon icon in iconList) {
			icons.Add(icon.name, icon);
		}
	}
	
	public Icon SelectIcon(string iconName) {
		if (icons.ContainsKey(iconName)) {
			return icons[iconName];
		} else {
			return icons["attraction"];
		}
	}
}
