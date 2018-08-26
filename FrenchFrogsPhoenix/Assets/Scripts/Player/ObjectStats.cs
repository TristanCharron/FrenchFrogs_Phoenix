using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectStats {
    //To complete
    public float speed;
    public float damage;

    public ObjectStats()
    {
        Reset();
    }

    public void Reset()
    {
        speed = 0;
        damage = 0;
    }

    public static ObjectStats operator +(ObjectStats statsThis, ObjectStats statsAdditional)
    {
        statsThis.speed += statsAdditional.speed;
        statsThis.damage += statsAdditional.damage;
        return statsThis;
    }

    public static ObjectStats operator *(ObjectStats statsThis, float multiplier)
    {
        statsThis.speed *= multiplier;
        statsThis.damage *= multiplier;
        return statsThis;
    }
}
