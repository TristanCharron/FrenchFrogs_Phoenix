using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinRotation : MonoBehaviour
{
    [SerializeField] float speed = .1f;
    [SerializeField] float shakeFactor = 0.1f;

    bool isActivated = false;

    Vector3 start;
    Vector3 end;
    float sinTimer = 0;

    public void Initialize()
    {
        isActivated = true;
    
        start = transform.localEulerAngles;
        end = Random.onUnitSphere * 360;
    }

    private void Update()
    {
        if (!isActivated)
            return;

        sinTimer += Time.deltaTime * speed;
        float sinT = ((Mathf.Sin(sinTimer) + 1) * 0.5f) * shakeFactor;
        transform.localEulerAngles = Vector3.Lerp(start, end, sinT);
    }
}
