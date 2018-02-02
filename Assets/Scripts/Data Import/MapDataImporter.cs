using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MapDataImporter {

	public const string nrowsKey = "nrows", ncolsKey = "ncols", nodatavalueKey = "NODATA_value", minheightKey = "minheight";
	public static Dictionary<string, float> ReadMetadata(string path) {
		Dictionary<string, float> metadata = new Dictionary<string, float>();
		using(StreamReader input = new StreamReader(path)) {
			string line;
			bool keepGoing = true;
			while (keepGoing && (line = input.ReadLine()) != null) {
				switch(line[0]) {
					case ' ':
						keepGoing = false;
						break;
					default:
						string[] keyValue = line.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
						metadata.Add(keyValue[0], float.Parse(keyValue[1]));
						break;
				}
			}
			foreach(string key in metadata.Keys) {
				Debug.Log(key + ", " + metadata[key]);
			}
		}
		return metadata;
	}

	public static float[,] ReadMapData(string path, Dictionary<string, float> metadata) {
		if(!metadata.ContainsKey(nrowsKey) || !metadata.ContainsKey(ncolsKey)) {
			return null;
		}
		float[,] mapData = new float[(int) metadata[nrowsKey], (int) metadata[ncolsKey]];
		float minHeight = metadata[nodatavalueKey];
		using(StreamReader input = new StreamReader(path)) {
			string line;
			int x = 0, y = 0;
			while ((line = input.ReadLine()) != null) {
				if(line[0] == ' ') {
					string[] values = line.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
					x = 0;
					foreach(string value in values) {
						try {
							float height = float.Parse(value);
							if (height < minHeight) {
								minHeight = height;
							} else if (minHeight == metadata[nodatavalueKey]) {
								minHeight = height;
							}
							mapData[y, x] = height;
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
		metadata.Add(minheightKey, minHeight);
		return mapData;
	}

}
