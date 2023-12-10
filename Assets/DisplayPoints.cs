using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Read in plugin points from csv file and display in unity scene
public class DisplayPoints : MonoBehaviour
{
    public string pluginCSVFileName;
    public string opticalCSVFileName;
    public GameObject HololensToDepth;

    public GameObject pluginSpherePrefab;
    public GameObject opticalSpherePrefab;

    public List<CalibrationPointPair> masterList;
    public Vector3 maxPoints;
    public Vector3 minPoints;

    private float maxX, minX, maxY, minY, maxZ, minZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ParseCSV()
    {
        maxX = maxY = maxZ = 0;
        minX = minY = 0;
        minZ = 1;

        masterList = new List<CalibrationPointPair>();

        string dataPath = Application.dataPath;
        var textFile = Resources.Load<TextAsset>(pluginCSVFileName);
        var opticalTextFile = Resources.Load<TextAsset>(opticalCSVFileName);
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
            float xPos = float.Parse(pluginRow[0]) / 1000.0f;
            float yPos = float.Parse(pluginRow[1]) / 1000.0f;
            float zPos = float.Parse(pluginRow[2]) / 1000.0f;

            // Calculate point cloud boundary
            if(xPos > maxX)
            {
                maxX = xPos;
            }
            else if(xPos < minX)
            {
                minX = xPos;
            }
            if (yPos > maxY)
            {
                maxY = yPos;
            }
            else if (yPos < minY)
            {
                minY = yPos;
            }
            if (zPos > maxZ)
            {
                maxZ = zPos;
            }
            else if (zPos < minZ)
            {
                minZ = zPos;
            }

            Vector3 pluginPos = new Vector3(xPos, yPos, zPos);
            Vector3 opticalPos = new Vector3(float.Parse(opticalRow[0]) / 1000.0f, float.Parse(opticalRow[1]) / 1000.0f, float.Parse(opticalRow[2]) / 1000.0f);



            
            GameObject pluginSphere = Instantiate(pluginSpherePrefab);
            GameObject opticalSphere = Instantiate(opticalSpherePrefab);
            CalibrationPointPair pointPair = new CalibrationPointPair(opticalPos, pluginPos, pluginSphere, opticalSphere);
            masterList.Add(pointPair);


            //opticalSphere.transform.parent = HololensToDepth.transform;
            //opticalSphere.transform.localPosition = opticalPos;
        
        }

        maxPoints = new Vector3(maxX, maxY, maxZ);
        minPoints = new Vector3(minX, minY, minZ);

    }
}
public class CalibrationPointPair
{
    public Vector3 pluginPoint;
    public Vector3 opticalPoint;
    public GameObject pluginSpherePrefab;
    public GameObject opticalSpherePrefab;

    public CalibrationPointPair(Vector3 opticalPoint, Vector3 pluginPoint, GameObject pluginSpherePrefab, GameObject opticalSpherePrefab)
    {
        this.pluginPoint = pluginPoint;
        this.opticalPoint = opticalPoint;
        this.pluginSpherePrefab = pluginSpherePrefab;
        this.opticalSpherePrefab = opticalSpherePrefab;
        this.pluginSpherePrefab.transform.localPosition = pluginPoint;
        this.opticalSpherePrefab.transform.localPosition = opticalPoint;       
    }
}
