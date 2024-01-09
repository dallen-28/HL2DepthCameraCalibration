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


        // All quadrants except TRE
        //calibrationQuadrantIndeces = new List<int>() {0,1,2,3,5,6,7,8,9,10,11,12,14,15,16,17 };

        // Configuration from notebook 
        //calibrationQuadrantIndeces = new List<int>() {8,3,1,16,9,14,15,2,11,6,10,7,17,0,12,5,13 };
        calibrationQuadrantIndeces = new List<int>() { 0,0,0,0,0,0 };


        // 6 fiducial configuration for paper
        //calibrationQuadrantIndeces = new List<int>() { 0,1,2,9,10,11 };
        // Mean tre = 2.5480

        //calibrationQuadrantIndeces = new List<int>() { 3,6,7,12,15,16 };
        // Mean TRE = 2.2008

        //calibrationQuadrantIndeces = new List<int>() { 0,7,2,15,10,17 };
        // Mean TRE = 2.0168

        //calibrationQuadrantIndeces = new List<int>() {3,6,7,10,11,14 };
        // Mean TRE = 1.9293

        //calibrationQuadrantIndeces = new List<int>() { 3,1,8,14,16,9 };
        // Mean TRE = 1.7757

        //calibrationQuadrantIndeces = new List<int>() { 3, 7, 5, 12, 16, 14 };
        // Mean TRE = 1.6583

        //calibrationQuadrantIndeces = new List<int>() { 12,17,14,16,9,11 };
        // Mean TRE = 1.6546


        // Different configurations of 6 points
        //calibrationQuadrantIndeces = new List<int>() {6,1,8,9,16,11 };
        // Mean TRE = 1.89 mm 

        //calibrationQuadrantIndeces = new List<int>() {3,1,5,12,16,14 };
        // Mean TRE = 1.7033

        //calibrationQuadrantIndeces = new List<int>() {3,7,5,12,10,14 };
        // Mean TRE = 1.7453

        //calibrationQuadrantIndeces = new List<int>() {3,1,5,12,10,14 };
        // Mean TRE =  1.8211

        //calibrationQuadrantIndeces = new List<int>() { 3, 7, 5, 12, 10, 14 };
        // Mean TRE =1.7521

        // Winner
        //calibrationQuadrantIndeces = new List<int>() { 3, 7, 5, 12, 16, 14 };
        // Mean TRE =1.6583

        //calibrationQuadrantIndeces = new List<int>() { 3, 7, 5, 12, 16, 14, 6, 8, 15, 17, 0, 2, 9, 11, 1, 10, 13 };
        // Mean TRE =1.6583


        //calibrationQuadrantIndeces = new List<int>() { 0,7,2,15,10,17 };
        // Mean TRE = 2.0168
        //calibrationQuadrantIndeces = new List<int>() { 3,1,8,14,16,9 };
        // Mean TRE = 1.7757

        // Only 1st plane
        //calibrationQuadrantIndeces = new List<int>() {6,0,7,5,2,1  };
        // Mean TRE = 1.8984 mm 

        //calibrationQuadrantIndeces = new List<int>() { 3,8,5,7,0,2};
        // Mean TRE = 1.9569

        // Only second plane 
        //calibrationQuadrantIndeces = new List<int>() { 15,9,16,14,11,10 };
        // Mean TRE = 1.7472

        //calibrationQuadrantIndeces = new List<int>() { 12,17,14,16,9,11 };
        // Mean TRE = 1.6546


        // Lower left corner
        //calibrationQuadrantIndeces = new List<int>() { 3,1,8,16,9,14 };
        // Mean TRE = 1.7762

        // Upper left corner
        // calibrationQuadrantIndeces = new List<int>() { 3,6,7,12,15,16 };
        // Mean TRE = 2.2008

        // Upper Right corner  
        //calibrationQuadrantIndeces = new List<int>() { 7,8,5,16,17,14 };
        // MEan TRE = 1.9487

        // Bottom Row
        //calibrationQuadrantIndeces = new List<int>() { 0,1,2,9,10,11};
        // MEan TRE = 2.5516

        // Left Side
        //calibrationQuadrantIndeces = new List<int>() { 6,3,0,15,12,9 };
        // MEan TRE = 2.3938


        // Left Right 1
        //calibrationQuadrantIndeces = new List<int>() { 6, 3, 0, 17,14,11};
        // Mean tre = 1.9694

        // Left Right 2
        //calibrationQuadrantIndeces = new List<int>() {8,5,2,15,12,9};
        // Mean tre = 1.8504

        // Right side
        //calibrationQuadrantIndeces = new List<int>() { 8,5,2,17,14,11 };
        // Mean tre = 2.1433

        // Top
        //calibrationQuadrantIndeces = new List<int>() { 6,7,8,15,16,17 };
        // Mean tre = 2.5499

        // Bottom
        //calibrationQuadrantIndeces = new List<int>() { 0,1,2,9,10,11 };
        // Mean tre = 2.5480

        // TopBottom1
        //calibrationQuadrantIndeces = new List<int>() { 0,1,2,15,16,17 };
        // Mean TRE = 1.9197

        // TopBottom2
        //calibrationQuadrantIndeces = new List<int>() { 6,7,8,9,10,11 };
        // Mean TRE = 1.9689

        // Corners but better disperstion

        //calibrationQuadrantIndeces = new List<int>() { 3,0,1,16,17,14 };
        // Mean TRE = 1.8695

        //calibrationQuadrantIndeces = new List<int>() { 7,8,5,12,9,10 };
        // Mean TRE = 1.8510

        //calibrationQuadrantIndeces = new List<int>() {3,6,7,10,11,14 };
        // Mean TRE = 1.9293

        //calibrationQuadrantIndeces = new List<int>() { 1,2,5,12,15,16 };
        // Mean TRE = 1.7966


        // 8 points no corners
        //calibrationQuadrantIndeces = new List<int>() { 3,7,5,1,12,16,14,10 };
        // Mean TRE = 1.7129

        //calibrationQuadrantIndeces = new List<int>() { 13,13,13,13,13,13 };
        // Mean TRE = 3.54 mm 

        // Bad dispersion
        //calibrationQuadrantIndeces = new List<int>() { 3,3,3,3,3,3 };
        // Mean TRE = 10.5178

        //calibrationQuadrantIndeces = new List<int>() { 0,0,0,0,0,0 };
        // Mean TRE = 38.3910


        // 4 points
        //calibrationQuadrantIndeces = new List<int>() { 6,0,8,2};
        //calibrationQuadrantIndeces = new List<int>() { 3,5,12,14};

        // Mean TRE = 3.5
        //calibrationQuadrantIndeces = new List<int>() { 6,2,9,17 };
        // Mean TRE = 1.9926
        //calibrationQuadrantIndeces = new List<int>() { 0,8,15,11 };
        // Mean TRE = 2.2346

        //calibrationQuadrantIndeces = new List<int>() { 15,17,9,11 };
        // Mean TRE = 4.4799

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
                sourcePoints.Clear();
                targetPoints.Clear();

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
