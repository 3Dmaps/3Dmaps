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

    public OSMData() {
        trails = new List<Trail>();
        poiNodes = new List<POINode>();
        areas = new List<Area>();
        rivers = new List<River>();
    }

    public void AddTrail(Trail trail) {
        if (!trails.Contains(trail)) {
            trails.Add(trail);
        }
    }

    public void AddRiver(River river) {
        if (!rivers.Contains(river)) {
            rivers.Add(river);
        }
    }
    public void AddPOI(POINode point) {
        if (!(poiNodes.Contains(point))) {
            poiNodes.Add(point);
        }
    }

    public void AddArea(Area area) {
        if (!(areas.Contains(area))) {
            areas.Add(area);
        }
    }
}
