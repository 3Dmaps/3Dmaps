using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;



public class TextureGeneratorTest
{

    [Test]
    public void TextureColorCorrectWithSmallColourMap()
    {
        Color[] colourMap = new Color[9];
        
        for (int x = 0; x < colourMap.GetLength(0); x++)
        {
            if (x == 0) colourMap[x] = Color.black;
            else colourMap[x] = Color.white;
        }

        Texture2D texture2D = TextureGenerator.TextureFromColourMap(colourMap,3,3);
        Assert.True(texture2D.GetPixel(0, 0).Equals(Color.black), "Test heightmap (0,0) not black.");
        Assert.True(texture2D.GetPixel(2, 2).Equals(Color.white), "Test heightmap (2,2) not white.");
    }


    [Test]
    public void TextureColorCorrectWithSmallHeightMap()
    {
        float[,] heightMap = new float[3, 3];
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                if (x == 0)
                {
                    heightMap[x, y] = 0;
                }
                else
                {
                    heightMap[x, y] = 1;
                }

            }
        }

        Texture2D texture2D = TextureGenerator.TextureFromHeightMap(heightMap);
        Assert.True(texture2D.GetPixel(0, 0).Equals(Color.black), "Test heightmap (0,0) not black.");
        Assert.True(texture2D.GetPixel(2, 2).Equals(Color.white), "Test heightmap (2,2) not white.");
    }

}
