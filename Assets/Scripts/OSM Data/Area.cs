using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single terrain area from OSM data. The trail is made up of
/// a list of TrailNodes.
/// </summary>

public class Area {
    List<OSMNode> nodeList;
    public long id;
	public string type;

    public Area(OSMway way) {
        this.nodeList = way.GetNodeList();
        this.id = way.GetID();
		this.type = way.LandUse();
    }


    public override bool Equals(object obj) {
        var terrain = obj as Trail;
        return terrain != null &&
               id == terrain.id;
    }

    public List<OSMNode> GetNodeList() {
        return nodeList;
    }

    public override int GetHashCode() {
        var hashCode = 476340561;
        hashCode = hashCode * -1521134295 + EqualityComparer<List<OSMNode>>.Default.GetHashCode(nodeList);
        hashCode = hashCode * -1521134295 + id.GetHashCode();
        return hashCode;
    }
}