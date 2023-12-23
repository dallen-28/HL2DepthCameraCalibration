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
    public int startingNumPoints = 6;
    public int endingNumPoints = 16;

    // For testing
    public GameObject HololensToDepth;

    private KabschAlgorithm kabschAlgorithm;
    private List<int> calibrationQuadrantIndeces;

    // Start is called before the first frame update
    void Awake()
    {
        kabschAlgorithm = gameObject.AddComponent<KabschAlgorithm>();

        // NEed to make sure we don't use same point pairs for TRE calculation as we are for calibration
        //calibrationQuadrantIndeces = new List<int>() {7,9,12,2,14,1,17,6,10 };

        // Separate graphs for each configuration
        //Tre = [, 5, 12, 14, 1]
        //calibrationQuadrantIndeces = new List<int>() { 0,2,7,15,10,17,6,9,8,11,3,16};
        //calibrationQuadrantIndeces = new List<int>() { 7, 9, 12, 2, 14, 1, 17, 6, 10 };
        //calibrationQuadrantIndeces = new List<int>() { 7, 9, 12, 2, 14, 1, 17, 6, 10 };

        // Different configurations of 6 points
        //calibrationQuadrantIndeces = new List<int>() {6,1,8,9,16,11 };
        //calibrationQuadrantIndeces = new List<int>() { 0,7,2,15,10,17 };
        //calibrationQuadrantIndeces = new List<int>() { 3,1,8,14,16,9 };

        // Only 1st plane
        //calibrationQuadrantIndeces = new List<int>() {6,0,7,5,2,1  };
        //calibrationQuadrantIndeces = new List<int>() { 3,8,5,7,0,2};

        // Only second plane 
        //calibrationQuadrantIndeces = new List<int>() { 15,9,16,14,11,10 };
        //calibrationQuadrantIndeces = new List<int>() { 12,17,14,16,9,11 };

        // All quadrants except TRE
        //calibrationQuadrantIndeces = new List<int>() {0,1,2,3,5,6,7,8,9,10,11,12,14,15,16,17 };

        // Configuration from notebook 
        calibrationQuadrantIndeces = new List<int>() {8,3,1,16,9,14,15,2,11,6,10,7,17,0,12,5,13 };

        // 4 points
        //calibrationQuadrantIndeces = new List<int>() { 6,0,8,2};
        //calibrationQuadrantIndeces = new List<int>() { 6,2,9,17 };
        //calibrationQuadrantIndeces = new List<int>() { 0,8,15,11 };
        //calibrationQuadrantIndeces = new List<int>() { 15,17,9,11 };

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
        for (int numPoints = startingNumPoints; numPoints <= endingNumPoints; numPoints++)
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

                // TODO: only apply transformation with smallest overall TRE
                //HololensToDepth.transform.SetPositionAndRotation(MatrixExtensions.ExtractPosition(transformationMatrix), MatrixExtensions.ExtractRotation(transformationMatrix));
                // Currently taking all the points in the TRE quadrant for errors
                // Could take half depending on its size
                float treError = 0;

                for (int k = 0; k < treQuadrant.Count; k++)
                {
                    Vector3 error = transformationMatrix.MultiplyPoint(treQuadrant[k].opticalPoint * 1000.0f) - (treQuadrant[k].pluginPoint * 1000.0f);
                    float err = Mathf.Abs(error.magnitude);
                    treError += err;
                }
                //int treRandIndex = Random.Range(0,treQuadrant.Count - 1);
                //Vector3 error = transformationMatrix.MultiplyPoint(treQuadrant[treRandIndex].opticalPoint * 1000.0f) - (treQuadrant[treRandIndex].pluginPoint * 1000.0f);
                //float err = Mathf.Abs(error.magnitude);
                //treError += err;

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

    // Pass in the partitionedList
    public void RunSimulationPartitioned(List<List<CalibrationPointPair>> partitionedList, List<CalibrationPointPair> treQuadrant)
    {
        List<List<float>> treErrors = new List<List<float>>();
        List<Vector3> sourcePoints = new List<Vector3>();
        List<Vector3> targetPoints = new List<Vector3>();
        int randIndex;
        Matrix4x4 transformationMatrix;

        // Modify according to range of points you want to use for registration
        for (int numPoints = startingNumPoints; numPoints <= endingNumPoints; numPoints++)
        {
            

            List<float> treLine = new List<float>();
            for (int i = 0; i < monteCarloIterations; i++)
            {
                sourcePoints.Clear();
                targetPoints.Clear();

                for (int j = 0; j < numPoints; j++)
                {
                    // Pick a random number corresponding to the count in each quadrant
                    // Ex: indexList: {9,12,16,2,14,1,17,6,10};  
                    randIndex = Random.Range(0, partitionedList[calibrationQuadrantIndeces[j]].Count - 1);

                    // Convert to mm
                    sourcePoints.Add(partitionedList[calibrationQuadrantIndeces[j]][randIndex].opticalPoint * 1000.0f);
                    targetPoints.Add(partitionedList[calibrationQuadrantIndeces[j]][randIndex].pluginPoint * 1000.0f);
                }

                // Calculate Transformation matrix
                transformationMatrix = kabschAlgorithm.CalculateTransformationMatrix(sourcePoints, targetPoints);

                // Currently taking all the points in the TRE quadrant for errors
                // Could take half depending on its size
                float treError = 0;

                for (int k = 0; k < treQuadrant.Count; k++)
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
        WriteTREErrorsToFile(treErrors, "trePartitionedErrors.csv");
    }

    public void WriteTREErrorsToFile(List<List<float>> treErrors, string fileName)
    {

        StreamWriter streamWriter = new StreamWriter(Application.streamingAssetsPath + "/" + fileName);
        for (int i = 0; i < treErrors.Count; i++)
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

}
