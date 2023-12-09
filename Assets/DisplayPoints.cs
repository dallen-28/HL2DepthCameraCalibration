using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Read in plugin points from csv file and display in unity scene
public class DisplayPoints : MonoBehaviour
{
    public string pluginCSVFileName;
    public string opticalCSVFileName;
    public GameObject HololensToDepth;

    private List<Vector3> pluginPoints;
    private List<Vector3> opticalPoints;
    public GameObject pluginSpherePrefab;
    public GameObject opticalSpherePrefab;

    // Start is called before the first frame update
    void Start()
    {
        pluginPoints = new List<Vector3>();
        opticalPoints = new List<Vector3>();
        ParseCSV();
    }

    private void ParseCSV()
    {
        string dataPath = Application.dataPath;
        //string streamingAssetsPath = Application.streamingAssetsPath;
        var textFile = Resources.Load<TextAsset>(pluginCSVFileName);
        var opticalTextFile = Resources.Load<TextAsset>(opticalCSVFileName);
        //Application.streamingAssetsPath("SingleClues.csv");
        //string filePath = Application.streamingAssetsPath + "/" + csvFileName;
        //string textFile = System.IO.File.ReadAllText(filePath);
        // Splitting the dataset in the end of line
        var pluginSplitDataset = textFile.text.Split(new char[] { '\n' });
        var opticalSplitDataset = opticalTextFile.text.Split(new char[] { '\n' });

        // Ensure each row has the correct amount of data
        for (var i = 0; i < pluginSplitDataset.Length; i++)
        {
            string[] pluginRow = pluginSplitDataset[i].Split(new char[] { ',' });
            string[] opticalRow = opticalSplitDataset[i].Split(new char[] { ',' });
            if (pluginRow.Length != 3 || opticalRow.Length != 3)
            {
                Debug.Log("Incorrect number of columns in row " + i + 1 + "!");
                Application.Quit();
            }
            

            Vector3 pluginPos = new Vector3(float.Parse(pluginRow[0]) / 1000.0f, float.Parse(pluginRow[1]) / 1000.0f, float.Parse(pluginRow[2]) / 1000.0f);
            //Vector3 pluginPos = new Vector3(float.Parse(row[0]) / 1000.0f, float.Parse(row[1]) / 1000.0f, float.Parse(row[2]) / 1000.0f);
            pluginPoints.Add(pluginPos);
            GameObject pluginSphere = Instantiate(pluginSpherePrefab);
            pluginSphere.transform.position = pluginPos;

            Vector3 opticalPos = new Vector3(float.Parse(opticalRow[0]) / 1000.0f, float.Parse(opticalRow[1]) / 1000.0f, float.Parse(opticalRow[2]) / 1000.0f);
            opticalPoints.Add(opticalPos);
            GameObject opticalSphere = Instantiate(opticalSpherePrefab);
            opticalSphere.transform.transform.position = opticalPos;

            opticalSphere.transform.parent = HololensToDepth.transform;
            opticalSphere.transform.localPosition = opticalPos;
         

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
