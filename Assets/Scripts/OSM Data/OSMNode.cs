using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single trail node (point). 
/// </summary>

public class OSMNode {
    public long id;
    public float lat;
    public float lon;

    public OSMNode() {
    }

    public override bool Equals(object obj) {
        var node = obj as OSMNode;
        return node != null &&
               id == node.id;
    }

    public override int GetHashCode() {
        return 1877310944 + id.GetHashCode();
    }
}
