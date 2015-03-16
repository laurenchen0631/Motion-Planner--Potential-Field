using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using System.IO;

public class fileManagerScript : MonoBehaviour
{

    public string OpenFilePanel()
    {
        string path = EditorUtility.OpenFilePanel(
                    "Read configuration data",
                    "",
                    "dat");

        return path;
    }

    public void LoadRobot()
    {
        string fileName = OpenFilePanel();

        try
        {
            StreamReader sReader = new StreamReader(fileName, Encoding.Default);
            int numOfObjects = 1;
            int numofPolygon = 1;
            int numofVertices = 3;
            int numofControls = 2;
            using(sReader)
            {
                while (readToDataLine(sReader) != null) ;

                sReader.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private string readToDataLine(StreamReader sr)
    {
        string line;
        while ((line = sr.ReadLine()) != null && line[0] == '#')
        {
            Debug.Log(line);
        }
        Debug.Log(line);
        //int numVal = 0;
        //try
        //{
        //    numVal = System.Convert.ToInt32(line);
        //}
        //catch (System.FormatException e)
        //{
        //    Debug.Log("Input string is not a sequence of digits.");
        //}
        //catch (System.OverflowException e)
        //{
        //    Debug.Log("The number cannot fit in an Int32.");
        //}

        return line;
    }

    public void SaveFile()
    {
        
    }
}
