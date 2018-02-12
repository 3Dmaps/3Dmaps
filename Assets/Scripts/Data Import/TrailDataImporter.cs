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

            TrailNode trailNode = new TrailNode() { id = int.Parse(node.GetAttribute("id")), lat = 0, lon = 0 };
            //Debug.Log(trailNode.id);
            if (count == 50) {
                break;
            }
        }
        
        
        return trailData;
    }

}
public struct Way {
    int id;
    List<int> nodes;
} 
