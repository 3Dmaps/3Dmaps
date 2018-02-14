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

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public float getLat() {
        return lat;
    }

    public void setLat(float lat) {
        this.lat = lat;
    }

    public float getLon() {
        return lon;
    }

    public void setLon(float lon) {
        this.lon = lon;
    }
}