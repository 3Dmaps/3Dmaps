using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single trail. The trail is made up of
/// a list of OSMNodes.
/// </summary>

public class Trail {
    List<OSMNode> nodeList;
    public long id;
	public Color color;

    public Trail(long id) {
        nodeList = new List<OSMNode>();
        this.id = id;
		color = Color.red;
    }

    public Trail(OSMway way) {
        this.nodeList = way.GetNodeList();
        this.id = way.GetID();
        this.color = way.GetColor();
    
    }

    public void AddNode(OSMNode trailNode) {
        nodeList.Add(trailNode);
    }

    public override bool Equals(object obj) {
        var trail = obj as Trail;
        return trail != null &&
               id == trail.id;
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