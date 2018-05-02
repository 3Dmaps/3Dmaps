using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates smoother color transitions for map texture.
/// </summary>
public class MapRegionSmoother {
    public TerrainType[] SmoothRegions( TerrainType[]regions, int amount)
    {
        Array.Sort<TerrainType>(regions, (x, y) => x.height.CompareTo(y.height));
        TerrainType[] smoothedRegions = new TerrainType[regions.Length * amount - amount + 1];
        for (int i = 0; i < regions.Length - 1; i++)
        {
            TerrainType current = regions[i];
            TerrainType next = regions[i + 1];

            for (int j = 0; j <= amount; j++)
            {
                float percentage = j == 0 ? 0 : (float)j / (float)amount;
                TerrainType smoothed = new TerrainType();
                smoothed.name = i + "_Smoothed_" + j;
                smoothed.height = current.height + ((next.height - current.height) * percentage);
                smoothed.colour = Color.Lerp(current.colour, next.colour, percentage);
                smoothedRegions[i * amount + j] = smoothed;
            }
        }
        return smoothedRegions;
    }
}
