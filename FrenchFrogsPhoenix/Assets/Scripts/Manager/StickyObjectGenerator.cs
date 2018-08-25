using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObjectGenerator : MonoBehaviour {

    [Header("Spawn params")]
    [SerializeField] float radiusSpawn = 25;
    [SerializeField] float timerSpawn = 5;

    [Header("StickingObjects params")]
    [SerializeField] StickingObject prefabStickingObjet;
    [SerializeField] float spinMagnitude;
    [SerializeField] float vectorMagnitude;

    float currentTimerSpawn = 0;

	void Update ()
    {
        currentTimerSpawn += Time.deltaTime;
        if (currentTimerSpawn > timerSpawn)
        {
            currentTimerSpawn = 0;
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        StickingObject stickingObject = Instantiate(prefabStickingObjet, Random.insideUnitSphere * radiusSpawn, Quaternion.identity);

        Rigidbody rigidBody = stickingObject.rb; // GetComponent<Rigidbody>();
        rigidBody.velocity = (Random.insideUnitSphere * vectorMagnitude);
        rigidBody.angularVelocity = (Random.insideUnitSphere * spinMagnitude);
    }
}
