using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds lists of all trails and points of interest derived an OpenStreetMap XML file.
/// </summary>

public class OSMData {

    public List<Trail> trails;
    public List<POINode> poiNodes;
    public List<Area> areas;
    public List<River> rivers;

    public OSMData () {
        this.trails = new List<Trail>();
        this.poiNodes = new List<POINode>();
        this.areas = new List<Area>();
        this.rivers = new List<River>();
    }

    public void AddTrail (Trail trail) {
        if (!this.trails.Contains(trail)) {
            this.trails.Add(trail);
        }
    }

    public void AddRiver (River river) {
        if (!this.rivers.Contains(river)) {
            this.rivers.Add(river);
        }
    }
    public void AddPOI (POINode point) {
        if(!(this.poiNodes.Contains(point))) {
            this.poiNodes.Add(point);
        }
    }

    public void AddArea (Area area) {
        if(!(this.areas.Contains(area))) {
            this.areas.Add(area);
        }
    }
}
