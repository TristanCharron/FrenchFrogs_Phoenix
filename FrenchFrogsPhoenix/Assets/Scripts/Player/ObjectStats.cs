using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectStats {

    public enum Type { Mass, Energy, Power};
    public float mass;
    public float energy;
    public float power;

    public void SetType(Type type)
    {
        switch(type)
        {
            case Type.Mass:
                mass = GetStats(true);
                energy = GetStats(false);
                power = GetStats(false);
                break;
            case Type.Energy:
                mass = GetStats(false);
                energy = GetStats(true);
                power = GetStats(false);
                break;
            case Type.Power:
                mass = GetStats(false);
                energy = GetStats(false);
                power = GetStats(true);
                break;
        }
    }

    float GetStats(bool strong)
    {
        if(strong)
            return Random.Range(1, 3);
        else
            return Random.Range(0, .5f);
    }

    public ObjectStats()
    {
        Reset();
    }

    public void Reset()
    {
        mass = 0;
        energy = 0;
        power = 0;
    }

    public static ObjectStats operator +(ObjectStats statsThis, ObjectStats statsAdditional)
    {
        statsThis.mass += statsAdditional.mass;
        statsThis.energy += statsAdditional.energy;
        statsThis.power += statsAdditional.power;

        return statsThis;
    }

    public static ObjectStats operator *(ObjectStats statsThis, float multiplier)
    {
        statsThis.mass *= multiplier;
        statsThis.energy *= multiplier;
        statsThis.power *= multiplier;

        return statsThis;
    }
}
