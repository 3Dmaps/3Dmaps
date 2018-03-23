using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single river. The river is made up of
/// a list of OSMNodes.
/// </summary>

public class River {
    List<OSMNode> nodeList;
    public long id;
	

    public River(long id) {
        nodeList = new List<OSMNode>();
        this.id = id;		
    }

    public River(OSMway way) {
        this.nodeList = way.GetNodeList();
        this.id = way.GetID();
       
    }

    public void AddNode(OSMNode riverNode) {
        nodeList.Add(riverNode);
    }

    public override bool Equals(object obj) {
        var river = obj as River;
        return river != null &&
               id == river.id;
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