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
        //HololensToDepth.transform.SetPositionAndRotation(MatrixExtensions.ExtractPosition(matrix), MatrixExtensions.ExtractRotation(matrix));
        //monteCarloSimulation.RunSimulationRandom(partitionPoints.masterList, partitionPoints.partitionedList[4]);
        monteCarloSimulation.RunSimulationPartitioned(partitionPoints.partitionedList, partitionPoints.partitionedList[14]);

        //Debug.Log(partitionPoints.partitionedList.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        //Matrix4x4 matrix = kabschAlgorithm.CalculateTransformationMatrix(displayPoints.masterList);
        //HololensToDepth.transform.SetPositionAndRotation(MatrixExtensions.ExtractPosition(matrix), MatrixExtensions.ExtractRotation(matrix));
    }
}
