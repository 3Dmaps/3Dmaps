using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds lists of all trails and points of interest derived an OpenStreetMap XML file.
/// </summary>

public class OSMData {

    public List<Trail> trails;
    public List<POINode> poiNodes;

    public OSMData () {
        this.trails = new List<Trail>();
        this.poiNodes = new List<POINode>();
    }

    public void AddTrail (Trail trail) {
        if (!this.trails.Contains(trail)) {
            this.trails.Add(trail);
        }
    }
    public void AddPOI (POINode point) {
        if(!(this.poiNodes.Contains(point))) {
            this.poiNodes.Add(point);
        }
    }
}
