using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconHandler : MonoBehaviour {

	public List<Icon> iconList; 
	private Dictionary<string, Icon> icons;

	public IconHandler() {
		icons = new Dictionary<string, Icon> ();
	}
	
	public Icon SelectIcon(string iconName) {
		if (icons.ContainsKey(iconName)) {
			return icons[iconName];
		} else {
			return null;
		}
	}
}