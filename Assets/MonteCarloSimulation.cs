using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex32;

public class MonteCarloSimulation : MonoBehaviour
{
    private Vector3 optimalTranslation;
    private Quaternion optimalRotation;

    public int registrationIterations = 100000;
    public int monteCarloIterations = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RunSimulation()
    {
        
    }
    private Matrix4x4 CalculateOptimalTransformation(Vector3[] pluginPoints, Vector3[] opticalPoints)
    {
        // optimal Translation between two point clouds calculated
        // as the distance between their centroids
        Vector3 opticalCentroid = GetCentroid(opticalPoints);
        Vector3 pluginCentroid = GetCentroid(pluginPoints);
        Vector3 optimalTranslation = pluginCentroid - opticalCentroid;
        //Quaternion optimalRotation;
        
        // Mean center the points 
        for(int i = 0; i < opticalPoints.Length; i++)
        {
            opticalPoints[i] = opticalPoints[i] - opticalCentroid;
            pluginPoints[i] = pluginPoints[i] - pluginCentroid;
        }

        // Calculate covariance matrix
        Vector3[] covarianceMatrix = {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0) };
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                for(int k = 0; k < opticalPoints.Length; k++)
                {
                    covarianceMatrix[i][j] += opticalPoints[k][i] * pluginPoints[k][j];
                }
            }
        }
        

        // Calculate polar decomposition of that matrix
        for(int i = 0; i < this.registrationIterations; i++)
        {
            
        }
        return Matrix4x4.identity;
        
    }

    // Take in a point cloud and returns the centroid
    Vector3 GetCentroid(Vector3[] points)
    {
        Vector3 centroid = Vector3.zero;
        foreach(Vector3 point in points)
        {
            centroid += point;
        }
        centroid /= points.Length;
        return centroid;
    }



    //Matrix4x4 CalculateRotationMatrix(Vector3[] source, Vector3[] target)
    //{
    //    // Convert Vector3 arrays to MathNet.Numerics matrices
    //    Matrix<double> sourceMatrix = DenseMatrix.OfColumnArrays(source.Select(v => new[] { v.x, v.y, v.z }));
    //    Matrix<double> targetMatrix = DenseMatrix.OfColumnArrays(target.Select(v => new[] { v.x, v.y, v.z }));

    //    // Perform Singular Value Decomposition
    //    var svd = sourceMatrix.Svd(true);

    //    // Calculate rotation matrix using U, Vt matrices from SVD
    //    Matrix<double> rotationMatrix = svd.U * svd.VT;

    //    // Convert rotationMatrix to Unity's Matrix4x4 format
    //    Matrix4x4 unityRotationMatrix = new Matrix4x4();
    //    for (int i = 0; i < 3; i++)
    //    {
    //        for (int j = 0; j < 3; j++)
    //        {
    //            unityRotationMatrix[i, j] = (float)rotationMatrix[i, j];
    //        }
    //    }
    //    unityRotationMatrix.m33 = 1f;

    //    return unityRotationMatrix;
    //}

    
}
