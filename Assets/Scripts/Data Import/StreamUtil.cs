using System;
using System.IO;
using UnityEngine;

public static class StreamUtil {
/// <summary>
/// Handles platform reading independencies
/// </summary>
    public static Stream GetFileStream(string path) {
        switch(Application.platform) {
            case RuntimePlatform.Android:
                return new MemoryStream(ReadAllBytes(path));
            default:
                return new FileStream(path, FileMode.Open, FileAccess.Read);
        }
    }

    public static byte[] ReadAllBytes(string path) {
        switch(Application.platform) {
            case RuntimePlatform.Android:
                byte[] bytes;
                using(WWW www = new WWW(path)) {
                    while(!www.isDone){}
                    bytes = www.bytes;
                }
                return bytes;
            default:
                return File.ReadAllBytes(path);
        }
    }

}
