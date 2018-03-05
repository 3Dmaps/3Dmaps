using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of a single trail node (point). 
/// </summary>

public class TrailNode {
    long id;
    float lat;
    float lon;

    public TrailNode() {
    }

    public long GetId() {
        return id;
    }

    public void SetId(long id) {
        this.id = id;
    }

    public float GetLat() {
        return lat;
    }

    public void SetLat(float lat) {
        this.lat = lat;
    }

    public float GetLon() {
        return lon;
    }

    public void SetLon(float lon) {
        this.lon = lon;
    }
}