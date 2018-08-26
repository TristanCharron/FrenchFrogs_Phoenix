using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObjectFactory : MonoBehaviour {

    [Header("Spawn params")]
    [SerializeField] float radiusSpawn = 25;
    [SerializeField] float timerSpawn = 5;

    [Header("StickingObjects params")]
    [SerializeField] StickingObject prefabStickingObjet;
    [SerializeField] float spinMagnitude;
    [SerializeField] float vectorMagnitude;

    [SerializeField] MeshRenderer[] meshPrefabs;
    [SerializeField] Material[] materials;

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

        stickingObject.transform.SetParent(transform);
            
        Rigidbody rigidBody = stickingObject.rb;
        rigidBody.velocity = (Random.insideUnitSphere * vectorMagnitude);
        rigidBody.angularVelocity = (Random.insideUnitSphere * spinMagnitude);

        Material material = materials[Random.Range(0, materials.Length)];
        MeshRenderer mesh = SetMeshChild(stickingObject, material);

        float randomSize = Random.Range(.1f, 2f);
        ObjectStats stats = new ObjectStats();

        stats *= randomSize;
        stickingObject.SetObjectStats(stats);

        stickingObject.transform.localScale *= randomSize;
    }

    private MeshRenderer SetMeshChild(StickingObject stickingObject, Material material)
    {
        int randomMeshIndex = Random.Range(0, meshPrefabs.Length);
        MeshRenderer meshModel = Instantiate(meshPrefabs[randomMeshIndex], Random.insideUnitSphere * radiusSpawn, Quaternion.identity);
        meshModel.transform.SetParent(stickingObject.transform);
        meshModel.transform.localPosition = Vector3.zero;
        meshModel.material = material;
        stickingObject.SetMeshChild(meshModel.transform);
        return meshModel;
    }
}
