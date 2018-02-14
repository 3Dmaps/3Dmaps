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
    public void GetWidthCorrect() {
        DisplayReadySlice slice = new DisplayReadySlice(new MapDataSlice(data, 2, 2, 5, 5), 2);
        Assert.True(slice.GetWidth() == 1, "DisplayReadySlice width was " + slice.GetWidth() + ", should have been 1");
    }

    [Test]
    public void GetHeightCorrect() {
        DisplayReadySlice slice = new DisplayReadySlice(new MapDataSlice(data, 3, 0, 5, 5), 2);
        Assert.True(slice.GetHeight() == 5, "DisplayReadySlice width was " + slice.GetHeight() + ", should have been 5");
    }

}