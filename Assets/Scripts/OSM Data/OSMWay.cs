using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the nodes and tags from 'Way' Elements in OSM Data.
/// </summary>

public class OSMway {
    private long id;
    private List<OSMNode> nodeList;
    private Dictionary<string, string> tags;
	

    public OSMway(long id) {
        this.id = id; 
        this.nodeList = new List<OSMNode>();
        this.tags = new Dictionary<string, string>();   
    }

    public void AddNode(OSMNode node) {
        nodeList.Add(node);
    }

    public void AddTag(string key, string value) {
        tags.Add(key, value);
    }

    public bool IsMeadow() {
        return this.tags.ContainsValue("meadow");
    }

    public bool IsRiver() {
        return this.tags.ContainsValue("river");
    }

    public string LandUse() {
        return tags.ContainsKey("landuse") ? tags["landuse"] : null;
    } 

    public List<OSMNode> GetNodeList() {
        return nodeList;
    }

    public long GetID() {
        return id;
    }

    public string Color() {
        return tags.ContainsKey("zmeucolor") ? tags["zmeucolor"] : null;
    }

}
