using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// Reads height data and related metadata from an "Arc/Info ASCII grid"-file
/// The data is stored in a MapData object and the metadata in a MapMetadata object
/// </summary>

public static class MapDataImporter {

	public static MapMetadata ReadMetadata(string path) {
		MapMetadata metadata = new MapMetadata();
		using(StreamReader input = new StreamReader(path)) {
			string line;
			bool keepGoing = true;
			while (keepGoing && (line = input.ReadLine()) != null) {
				switch(line[0]) {
					case ' ':
						keepGoing = false; // We hit the actual data, stop reading
						break;
					default:
						string[] keyValue = line.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
						metadata.Set(keyValue[0], keyValue[1]);
						break;
				}
			}
		}
		return metadata;
	}

	public static MapData ReadMapData(string path, MapMetadata metadata) {
		if(metadata.nrows < 1 || metadata.ncols < 1) {
			return null;
		}
		float[,] mapData = new float[metadata.ncols, metadata.nrows];
		float minHeight = metadata.nodatavalue, maxHeight = metadata.nodatavalue;
		using(StreamReader input = new StreamReader(path)) {
			string line;
			int x = 0, y = 0;
			while ((line = input.ReadLine()) != null) {
				if(line[0] == ' ') { // Data lines start with a space
					string[] values = line.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
					x = 0;
					foreach(string value in values) {
						try {
							float height = float.Parse(value);
							if (height < minHeight) {
								minHeight = height;
							} else if (minHeight == metadata.nodatavalue) {
								minHeight = height;
							}
							if (height > maxHeight) {
								maxHeight = height;
							} else if (maxHeight == metadata.nodatavalue) {
								maxHeight = height;
							}
							mapData[x, y] = height;
						} catch (Exception e) {
							Debug.Log(x + ", " + y);
							Debug.Log(e.ToString());
						}
						x++;
					}
					y++;
				}
			}
		}
		metadata.Set(MapMetadata.minheightKey, minHeight.ToString());
		metadata.Set(MapMetadata.maxheightKey, maxHeight.ToString());
		return new MapData(mapData, metadata);
	}

}
