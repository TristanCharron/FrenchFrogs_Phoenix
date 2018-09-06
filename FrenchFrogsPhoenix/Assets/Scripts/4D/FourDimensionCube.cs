using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDimensionCube : Rotation4D
{
    LineGroup[] lineGroups;
    int dimension = 4;

    void Start()
    {
        Calculate();
        lineGroups = GetComponentsInChildren<LineGroup>();
    }

    void Update()
    {
        if (verticesMatrixPosition != null)
        {
            base.Rotate();
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        for (int i = 0; i < lineGroups.Length; i++)
        {
            lineGroups[i].ApplyLineToGroup(verticesPositionsAfterRotation);
        }
    }

    protected override void Calculate()
    {
        verticesMatrixPosition = MatrixVerticeGenerator.GenerateCubeVertices(dimension);
    }    
}