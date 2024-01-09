using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller class
// Ensures gameobject functions are called in the 
// correct order with appropriate parameters being passed in

public class SimulationController : MonoBehaviour
{
    public GameObject HololensToDepth;

    public DisplayPoints displayPoints;
    public PartitionPoints partitionPoints;
    public MonteCarloSimulation monteCarloSimulation;

    // Start is called before the first frame update
    void Start()
    {


        displayPoints.ParseCSV();
        partitionPoints.Initialize(displayPoints.masterList, displayPoints.maxPoints, displayPoints.minPoints);
        partitionPoints.Partition();
        partitionPoints.DisplayColouredPartitions();
        //Matrix4x4 matrix = kabschAlgorithm.CalculateTransformationMatrix(displayPoints.masterList);
        //HololensToDepth.transform.SetPositionAndRotation(MatrixExtensions.ExtractPosn(matrix), MatrixExtensions.ExtractRotation(matrix));
        //monteCarloSimulation.RunSimulationRandom(partitionPoints.masterList, partitionPoints.partitionedList[4]);
        System.DateTime before = System.DateTime.Now;
        Debug.Log(before.ToString());
        monteCarloSimulation.RunSimulationPartitioned(partitionPoints.partitionedList, partitionPoints.partitionedList[4]);
        System.DateTime after = System.DateTime.Now;
        Debug.Log(after.ToString());
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Duration in seconds: " + duration.Milliseconds/1000.0f);

        //Debug.Log(partitionPoints.partitionedList.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        //Matrix4x4 matrix = kabschAlgorithm.CalculateTransformationMatrix(displayPoints.masterList);
        //HololensToDepth.transform.SetPositionAndRotation(MatrixExtensions.ExtractPosition(matrix), MatrixExtensions.ExtractRotation(matrix));
    }
}
