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

    public Trail(long id) {
        nodeList = new List<TrailNode>();
        this.id = id;
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
}