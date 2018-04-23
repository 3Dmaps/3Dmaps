using System;
using UnityEngine;

/// <summary>
/// Handles turning a DisplayReadySlice into something that can be displayed
/// </summary>
public class MapDisplayData {

    private const int lowLod = 20;
    public DisplayReadySlice mapData;

	public Texture2D texture;
    public int textureLod;
    private Mesh mesh;
	public Mesh Mesh {
		get {
			return mesh;
		}
		set {
			if(mesh != null) {
				MonoBehaviour.Destroy(mesh);
			}
			mesh = value;
		}
	}
	public Mesh lowLodMesh;
	public MapDisplayStatus status;
	private AreaDisplay areaDisplay;

	public MapDisplayData(){}

	public MapDisplayData(DisplayReadySlice mapData) {
		this.SetMapData(mapData);
	}

	public void SetMapData(DisplayReadySlice mapData) {
		this.mapData    = mapData;
		int originalLod = mapData.lod;
		mapData.lod     = lowLod;
		lowLodMesh      = GenerateMesh();
		mapData.lod     = originalLod;
	}

    public Color[] CalculateColourMap(MapData mapData, int lod) {
		if(SatelliteImageService.UseSatelliteImage()) {
            textureLod = 0;
            return TextureGenerator.ColorMapForSatelliteImage(mapData);
        } else {
            textureLod = lod;
            return TextureGenerator.ColorMapForHeightAndAreas(mapData, lod);
        }
	}

	public void SetStatus(MapDisplayStatus newStatus) {
		this.status = newStatus;
	}

	private Mesh GenerateMesh() {
		return FixNormals(MeshGenerator.GenerateTerrainMesh(mapData).CreateMesh());
	}

    private static void FillInNormals(Vector3[] normals, int coord, int inc, 
        Func<int, int> indexFunc, Vector3 normal) {
        
        if(inc > 1 && coord > 0) {
            for(int i = 1; i < inc; i++) {
                normals[indexFunc(coord - i)] = normal;
            }
        }

    }

    private static void FixNormalEdge(
        Vector3[] first, int firstDimension, Func<int, int> firstIndexFunc,
        Vector3[] second, int secondDimension, Func<int, int> secondIndexFunc
        ) {
        
        int firstCoord = 0, secondCoord = 0;
        int firstInc = secondDimension / firstDimension > 0 ? secondDimension / firstDimension : 1;
        int secondInc = firstDimension / secondDimension > 0 ? firstDimension / secondDimension : 1;
        while(firstCoord < firstDimension && secondCoord < secondDimension) {
            int firstIndex = firstIndexFunc(firstCoord);
            int secondIndex = secondIndexFunc(secondCoord);
            Vector3 firstNormal = first[firstIndex];
            Vector3 secondNormal = second[secondIndex];
            Vector3 consensus = (firstNormal + secondNormal);
            consensus.Normalize();
            first[firstIndex] = consensus;
            second[secondIndex] = consensus;
            FillInNormals(first, firstCoord, firstInc, firstIndexFunc, consensus);
            FillInNormals(second, secondCoord, secondInc, secondIndexFunc, consensus);
            firstCoord += firstInc;
            secondCoord += secondInc;
        }

    }

    public static void FixNormals(
        Vector3[] first, int firstWidth, int firstHeight,
        Vector3[] second, int secondWidth, int secondHeight,
        NeighborType relation) {
        
        if(relation == NeighborType.TopBottom) {
            FixNormalEdge(
                first, firstWidth, (x) => firstHeight*(firstWidth - 1) + x,
                second, secondWidth, (x) => x
            );
        } else if (relation == NeighborType.LeftRight) {
            FixNormalEdge(
                first, firstHeight, (y) => y * firstWidth + (firstWidth - 1),
                second, secondHeight, (y) => y * secondWidth
            );
        } else throw new System.ArgumentException("Unsupported NeighborType " + relation);

    }

    public Mesh FixNormals(Mesh mesh) {
        Vector3[] normals = mesh.normals;
        int width = MeshGenerator.GetVerticesPerDimension(mapData.GetWidth(), GetActualLOD());
        int height = MeshGenerator.GetVerticesPerDimension(mapData.GetHeight(), GetActualLOD());
        foreach(DisplayNeighborRelation relation in mapData.GetDisplayNeighbors()) {
            MapDisplay other = relation.GetOtherDisplay(mapData);
            if(other == null || other.GetStatus() == MapDisplayStatus.HIDDEN) continue;
            Mesh otherMesh = relation.GetOtherMesh(mapData);
            DisplayReadySlice otherSlice = relation.GetOtherDRSlice(mapData);
            Vector3[] otherNormals = otherMesh.normals;
            int otherWidth =  MeshGenerator.GetVerticesPerDimension(otherSlice.GetWidth(), other.GetActualLOD());
            int otherHeight = MeshGenerator.GetVerticesPerDimension(otherSlice.GetHeight(), other.GetActualLOD());
            if(relation.IsFirstMember(mapData)) {
                FixNormals(normals, width, height,
                              otherNormals, otherWidth, otherHeight,
                              relation.neighborType);
            } else {
                FixNormals(otherNormals, otherWidth, otherHeight,
                              normals, width, height,
                              relation.neighborType);
            }
            otherMesh.normals = otherNormals;
        }
        mesh.normals = normals;
        return mesh;
    }

    public Texture2D GenerateTexture(int lod) {
        return TextureGenerator.TextureFromColourMap(CalculateColourMap(mapData, lod), mapData.GetWidth(), mapData.GetHeight());
	}

	public void UpdateLOD(int lod) {
		if(mapData.lod != lod) {
			mapData.lod = lod;
			Mesh = GenerateMesh();
		}
	}

	public int GetActualLOD() {
		return status == MapDisplayStatus.LOW_LOD ? lowLod * 2 : mapData.GetActualLOD();
	}

    public MapDisplayStatus PrepareDraw() {
        switch(this.status) {
			case MapDisplayStatus.VISIBLE:
				if(Mesh == null) Mesh = GenerateMesh();
				break;
			case MapDisplayStatus.LOW_LOD:
				break;
			case MapDisplayStatus.HIDDEN:
				break;
		}
		return this.status;
	}

	public Mesh GetMesh() {
		return (this.status == MapDisplayStatus.VISIBLE && Mesh != null) ? Mesh : lowLodMesh;
	}

	public Texture2D GetTexture() {
		return texture;
	}

	public float GetScale() {
		return mapData.GetScale();
	}

}
