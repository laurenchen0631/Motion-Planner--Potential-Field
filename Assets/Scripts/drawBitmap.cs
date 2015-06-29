using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtoTurtle.BitmapDrawing;

public class drawBitmap : MonoBehaviour {

    public void draw(byte[,] bitmap)
    {
        Material material = GetComponent<Renderer>().material;
        Texture2D texture = new Texture2D(128, 128, TextureFormat.RGB24, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(0, texture);

        for (int i = 0; i < 128; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                float bitValue = bitmap[i + 1, j + 1];
                Color co = Color.white;
                if (bitValue < 64)
                    co = new Color(bitValue / 64f, 0, 0);
                else if (bitValue < 128)
                    co = new Color(0, bitValue / 128f, 0);
                else if (bitValue < 192)
                    co = new Color(0, 0, bitValue / 192f);
                else
                    co = new Color(1f - bitValue / 256f, 1f - bitValue / 256f, 1f - bitValue / 256f);
                
                //print("(" + bitValue + "," + bitValue / 256f + ")");
                //texture.DrawFilledRectangle(new Rect(j, 127 - i, 1, 1), new Color(1f - bitValue / 256f, 1f-bitValue / 256f, 1f-bitValue / 256f));
                texture.DrawFilledRectangle(new Rect(j, 127 - i, 1, 1), co);
            }
        }
        texture.Apply();
    }

    public void draw(int[,] bitmap, int max)
    {
        Material material = GetComponent<Renderer>().material;
        Texture2D texture = new Texture2D(128, 128, TextureFormat.RGB24, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(0, texture);

        for (int i = 0; i < 128; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                float bitValue = bitmap[i + 1, j + 1];
                texture.DrawFilledRectangle(new Rect(j, 127 - i, 1, 1), new Color(1f - bitValue / max, 1f - bitValue / max, 1f - bitValue / max));
            }
        }
        texture.Apply();
    }

    public void drawPath(List<Configuration> path)
    {
        Material material = GetComponent<Renderer>().material;
        Texture2D texture = new Texture2D(128, 128, TextureFormat.RGB24, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(0, texture);

        for (int i = 0; i < path.Count; i++)
        {
            texture.DrawFilledRectangle(new Rect(Mathf.FloorToInt(path[i].x), 127 - Mathf.FloorToInt(path[i].y), 1, 1), Color.red);
        }

        texture.Apply();
    }
}
