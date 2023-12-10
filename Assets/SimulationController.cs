using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller class
// Ensures gameobject functions are called in the 
// correct order with appropriate parameters being passed in

public class SimulationController : MonoBehaviour
{
    public DisplayPoints displayPoints;
    public PartitionPoints partitionPoints;

    // Start is called before the first frame update
    void Start()
    {
        displayPoints.ParseCSV();
        partitionPoints.Initialize(displayPoints.masterList, displayPoints.maxPoints, displayPoints.minPoints);
        partitionPoints.Partition();
        partitionPoints.DisplayColouredPartitions();

        Debug.Log(partitionPoints.partitionedList.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
