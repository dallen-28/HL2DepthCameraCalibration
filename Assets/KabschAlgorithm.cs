using UnityEngine;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class KabschAlgorithm : MonoBehaviour
{
    public Matrix4x4 CalculateTransformationMatrix(List<Vector3> sourcePoints, List<Vector3> targetPoints)
    {
        if (sourcePoints.Count != targetPoints.Count || sourcePoints.Count < 3)
        {
            Debug.LogError("Invalid point sets for Kabsch algorithm.");
            return Matrix4x4.identity;
        }

        Matrix<double> sourceMatrix = ConvertToMatrix(sourcePoints);
        Matrix<double> targetMatrix = ConvertToMatrix(targetPoints);

        Vector3 sourceCentroid = CalculateCentroid(sourcePoints);
        Vector3 targetCentroid = CalculateCentroid(targetPoints);

        Matrix<double> centeredSource = CenterPointCloud(sourceMatrix, sourceCentroid);
        Matrix<double> centeredTarget = CenterPointCloud(targetMatrix, targetCentroid);

        Matrix<double> covarianceMatrix = centeredSource.TransposeThisAndMultiply(centeredTarget);

        Matrix<double> rotationMatrix;
        SVD(covarianceMatrix, out rotationMatrix);

        Matrix<double> transformationMatrix = Matrix<double>.Build.DenseIdentity(4, 4);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                transformationMatrix[i, j] = rotationMatrix[i, j];
            }
        }
        transformationMatrix = transformationMatrix.Transpose();
        //Vector<double> sourceCentroidVector = DenseVector.OfArray(new double[] { sourceCentroid.x, sourceCentroid.y, sourceCentroid.z });
        //Vector<double> targetCentroidVector = DenseVector.OfArray(new double[] { targetCentroid.x, targetCentroid.y, targetCentroid.z });

        //Vector<double> translation = targetCentroidVector - transformationMatrix.Multiply(sourceCentroidVector);


        Vector<double> sourceCentroidVector = DenseVector.OfArray(new double[] { sourceCentroid.x, sourceCentroid.y, sourceCentroid.z, 1.0 });
        Vector<double> targetCentroidVector = DenseVector.OfArray(new double[] { targetCentroid.x, targetCentroid.y, targetCentroid.z, 1.0 });

        Vector<double> translation = targetCentroidVector.Subtract(transformationMatrix.Multiply(sourceCentroidVector));


        transformationMatrix[0, 3] = translation.At(0);
        transformationMatrix[1, 3] = translation.At(1);
        transformationMatrix[2, 3] = translation.At(2);


        // Apply the transformation to the source point set if needed
        //Debug.Log("Transformation Matrix:\n" + transformationMatrix);
        return ConvertToUnityMatrix(transformationMatrix);

    }

    Matrix4x4 ConvertToUnityMatrix(Matrix<double> mathNetMatrix)
    {
        if (mathNetMatrix.RowCount != 4 || mathNetMatrix.ColumnCount != 4)
        {
            Debug.LogError("Invalid matrix dimensions. Expecting a 4x4 matrix.");
            return Matrix4x4.identity;
        }

        Matrix4x4 unityMatrix = new Matrix4x4();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                unityMatrix[i, j] = (float)mathNetMatrix[i, j];
            }
        }

        return unityMatrix;
    }

    Matrix<double> ConvertToMatrix(List<Vector3> points)
    {
        int numRows = points.Count;
        int numCols = 3; // Assuming 3D points (x, y, z)

        double[,] data = new double[numRows, numCols];

        for (int i = 0; i < numRows; i++)
        {
            data[i, 0] = points[i].x;
            data[i, 1] = points[i].y;
            data[i, 2] = points[i].z;
        }

        return Matrix<double>.Build.DenseOfArray(data);
    }

    Vector3 CalculateCentroid(List<Vector3> points)
    {
        Vector3 centroid = Vector3.zero;

        foreach (Vector3 point in points)
        {
            centroid += point;
        }

        centroid /= points.Count;

        return centroid;
    }

    Matrix<double> CenterPointCloud(Matrix<double> points, Vector3 centroid)
    {
        int numRows = points.RowCount;
        int numCols = points.ColumnCount;

        Matrix<double> centeredPoints = Matrix<double>.Build.Dense(numRows, numCols);

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                centeredPoints[i, j] = points[i, j] - centroid[j];
            }
        }

        return centeredPoints;
    }

    void SVD(Matrix<double> matrix, out Matrix<double> rotationMatrix)
    {
        // Perform Singular Value Decomposition (SVD) and extract rotation matrix
        var svd = matrix.Svd();
        rotationMatrix = svd.U * svd.VT;
    }
}
