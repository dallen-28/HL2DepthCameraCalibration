using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionPoints : MonoBehaviour
{

    public List<CalibrationPointPair> masterList;
    public List<List<CalibrationPointPair>> partitionedList;
    public GameObject HololensToDepth;

    private float depthPartition;
    private float minXBoundary;
    private float maxXBoundary;
    private float leftXPartition;
    private float rightXPartition;
    private float minYBoundary;
    private float maxYBoundary;
    private float topYPartition;
    private float bottomYPartition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(List<CalibrationPointPair> masterListReference, Vector3 maxPositions, Vector3 minPositions)
    {
        // Get master list reference from display points object
        this.masterList = masterListReference;

        this.partitionedList = new List<List<CalibrationPointPair>>();

        // Modify line below to account for number of quadrants
        for(int i = 0; i < 18; i++)
        {
            List<CalibrationPointPair> quadrantList = new List<CalibrationPointPair>();
            this.partitionedList.Add(quadrantList);
        }


        depthPartition = (maxPositions.z - minPositions.z)/2.0f + minPositions.z;
        minXBoundary = minPositions.x;
        maxXBoundary = maxPositions.x;
        minYBoundary = minPositions.y;
        maxYBoundary = maxPositions.y;
        leftXPartition = (maxXBoundary - minXBoundary) / 3.0f + minXBoundary;
        rightXPartition = maxXBoundary - ((maxXBoundary - minXBoundary) / 3.0f);
        bottomYPartition = (maxYBoundary - minYBoundary) / 3.0f + minYBoundary;
        topYPartition = maxYBoundary - ((maxYBoundary - minYBoundary) / 3.0f);

    }
    public void Partition()
    {
        for(int i = 0; i < this.masterList.Count; i++)
        {
            CalculatePartition(this.masterList[i]);
        }
    }

    // Change colour of each sphere gameobject to correspond to quadrant
    public void DisplayColouredPartitions()
    {
        foreach (List<CalibrationPointPair> subList in partitionedList)
        {
            // Get new random colour
            float randomRed = Random.value;
            float randomGreen = Random.value;
            float randomBlue = Random.value;
            Color randomColor = new Color(randomRed, randomGreen, randomBlue);
            float randomRed2 = Random.value;
            float randomGreen2 = Random.value;
            float randomBlue2 = Random.value;
            Color randomColor2 = new Color(randomRed2, randomGreen2, randomBlue2);

            foreach (CalibrationPointPair pointPair in subList)
            {

                pointPair.pluginSpherePrefab.GetComponent<Renderer>().material.color = randomColor;
                //pointPair.pluginSpherePrefab.SetActive(false);
                //pointPair.opticalSpherePrefab.SetActive(false);
                pointPair.opticalSpherePrefab.GetComponent<Renderer>().material.color = randomColor2;
                pointPair.opticalSpherePrefab.transform.parent = HololensToDepth.transform;
                pointPair.opticalSpherePrefab.transform.localPosition = pointPair.opticalPoint;
            }
        }

        //// For testing
        //foreach (CalibrationPointPair pointPair in partitionedList[7])
        //{
        //    pointPair.pluginSpherePrefab.SetActive(true);
        //    pointPair.pluginSpherePrefab.GetComponent<Renderer>().material.color = Color.white;
        //}
    }

    // Takes in single point pair, calculates its partition
    // and adds to appropriate sub list
    public void CalculatePartition(CalibrationPointPair pointPair)
    {
        int row;
        int col;
        int plane;
        int quadrant;

        // Calculate Column partition
        if(pointPair.pluginPoint.x >= minXBoundary && pointPair.pluginPoint.x < leftXPartition)
        {
            col = 0;
        }
        else if(pointPair.pluginPoint.x >= leftXPartition && pointPair.pluginPoint.x < rightXPartition)
        {
            col = 1;
        }
        else
        {
            col = 2;
        }

        // Calculate Row Partition
        if (pointPair.pluginPoint.y >= minYBoundary && pointPair.pluginPoint.y < bottomYPartition)
        {
            row = 2;
        }
        else if (pointPair.pluginPoint.y >= bottomYPartition && pointPair.pluginPoint.y < topYPartition)
        {
            row = 1;
        }
        else
        {
            row = 0;
        }

        // Calculate Depth Partition
        if(pointPair.pluginPoint.z < depthPartition)
        {
            plane = 0;
        }
        else
        {
            plane = 1;
        }
        
        // Calculate quadrant and add to corresponding list
        quadrant = 9*plane + 3*row + col;


        this.partitionedList[quadrant].Add(pointPair);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
