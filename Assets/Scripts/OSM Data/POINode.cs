using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Holds the data of a single point of interest node (point).
///</summary>

public class POINode {

    public long id;
    public float lat;
    public float lon;
    public string icon;
    public string name;

    public POINode(string icon, string name) {
        this.icon=icon;
        this.name=name;               
    }
}
