using UnityEngine;
using System.Collections;

using ProtoTurtle.BitmapDrawing;

public class drawBitmap : MonoBehaviour {

    void Start()
    {

        //Material material = GetComponent<Renderer>().material;
        //Texture2D texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        //texture.wrapMode = TextureWrapMode.Clamp;
        //material.SetTexture(0, texture);

        //texture.DrawFilledRectangle(new Rect(0, 0, 120, 120), Color.green);

        //texture.DrawRectangle(new Rect(0, 0, 120, 60), Color.red);

        //texture.DrawCircle(256, 256, 100, Color.cyan);
        //texture.DrawFilledCircle(256, 256, 50, Color.grey);

        //texture.DrawCircle(0, 0, 512, Color.red);

        //texture.DrawLine(new Vector2(120, 60), new Vector2(256, 256), Color.black);

        //texture.Apply();
    }

    public void draw(byte[,] bitmap)
    {
        Material material = GetComponent<Renderer>().material;
        Texture2D texture = new Texture2D(128, 128, TextureFormat.RGB24, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(0, texture);

        for (int i = 0; i < 128; i++)
        {
            //float _bitValue = bitmap[(int)i + 1, (int)1];
            //print("(" + _bitValue + "," + _bitValue / 256f + ")");
            for (int j = 0; j < 128; j++)
            {
                float bitValue = bitmap[Mathf.FloorToInt(i + 1), Mathf.FloorToInt(j + 1)];
                
                //print("(" + bitValue + "," + bitValue / 256f + ")");
                texture.DrawFilledRectangle(new Rect(j, 127 - i, 1, 1), new Color(1f - bitValue / 256f, 1f-bitValue / 256f, 1f-bitValue / 256f));
            }
        }
        texture.Apply();
    }

    public void draw(int[,] bitmap)
    {
        Material material = GetComponent<Renderer>().material;
        Texture2D texture = new Texture2D(128, 128, TextureFormat.RGB24, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        material.SetTexture(0, texture);

        for (int i = 0; i < 128; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                int bitValue = bitmap[(int)i+1,(int)j+1];
                texture.DrawFilledRectangle(new Rect(i, j, i + 1, j + 1), new Color(bitValue / 256f, bitValue / 256f, bitValue / 256f));
            }
        }
    }
}
