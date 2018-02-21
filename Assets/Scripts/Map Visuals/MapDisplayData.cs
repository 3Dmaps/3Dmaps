using UnityEngine;

/// <summary>
/// Handles turning a DisplayReadySlice into something that can be displayed
/// </summary>
public class MapDisplayData {

    private const int lowLod = 20;
    public DisplayReadySlice mapData;
    private TerrainType[] regions;

    public Texture2D texture;
    public Mesh mesh;
    public Mesh lowLodMesh;
    public MapDisplayStatus status;

    public MapDisplayData() { }

    public MapDisplayData(DisplayReadySlice mapData) {
        this.SetMapData(mapData);
    }

    public void SetMapData(DisplayReadySlice mapData) {
        this.mapData = mapData;
        int originalLod = mapData.lod;
        mapData.lod = lowLod;
        lowLodMesh = GenerateMesh();
        mapData.lod = originalLod;
    }

    private Color[] CalculateColourMap(MapData mapData) {
        int width = mapData.GetWidth();
        int height = mapData.GetHeight();
        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float currentHeight = mapData.GetSquished(x, y);
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colourMap[y * width + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        return colourMap;
    }

    public void SetRegions(TerrainType[] regions) {
        this.regions = regions;
    }

    public void SetStatus(MapDisplayStatus newStatus) {
        this.status = newStatus;
    }

    private Mesh GenerateMesh() {
        return MeshGenerator.GenerateTerrainMesh(mapData).CreateMesh();
    }

    private Texture2D GenerateTexture() {
        if (regions != null)
            return TextureGenerator.TextureFromColourMap(CalculateColourMap(mapData), mapData.GetWidth(), mapData.GetHeight());
        else
            return TextureGenerator.TextureFromHeightMap(mapData);
    }

    public void UpdateLOD(int lod) {
        if (mapData.lod != lod) {
            mapData.lod = lod;
            mesh = GenerateMesh();
        }
    }

    public MapDisplayStatus PrepareDraw() {
        if (texture == null) texture = GenerateTexture();
        switch (this.status) {
            case MapDisplayStatus.VISIBLE:
                if (mesh == null) mesh = GenerateMesh();
                break;
            case MapDisplayStatus.LOW_LOD:
                break;
            case MapDisplayStatus.HIDDEN:
                break;
        }
        return this.status;
    }

    public Mesh GetMesh() {
        return (this.status == MapDisplayStatus.VISIBLE && mesh != null) ? mesh : lowLodMesh;
    }

    public Texture2D GetTexture() {
        return texture;
    }

    public float GetScale() {
        return mapData.GetScale();
    }

}
