using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a list of all trails from an OpenStreetMap XML file.
/// </summary>

public class TrailData {

    public List<Trail> trails;

    public TrailData() {
        this.trails = new List<Trail>();
    }

    public void AddTrail(Trail trail) {
        if (!this.trails.Contains(trail)) {
            this.trails.Add(trail);
        }
    }
}
