using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

public static class Unzipper {

    private const int bufferSize = 2048;

    public static string Unzip(string path) {

        string outputPath = Application.temporaryCachePath;

        using(ZipInputStream input = new ZipInputStream(StreamUtil.GetFileStream(path))) {

            ZipEntry entry;
            while((entry = input.GetNextEntry()) != null) {
                using (FileStream streamWriter = File.Create(
                    Path.Combine(outputPath, entry.Name))
                    ) {

                    byte[] data = new byte[bufferSize];
                    int size = bufferSize;
                    while (size > 0) {
                        size = input.Read(data, 0, data.Length);
                        if (size > 0) {
                            streamWriter.Write(data, 0, size);
                        }
                    }

                }
            } 
        }

        return outputPath;
    }
}