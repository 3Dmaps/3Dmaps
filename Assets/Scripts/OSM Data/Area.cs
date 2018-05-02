using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single terrain area from OSM data. The trail is made up of
/// a list of OSMNodes.
/// </summary>

public class Area {
    public List<OSMNode> nodeList;
    public long id;
    public string type;
    public Color color;

    public Area(OSMway way, string type) {
        this.nodeList = way.GetNodeList();
        this.id = way.GetID();
        this.color = way.GetColor();
        this.type = type;
    }

    public override bool Equals(object obj) {
        var area = obj as Area;
        return area != null &&
               id == area.id;
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
