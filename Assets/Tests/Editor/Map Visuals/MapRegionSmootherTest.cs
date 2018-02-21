using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapRegionSmootherTest
{

    [Test]
    public void RegionSmootherWorks()
    {
        MapRegionSmoother smoother = new MapRegionSmoother();
        TerrainType[] testTerrains = new TerrainType[5];
        for (int i = 0; i < testTerrains.Length; i++)
        {
            TerrainType tt = new TerrainType();
            tt.colour = new Color(i * 10, i * 10, i * 10);
            tt.height = i / 100;
            tt.name = "test" + i;
            testTerrains[i] = tt;
        }

        testTerrains = smoother.SmoothRegions(testTerrains, 10);
        Assert.True(testTerrains.Length == 4 * 10 + 1);
    }
}
