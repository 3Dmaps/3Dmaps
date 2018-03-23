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
    }

    public void AddNode(OSMNode node) {
        this.nodeList.Add(node);
    }

    public void AddTag(string key, string value) {
        tags.Add(key, value);
    }

    public bool IsArea() {
        if (this.tags.ContainsKey("landuse")) {           
                 return true;                      
        }
        return false;
    }

    public string LandUse() {
        return this.tags["landuse"];
    } 

    public List<OSMNode> GetNodeList() {
        return nodeList;
    }

    public long GetID() {
        return id;
    }

    public string Color() {
        if (tags.ContainsKey("zmeucolor")) {
            return tags["zmeuclor"];
        }
        return null;
    }

}