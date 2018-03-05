using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

/// <summary>
/// Reads trail and node data from a OpenStreetMap XML-file. The input file must 
/// have the ending .xml. The data is stored in a TrailData object. Currently
/// the "node and ""way"-tagged elements are read in and formed into Trails.
/// </summary>

public class TrailDataImporter {
    private const string wayElement = "way", nodeElement = "node", childNodeElement = "nd";
	private const string idAttribute = "id", latAttribute = "lat", lonAttribute = "lon", 
			refAttribute = "ref", colorKeyValue = "zmeucolor";
	private const string tagElement = "tag";


    public static TrailData ReadTrailData(string path) {
        XmlDocument xmlDoc = new XmlDocument();

        Dictionary<long, TrailNode> trailNodes = new Dictionary<long, TrailNode>();
        TrailData trailData = new TrailData();

        ReadXmlDocument(path, xmlDoc);

        XmlElement rootNode = xmlDoc.DocumentElement;
        
        foreach (XmlElement node in rootNode) {
            string childNodeType = node.LocalName;
            switch (childNodeType) {
                case wayElement:
                    ReadTrail(trailData, node);
                    break;
                case nodeElement:
                    ReadTrailNode(trailNodes, node);
                    break;
                default:
                    break;
            }
        }

        foreach (Trail trail in trailData.trails) {
            FillInTrailNodeLatLon(trail, trailNodes);
        }

        return trailData;
    }

    private static void ReadTrailNode(Dictionary<long, TrailNode> trailNodes, XmlElement node) {
        TrailNode trailNode = new TrailNode();
        trailNode.SetId(long.Parse(node.GetAttribute(idAttribute)));
        trailNode.SetLat(float.Parse(node.GetAttribute(latAttribute)));
        trailNode.SetLon(float.Parse(node.GetAttribute(lonAttribute)));
        trailNodes.Add(trailNode.GetId(), trailNode);
    }

    private static void ReadTrail(TrailData trailData, XmlElement node) {
        Trail trail = new Trail(long.Parse(node.GetAttribute(idAttribute)));

        foreach (XmlElement childNode in node.ChildNodes) {
            if (childNode.LocalName.Equals(childNodeElement)) {
                TrailNode trailNode = new TrailNode();
                trailNode.SetId(long.Parse(childNode.GetAttribute(refAttribute)));
                trail.AddNode(trailNode);
            }
			if (childNode.LocalName.Equals (tagElement) && childNode.GetAttribute("k").Equals(colorKeyValue))  {
				trail.colorName = childNode.GetAttribute ("v");
			}
        }
        trailData.AddTrail(trail);
    }

    private static void ReadXmlDocument(string path, XmlDocument xmlDoc) {
        try {
            xmlDoc.Load(path);
        }
        catch (Exception e) {
            Debug.Log("Got an exception in reading trail data file.");
            Debug.Log(e.ToString());
        }
    }

    private static void FillInTrailNodeLatLon(Trail trail, Dictionary<long, TrailNode> trailNodes) {
        foreach (TrailNode trailNode in trail.GetNodeList()) {
            trailNode.SetLat(trailNodes[trailNode.GetId()].GetLat());
            trailNode.SetLon(trailNodes[trailNode.GetId()].GetLon());
        }
    }
}