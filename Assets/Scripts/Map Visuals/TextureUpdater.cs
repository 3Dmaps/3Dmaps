public class TextureUpdater : DisplayUpdater {
    public override void DoUpdate(MapDisplay display, int lod) {
        if(display.GetTextureLOD() != 0) {
            display.UpdateMapTexture(0);
        }
    }
}