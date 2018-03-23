using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Service class for converting map coordinates to Unity space coordinates.
/// </summary>

public static class PositionService {

	public static Vector3 GetUnityPosition (DisplayNode node, float heightAdjustment, MapData mapData) {

		float height = mapData.GetNormalized (node.x, node.y);

		float xFromCenter = node.x - mapData.GetWidth() / 2;
		float yFromCenter = (mapData.GetHeight() / 2) - node.y;

		float scale = 1 / ((float) Mathf.Max (mapData.GetWidth(), mapData.GetHeight()) - 1);

		Vector3 nodePosition = new Vector3 ((float) xFromCenter * scale, height + heightAdjustment, (float) yFromCenter * scale);
		return nodePosition;
	}
		
	public static bool IsWithinBounds(int rawX, int rawY, MapData mapData) {
		if (rawX < 0 || rawX > mapData.GetWidth() - 1) {
			return false;		
		}

		if (rawY < 0 || rawY > mapData.GetHeight() - 1) {
			return false;		
		}

		return true;
	}
}
