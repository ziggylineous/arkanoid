using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class BlockTextureGenerator
{
    private static void InitPaint(Texture2D tex) {
        for (int r = 0; r != tex.height; ++r)
            for (int c = 0; c != tex.width; ++c)
                tex.SetPixel(c, r, Color.white);
    }

    private static List<Vector2Int> DisappearPositionsShuffled(int framePixelSize) {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int r = 0; r != framePixelSize; ++r)
        {
            for (int c = 0; c != framePixelSize; ++c)
            {
                positions.Add(new Vector2Int(c, r));
            }
        }

        ListExtensions.Shuffle<Vector2Int>(positions);

        return positions;
    }

    [MenuItem("Assets/Blocks/Texture/UnbreakeableOut")]
    public static void GenUnbreakeableOutAnim() {
		int frameCount = 30;
        int framePixelSize = 16;
        int imageCols = 8;
        int imageRows = frameCount / imageCols;

        Texture2D tex = new Texture2D(imageCols * framePixelSize, imageRows * framePixelSize);

        InitPaint(tex);

        List<Vector2Int> positions = DisappearPositionsShuffled(framePixelSize);


        int outPositionPerFrame = (framePixelSize * framePixelSize) / frameCount;

        for (int i = 0; i != frameCount; ++i) {
            int outCount = (i + 1) * outPositionPerFrame;

            Vector2Int frameStart = new Vector2Int(
                framePixelSize * (i % imageCols),
                framePixelSize * (i / imageCols)
            );

            for (int j = 0; j != outCount; ++j) {
                Vector2Int outPos = positions[j];
                tex.SetPixel(frameStart.x + outPos.x, frameStart.y + outPos.y, Color.clear);
            }
        }

		tex.Apply();

        byte[] pngData = tex.EncodeToPNG();

        if (pngData == null) {
            Debug.Log("no image data");
        } else {
            File.WriteAllBytes("Assets/Textures/unbreakeableBlockOut2.png", pngData);
        }


        AssetDatabase.Refresh();

    }
}
