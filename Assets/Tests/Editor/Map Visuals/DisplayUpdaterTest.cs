using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DisplayUpdaterTest
{
    [Test]
    public void DisplayUpdaterWorks()
    {
        DisplayUpdater du = new DisplayUpdater();
        Assert.True(du.IsEmpty());
        
        du.Add(new UnupdatedDisplay(), 0);
        Assert.False(du.IsEmpty());

        du.Clear();
        Assert.True(du.IsEmpty());

        du.Add(new UnupdatedDisplay(), 0);
        du.UpdateNextDisplay();
        Assert.True(du.IsEmpty());
    }
}
