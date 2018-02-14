using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailData {
    List<TrailNode> nodeList;
    int id;

    public TrailData (int id) {
        nodeList = new List<TrailNode>();
        this.id = id;
    }

    public void AddNode (TrailNode trailNode) {
        nodeList.Add(trailNode);
    }
}


public struct TrailNode {
    public long id;
    public float lat;
    public float lon;
}