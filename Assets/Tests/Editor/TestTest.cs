using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestTest {

	private const bool truth = true;

    [Test]
    public void TestThatSucceeds() {
        Assert.True(truth, "Something went terribly wrong!");
    }

    // [Test]
    // public void TestThatFails() {
    //     Assert.False(truth, "This was bound to happen.");
    // }

}
