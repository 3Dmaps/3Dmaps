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
        if (!tags.ContainsKey(key)) {
            tags.Add(key, value);
        }
    }
    public string getName() {
        if (this.tags.ContainsKey("name")) {
            return this.tags["name"];
        }
        return "";
    }

    public bool IsArea() {
        return this.tags.ContainsKey("landuse");
    }

    public bool IsRiver() {
        return this.tags.ContainsValue("river") || this.tags.ContainsValue("stream");
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

    public Color GetColor() {
        if (tags.ContainsKey("3dmapsrgb")) {
            return ColorHandler.ParseColor(tags["3dmapsrgb"]);
        } else if (tags.ContainsKey("zmeucolor")) {
            return ColorHandler.SelectColor(tags["zmeucolor"]);
        } else if (IsArea()) {
            return ColorHandler.SelectAreaColor(tags["landuse"]);
        } else {
            return Color.white;
        }
    }

    public override bool Equals(object obj) {
        var mway = obj as OSMway;
        return mway != null &&
               id == mway.id;
    }

    public override int GetHashCode() {
        return 1877310944 + id.GetHashCode();
    }
}
