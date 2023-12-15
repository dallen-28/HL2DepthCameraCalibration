using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex32;
using System.IO;

public class MonteCarloSimulation : MonoBehaviour
{
    public int monteCarloIterations = 500;
    private KabschAlgorithm kabschAlgorithm;
    private List<int> calibrationQuadrantIndeces;

    // Start is called before the first frame update
    void Start()
    {
        kabschAlgorithm = gameObject.AddComponent<KabschAlgorithm>();


        calibrationQuadrantIndeces = new List<int>() {7,9,12,16,2,14,1,17,6,10 };       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pass in the master List
    public void RunSimulationRandom(List<CalibrationPointPair> masterList, List<CalibrationPointPair> treQuadrant)
    {

        List<List<float>> treErrors = new List<List<float>>(); 
        List<Vector3> sourcePoints = new List<Vector3>();
        List<Vector3> targetPoints = new List<Vector3>();
        int randIndex;
        Matrix4x4 transformationMatrix;

        // Modify according to range of points you want to use for registration
        for (int numPoints = 6; numPoints < 25; numPoints++)
        {
            List<float> treLine = new List<float>();
            for (int i = 0; i < monteCarloIterations; i++)
            {
                for (int j = 0; j < numPoints; j++)
                {
                    randIndex = Random.Range(0,masterList.Count - 1);

                    // Convert to mm
                    sourcePoints.Add(masterList[randIndex].opticalPoint * 1000.0f);
                    targetPoints.Add(masterList[randIndex].pluginPoint * 1000.0f);
                }

                // Calculate Transformation matrix
                transformationMatrix = kabschAlgorithm.CalculateTransformationMatrix(sourcePoints, targetPoints);

                // Currently taking all the points in the TRE quadrant for errors
                // Could take half depending on its size
                float treError = 0;

                for(int k = 0; k < treQuadrant.Count; k++)
                {
                    Vector3 error = transformationMatrix.MultiplyPoint(treQuadrant[k].opticalPoint * 1000.0f) - (treQuadrant[k].pluginPoint * 1000.0f);
                    float err = Mathf.Abs(error.magnitude);
                    treError += err;
                }

                // Calculate averate TRE Error
                treError /= treQuadrant.Count;
                treLine.Add(treError);
            }

            // Now Add TRE Line to master list
            treErrors.Add(treLine);
        }

        // Now write to file
        WriteTREErrorsToFile(treErrors, "treRandomErrors.csv");

    }
    public void WriteTREErrorsToFile(List<List<float>> treErrors, string fileName)
    {
  
        StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/" + fileName);
        for(int i = 0; i < treErrors.Count; i++)
        {
            string line = null;
            for (int j = 0; j < treErrors[i].Count; j++)
            {
                line += treErrors[i][j].ToString() + " ";

            }           
            streamWriter.WriteLine(line);
        }
        streamWriter.Close();
    }

    // Pass in the partitionedList
    public void RunSimulationPartitioned(List<List<CalibrationPointPair>> partitionedList)
    {
        for(int numPoints = 6; numPoints < 16; numPoints++)
        {
            for(int i = 0; i < monteCarloIterations; i++)
            {
                for(int j = 0; j < numPoints; j++)
                {
                    //randIndex = Random.RandomRange(0,tre)
                    
                }
            }
        }
    }
   
}
