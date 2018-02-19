using UnityEngine;
using System.Collections;

/// <summary>
/// Generates the mesh from height data.
/// All data used by the mesh is stored in the MeshData class for easy access.
/// </summary>

public static class MeshGenerator {

	public static MeshData GenerateTerrainMesh(DisplayReadySlice mapData, float heightMultiplier = 0f, int levelOfDetail = 0, float minHeight = 0f) {
		int width       = mapData.GetWidth();
		int height      = mapData.GetHeight();
        //Debug.Log("w: " + width + "  h:" + height);
		Vector2 topLeft = mapData.GetTopLeft();
		float topLeftX  = topLeft.x;
		float topLeftZ  = topLeft.y;

        int meshSimplificationIncrement = (mapData.lod == 0) ? 1 : mapData.lod * 2;
        int verticesPerLine = mapData.lod > 0 ? (width - 2) / meshSimplificationIncrement + 2 : width;
        int verticesPerColumn = mapData.lod > 0 ? (height - 2) / meshSimplificationIncrement + 2 : height;

        MeshData meshData = new MeshData (verticesPerLine, verticesPerColumn);
		int vertexIndex = 0;

		for (int y = 0; y < height; y += mapData.SimplificationIncrementForY(y)) {
			for (int x = 0; x < width; x += mapData.SimplificationIncrementForX(x)) {

				meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, mapData.GetNormalized(x, y), topLeftZ - y);
				meshData.uvs [vertexIndex]      = new Vector2 (x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle (vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle (vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}
		return meshData;
	}
}

public class MeshData {

	public Vector3[] vertices;
	public int[]     triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices  = new Vector3[meshWidth * meshHeight];
		uvs       = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1) * (meshHeight-1)*6];
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex]     = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex                += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh      = new Mesh ();
		mesh.vertices  = vertices;
		mesh.triangles = triangles;
		mesh.uv        = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}

}