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
    private const string wayElement = "way", nodeElement = "node", childNodeElement = "nd", relationElement  = "relation";
	private const string idAttribute = "id", latAttribute = "lat", lonAttribute = "lon", 
			refAttribute = "ref", roleAttribute = "role", memberTypeAttribute = "member type", colorKeyValue = "zmeucolor", iconKeyValue="zmeuicon", labelName = "name", POIName = "name";
	private const string tagElement = "tag";    

    public static OSMData ReadOSMData(string path) {
        XmlDocument xmlDoc = new XmlDocument();       
        ReadXmlDocument(path, xmlDoc);
        XmlElement rootNode = xmlDoc.DocumentElement;

        Dictionary<long, OSMNode> wayNodes = new Dictionary<long, OSMNode>();
        Dictionary<long, OSMway> ways = new Dictionary<long, OSMway>();                
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

        
        
        foreach (OSMway way in ways.Values) {
            FillInWayNodeLatLon(way, wayNodes);

            if (way.IsArea()) {
                osmData.AddArea(new Area (way, way.LandUse()));
            }
            else if (way.IsRiver()) {
                osmData.AddRiver(new River (way));                                                                   
            }
            else {
                osmData.AddTrail(new Trail(way));
            }        
        }       
        return osmData;
    }


    private static void ReadTrailNode(OSMData trailData, Dictionary<long, OSMNode> wayNodes, XmlElement node) {
        OSMNode trailNode = new OSMNode();
        if (node.ChildNodes.Count > 0) {
            string iconName = "";
            string name = "";
            foreach (XmlElement childNode in node.ChildNodes) {
                if (childNode.LocalName.Equals(tagElement) && childNode.GetAttribute("k").Equals(iconKeyValue)) {
                    iconName = childNode.GetAttribute("v");
                } else if (childNode.LocalName.Equals(tagElement) && childNode.GetAttribute("k").Equals(POIName)) {
                    name = childNode.GetAttribute("v");
                }
            }
            if (!iconName.Equals("") && !name.Equals("")) {
                POINode poiNode = new POINode(iconName, name);
                poiNode.id      = long.Parse(node.GetAttribute(idAttribute));
                poiNode.lat     = float.Parse(node.GetAttribute(latAttribute));
                poiNode.lon     = float.Parse(node.GetAttribute(lonAttribute));
                trailData.AddPOI(poiNode);
            }
        }
        
        trailNode.id  = long.Parse(node.GetAttribute(idAttribute));
		trailNode.lat = float.Parse(node.GetAttribute(latAttribute));
		trailNode.lon = float.Parse(node.GetAttribute(lonAttribute));

        wayNodes.Add(trailNode.id, trailNode);
                
    }
    private static void ReadWay(Dictionary<long, OSMway> ways, XmlElement node) {
        OSMway way = new OSMway(long.Parse(node.GetAttribute(idAttribute)));
        foreach (XmlElement childNode in node.ChildNodes) {
            if (childNode.LocalName.Equals(childNodeElement)) {
                OSMNode wayNode = new OSMNode();
				wayNode.id = long.Parse(childNode.GetAttribute(refAttribute));
                way.AddNode(wayNode);
            }
            if (childNode.LocalName.Equals (tagElement)) {
                way.AddTag(childNode.GetAttribute ("k"), childNode.GetAttribute ("v"));
            } else if(childNode.GetAttribute("k").Equals(labelName)) {
                way.AddTag(childNode.GetAttribute ("k"), childNode.GetAttribute ("v"));
            }
        }
        ways.Add(way.GetID(), way);
    }

    private static void ReadXmlDocument(string path, XmlDocument xmlDoc) {
        try {
            xmlDoc.Load(StreamUtil.GetFileStream(path));
        }
        catch (Exception e) {
            Debug.Log("Got an exception in reading trail data file.");
            Debug.Log(e.ToString());
        }
    }

    private static void FillInWayNodeLatLon(OSMway way, Dictionary<long, OSMNode> wayNodes) {
        OSMNode value;
        foreach (OSMNode node in way.GetNodeList()) {
            if (wayNodes.TryGetValue(node.id, out value)) {
                node.lat = value.lat;
			    node.lon = value.lon;
            }			
        }
    }
}
