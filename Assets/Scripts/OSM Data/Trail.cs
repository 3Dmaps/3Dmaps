using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single trail. The trail is made up of
/// a list of TrailNodes.
/// </summary>

public class Trail {
    List<TrailNode> nodeList;
    public long id;
	public string colorName;

    public Trail(long id) {
        nodeList = new List<TrailNode>();
        this.id = id;
		colorName = "red";
    }

    public void AddNode(TrailNode trailNode) {
        nodeList.Add(trailNode);
    }

    public override bool Equals(object obj) {
        var trail = obj as Trail;
        return trail != null &&
               id == trail.id;
    }

    public List<TrailNode> GetNodeList() {
        return nodeList;
    }

    public override int GetHashCode() {
        var hashCode = 476340561;
        hashCode = hashCode * -1521134295 + EqualityComparer<List<TrailNode>>.Default.GetHashCode(nodeList);
        hashCode = hashCode * -1521134295 + id.GetHashCode();
        return hashCode;
    }
}