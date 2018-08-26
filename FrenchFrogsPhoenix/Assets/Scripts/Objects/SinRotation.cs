using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinRotation : MonoBehaviour
{
    [SerializeField] float speed = .1f;
    [SerializeField] float shakeFactor = 0.1f;
    [SerializeField] float randomness = 10;

    bool isActivated = false;

    Vector3 start;
    Vector3 end;
    float sinTimer = 0;

    public void Initialize()
    {
        isActivated = true;
    
        start = transform.localEulerAngles;

        float remainRandom = randomness;
        Vector3 random = new Vector3(RandomScalar(ref remainRandom), RandomScalar(ref remainRandom), RandomScalar(ref remainRandom));

        end = start + random;
        //end = Random.onUnitSphere * 360;
    }

    float RandomScalar(ref float randomness)
    {
        float random = Random.Range(0, randomness);

        randomness -= random;
        return random * Mathf.Sign(Random.Range(-1,1));
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
