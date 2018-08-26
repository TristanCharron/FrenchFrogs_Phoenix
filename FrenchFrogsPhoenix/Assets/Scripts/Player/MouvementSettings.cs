using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MouvementSettings {

    ObjectStats stats;
    public float baseMoveSpeed = 5;
    public float baseAcceleration = 2;
    public float warpBoostMultiplyer = 3;
    [HideInInspector] public bool isWarpAcceleration = false;

    public float CalculateAcceleration()
    {
        return baseAcceleration * ((isWarpAcceleration) ? warpBoostMultiplyer : 1);
    }

    public float GetMaxSpeed()
    {
        if (isWarpAcceleration)
            return baseMoveSpeed * warpBoostMultiplyer;
        else
            return baseMoveSpeed;
    }
}
