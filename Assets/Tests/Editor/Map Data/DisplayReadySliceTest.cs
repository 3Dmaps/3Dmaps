using System;
using NUnit.Framework;

public class DisplayReadySliceTest {

    MapData data;

    [SetUp]
    public void Setup() {
        data = MapData.ForTesting(new float[5, 5] {
            {0.0F, 0.1F, 0.2F, 0.3F, 0.4F},
            {1.0F, 1.1F, 1.2F, 1.3F, 1.4F},
            {2.0F, 2.1F, 2.2F, 2.3F, 2.4F},
            {3.0F, 3.1F, 3.2F, 3.3F, 3.4F},
            {4.0F, 4.1F, 4.2F, 4.3F, 4.4F},
        });
    }

    [Test]
    public void SimplificationIncrementForYCorrect() {
        DisplayReadySlice slice = new MapDataSlice(data, 0, 0, 4, 5).AsDisplayReadySlice(2);
        Action<int, int> check = (expected, y) => {
            Assert.True(expected == slice.SimplificationIncrementForY(y), 
            "SimplificationIncrementForY wrong at " + y + "; should be " + expected + ", was " + slice.SimplificationIncrementForY(y));
        };
        check(4, 0); check(3, 1); check(2, 2); check(1, 3); check(1, 4);
    }

    [Test]
    public void SimplificationIncrementForXCorrect() {
        DisplayReadySlice slice = new MapDataSlice(data, 1, 0, 5, 3).AsDisplayReadySlice(1);
        Action<int, int> check = (expected, x) => {
            Assert.True(expected == slice.SimplificationIncrementForX(x), 
            "SimplificationIncrementForX wrong at " + x + "; should be " + expected + ", was " + slice.SimplificationIncrementForX(x));
        };
        check(2, 0); check(2, 1); check(1, 2); check(1, 3);
    }

}