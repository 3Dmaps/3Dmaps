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

public class OSMDataImporter {
    private const string wayElement = "way", nodeElement = "node", childNodeElement = "nd";
	private const string idAttribute = "id", latAttribute = "lat", lonAttribute = "lon", 
			refAttribute = "ref", colorKeyValue = "zmeucolor", iconKeyValue="zmeuicon";
	private const string tagElement = "tag";


    public static OSMData ReadTrailData(string path) {
        XmlDocument xmlDoc = new XmlDocument();

        Dictionary<long, TrailNode> trailNodes = new Dictionary<long, TrailNode>();
        OSMData osmData = new OSMData();

        ReadXmlDocument(path, xmlDoc);

        XmlElement rootNode = xmlDoc.DocumentElement;
        
        foreach (XmlElement node in rootNode) {
            string childNodeType = node.LocalName;
            switch (childNodeType) {
                case wayElement:
                    ReadTrail(osmData, node);
                    break;
                case nodeElement:
                    ReadTrailNode(osmData, trailNodes, node);
                    break;
                default:
                    break;
            }
        }

        foreach (Trail trail in osmData.trails) {
            FillInTrailNodeLatLon(trail, trailNodes);
        }

        return osmData;
    }

    private static void ReadTrailNode(OSMData trailData ,Dictionary<long, TrailNode> trailNodes, XmlElement node) {
        TrailNode trailNode = new TrailNode();
        if (node.ChildNodes.Count > 0) {


            foreach (XmlElement childNode in node.ChildNodes) {
                if (childNode.LocalName.Equals(tagElement) && childNode.GetAttribute("k").Equals(iconKeyValue)) {
                    POINode poiNode = new POINode(childNode.GetAttribute("v"));
                    poiNode.id = long.Parse(node.GetAttribute(idAttribute));
                    poiNode.lat = float.Parse(node.GetAttribute(latAttribute));
                    poiNode.lon = float.Parse(node.GetAttribute(lonAttribute));
                    trailData.AddPOI(poiNode);

                }
            }
        }
        trailNode.id = long.Parse(node.GetAttribute(idAttribute));
		trailNode.lat = float.Parse(node.GetAttribute(latAttribute));
		trailNode.lon = float.Parse(node.GetAttribute(lonAttribute));

        trailNodes.Add(trailNode.id, trailNode);
        
        
    }

    private static void ReadTrail(OSMData trailData, XmlElement node) {
        Trail trail = new Trail(long.Parse(node.GetAttribute(idAttribute)));

        foreach (XmlElement childNode in node.ChildNodes) {
            if (childNode.LocalName.Equals(childNodeElement)) {
                TrailNode trailNode = new TrailNode();
				trailNode.id = long.Parse(childNode.GetAttribute(refAttribute));
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
			trailNode.lat = trailNodes[trailNode.id].lat;
			trailNode.lon = trailNodes[trailNode.id].lon;
        }
    }
}