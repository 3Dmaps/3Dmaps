using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

/// <summary>
/// Reads trail and node data from an OpenStreetMap XML-file. The input file must 
/// have the ending .xml. The data is stored in an OSMData object. Currently
/// the "node and ""way"-tagged elements are read in and formed into Trails and Areas.
/// Points of interest are stored as POINode objects.
/// </summary>

public class OSMDataImporter {
    private const string wayElement = "way", nodeElement = "node", childNodeElement = "nd";
	private const string idAttribute = "id", latAttribute = "lat", lonAttribute = "lon", 
			refAttribute = "ref", colorKeyValue = "zmeucolor", iconKeyValue="zmeuicon";
	private const string tagElement = "tag";
    

    public static OSMData ReadOSMData(string path) {
        XmlDocument xmlDoc = new XmlDocument();       
        ReadXmlDocument(path, xmlDoc);
        XmlElement rootNode = xmlDoc.DocumentElement;

        Dictionary<long, OSMNode> wayNodes = new Dictionary<long, OSMNode>();
        List<OSMway> ways = new List<OSMway>();           
        OSMData osmData = new OSMData();
        
        foreach (XmlElement node in rootNode) {
            string childNodeType = node.LocalName;
            switch (childNodeType) {
                case wayElement:
                    ReadWay(ways, node);
                    break;
                case nodeElement:
                    ReadTrailNode(osmData, wayNodes, node);
                    break;
                default:
                    break;
            }
        }
        
        foreach (OSMway way in ways) {
            if (way.IsMeadow()) {                
                    FillInWayNodeLatLon(way, wayNodes);
                    osmData.AddArea(new Area (way, "meadow"));                                                                   
            }
            else if (way.IsRiver()) {                
                    FillInWayNodeLatLon(way, wayNodes);
                    osmData.AddRiver(new River (way));                                                                   
            }
            else {
                FillInWayNodeLatLon(way, wayNodes);            
                osmData.AddTrail(new Trail(way));
            }        
        }       
        return osmData;
    }


    private static void ReadTrailNode(OSMData trailData, Dictionary<long, OSMNode> wayNodes, XmlElement node) {
        OSMNode trailNode = new OSMNode();
        if (node.ChildNodes.Count > 0) {
            foreach (XmlElement childNode in node.ChildNodes) {
                if (childNode.LocalName.Equals(tagElement) && childNode.GetAttribute("k").Equals(iconKeyValue)) {
                    POINode poiNode = new POINode(childNode.GetAttribute("v"));
                    poiNode.id      = long.Parse(node.GetAttribute(idAttribute));
                    poiNode.lat     = float.Parse(node.GetAttribute(latAttribute));
                    poiNode.lon     = float.Parse(node.GetAttribute(lonAttribute));
                    trailData.AddPOI(poiNode);
                }
            }
        }
        trailNode.id  = long.Parse(node.GetAttribute(idAttribute));
		trailNode.lat = float.Parse(node.GetAttribute(latAttribute));
		trailNode.lon = float.Parse(node.GetAttribute(lonAttribute));

        wayNodes.Add(trailNode.id, trailNode);
        
        
    }


    private static void ReadWay(List<OSMway> ways, XmlElement node) {
        OSMway way = new OSMway(long.Parse(node.GetAttribute(idAttribute)));
        foreach (XmlElement childNode in node.ChildNodes) {
            if (childNode.LocalName.Equals(childNodeElement)) {
                OSMNode wayNode = new OSMNode();
				wayNode.id = long.Parse(childNode.GetAttribute(refAttribute));
                way.AddNode(wayNode);
            }
            if (childNode.LocalName.Equals (tagElement)) {
                way.AddTag(childNode.GetAttribute ("k"), childNode.GetAttribute ("v"));
            }
        }
        ways.Add(way);
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

    private static void FillInWayNodeLatLon(OSMway way, Dictionary<long, OSMNode> wayNodes) {
        foreach (OSMNode node in way.GetNodeList()) {
			node.lat = wayNodes[node.id].lat;
			node.lon = wayNodes[node.id].lon;
        }
    }
}
