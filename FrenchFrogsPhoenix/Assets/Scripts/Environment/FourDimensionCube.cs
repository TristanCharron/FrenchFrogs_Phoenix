using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDimensionCube : MonoBehaviour {

    [SerializeField] GameObject prefabVertex;

    LineGroup[] lineGroups;
    Vector3[] verticesPositions = new Vector3[0];

    [SerializeField] float scaleByW;
    [SerializeField] float size = 1;
    [SerializeField] float rotationSpeed = 1;

    [SerializeField] bool rotateXY;
    [SerializeField] bool rotateXZ;
    [SerializeField] bool rotateXW;

    [SerializeField] bool rotateYZ;
    [SerializeField] bool rotateYW;

    [SerializeField] bool rotateZW;
    [SerializeField] bool randomXDLOL;

    Matrix verticesMatrixPosition;
    int dimension = 4;
    float angle = 0;


    void Start()
    {
        CalculateVertices();
        lineGroups = GetComponentsInChildren<LineGroup>();
    }

    void Update()
    {
        if (verticesMatrixPosition != null)
        {
            Matrix matrixNewPosition = ApplyRotations(verticesMatrixPosition, angle);
            UpdatePosition(matrixNewPosition);

            angle += Time.deltaTime * rotationSpeed;
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        for (int i = 0; i < lineGroups.Length; i++)
        {
            lineGroups[i].ApplyLineToGroup(verticesPositions);
        }
    }
   
    Matrix ApplyRotations(Matrix positionMatrix, float angle)
    {
        Matrix matrixNewPosition = verticesMatrixPosition;
        if(rotateXY)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RotationXY(angle));
        if (rotateXZ)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RotationXZ(angle));
        if (rotateXW)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RotationXW(angle));

        if (rotateYZ)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RotationYZ(angle));
        if (rotateYW)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RotationYW(angle));

        if (rotateZW)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RotationZW(angle));

        if (randomXDLOL)
            matrixNewPosition = Matrix.StupidMultiply(matrixNewPosition, MatrixRef.RandomXd(angle));

        return matrixNewPosition;
    }

    [ContextMenu("Calculate Vertices")]
    void CalculateVertices()
    {
        Matrix verticeMatrix = MatrixVerticeGenerator.GenerateVertices(dimension);

        verticesMatrixPosition = verticeMatrix;
        Debug.Log(verticeMatrix.ToString());

        GenerateVertices(verticeMatrix);
    }

    void GenerateVertices(Matrix verticeMatrix)
    {
        verticesPositions = new Vector3[verticeMatrix.rows];

        for (int i = 0; i < verticesPositions.Length; i++)
        {
            Vector3 position = new Vector3(
                (float)verticeMatrix[i,0],
                (float)verticeMatrix[i,1],
                (float)verticeMatrix[i,2]);

            verticesPositions[i] = position;
        }
    }

    void UpdatePosition(Matrix matrixPosition)
    {
        for (int i = 0; i < verticesPositions.Length; i++)
        {
            float w = 1 / (scaleByW - (float)matrixPosition[i, 3]);

            Vector3 position = new Vector3(
                (float)matrixPosition[i, 0],
                (float)matrixPosition[i, 1],
                (float)matrixPosition[i, 2]);

            verticesPositions[i] = position * w * size;
        }
    }
}

public static class MatrixVerticeGenerator
{
    public static Matrix GenerateVertices(int dimension)
    {
        List<List<float>> verticesList = new List<List<float>>();
        RecursiveCalculateVertices(verticesList, new List<float>(), dimension);
        return VerticesListToMatrix(verticesList);
    }

    static void RecursiveCalculateVertices(List<List<float>> verticesList, List<float> vertices, int dimensionLeft)
    {
        if (dimensionLeft == 0)
        {
            verticesList.Add(vertices);
            return;
        }

        dimensionLeft--;

        List<float> vertices1 = new List<float>(vertices);
        List<float> vertices2 = new List<float>(vertices);

        //vertices1.Add(size);
        //vertices2.Add(-size);

        vertices1.Insert(0, .5f);
        vertices2.Insert(0, -.5f);

        RecursiveCalculateVertices(verticesList, vertices1, dimensionLeft);
        RecursiveCalculateVertices(verticesList, vertices2, dimensionLeft);
    }


    static Matrix VerticesListToMatrix(List<List<float>> verticesList)
    {
        Matrix verticesMatrix = new Matrix(verticesList.Count, verticesList[0].Count);

        for (int i = 0; i < verticesList.Count; i++)
        {
            for (int j = 0; j < verticesList[i].Count; j++)
            {
                verticesMatrix[i, j] = verticesList[i][j];
            }
        }
        return verticesMatrix;
    }
}

public static class MatrixRef
{
    public static Matrix RandomXd(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { sin, -cos, 0, 0 },
                    { cos,  cos,  0, 0 },
                    { 0  , sin  ,  1, 0 },
                    { 0  , 0   ,  0, sin }
            }
        );
    }

    public static Matrix RotationXY(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, -sin, 0, 0 },
                    { sin,  cos,  0, 0 },
                    { 0  , 0   ,  1, 0 },
                    { 0  , 0   ,  0, 1 }
            }
        );
    }

    public static Matrix RotationXZ(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, 0, -sin, 0 },
                    { 0  , 1,  0  , 0 },
                    { sin, 0,  cos, 0 },
                    { 0  , 0,  0  , 1 }
            }
        );
    }

    public static Matrix RotationXW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { cos, 0, 0, -sin },
                    { 0  , 1, 0, 0 },
                    { 0  , 0, 1, 0 },
                    { sin, 0, 0, cos}
            }
        );
    }

    public static Matrix RotationYZ(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0   ,    0, 0 },
                    { 0, cos , -sin, 0 },
                    { 0, sin ,  cos, 0 },
                    { 0, 0   ,    0, 1 }
            }
        );
    }

    public static Matrix RotationYW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0   ,0, 0 },
                    { 0, cos , 0, -sin },
                    { 0, 0 ,  1, 0 },
                    { 0, sin,0, cos }
            }
        );
    }

    public static Matrix RotationZW(float angle)
    {
        double cos = Mathf.Cos(angle);
        double sin = Mathf.Sin(angle);
        return new Matrix
        (
            new double[,]
            {
                    { 1, 0, 0  , 0 },
                    { 0, 1, 0  , 0 },
                    { 0, 0, cos, -sin },
                    { 0, 0, sin, cos}
            }
        );
    }
}