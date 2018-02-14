using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class TrailDataImporter {
    public TrailDataImporter() {

    }

    public TrailData ReadTrailData(string path) {
        XmlDocument xmlDoc = new XmlDocument();

		Dictionary<long, TrailNode> trailNodes = new Dictionary<long, TrailNode> ();
		List<Way> ways = new List<Way> ();

        try {
            xmlDoc.Load(path);
        }
        catch (Exception e) {
            Debug.Log("Got an exception in reading trail data file.");
            Debug.Log(e.ToString());
        }

        TrailData trailData = new TrailData(1);

        XmlElement rootNode = xmlDoc.DocumentElement;
        Debug.Log(rootNode);
        int count = 0;
        foreach (XmlElement node in rootNode) {
            count++;

			if (node.LocalName.Equals("way")) {
				Debug.Log("way element found!");
				Way way = new Way () { id = long.Parse (node.GetAttribute ("id")), nodes = new List<long>() };
				foreach (XmlElement childNode in node.ChildNodes) {
					if (childNode.LocalName.Equals ("nd")) {
						way.nodes.Add (long.Parse(childNode.GetAttribute ("ref")));
					}
				}
				Debug.Log ("Number of nodes in way: " + way.nodes.Count);
				ways.Add (way);
			}

			if (node.LocalName.Equals ("node")) {
				TrailNode trailNode = new TrailNode () { id = long.Parse (node.GetAttribute ("id")), 
					lat = float.Parse(node.GetAttribute ("lat")), lon = float.Parse(node.GetAttribute ("lon")) };
				trailNodes.Add(trailNode.id, trailNode);
				//Debug.Log (trailNode.id);
			}

            if (count == 50) {
                //break;
            }
        }
		Debug.Log ("Number of nodes: " + trailNodes.Count);
		Debug.Log ("Number of ways: " + ways.Count);

        return trailData;
    }
}

public struct Way {
    public long id;
    public List<long> nodes;
} 
