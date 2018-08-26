﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyObjectFactory : MonoBehaviour {

    [SerializeField] int numberSpawn = 2000;
    [SerializeField] int initialCount = 100;

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

    Queue<StickingObject> stickyQueue = new Queue<StickingObject>();

	void Update ()
    {
        currentTimerSpawn += Time.deltaTime;
        if (currentTimerSpawn > timerSpawn)
        {
            currentTimerSpawn = 0;
            SpawnObject();
        }
    }

    private void Start()
    {
        for (int i = 0; i < numberSpawn; i++)
        {
            SetToPool();
        }
        for (int i = 0; i < initialCount; i++)
        {
            SpawnObject();
        }
    }

    public void DestroyObject(StickingObject stickingObject)
    {
        stickingObject.gameObject.SetActive(false);
        stickingObject.transform.SetParent(transform);
    }

    void SpawnObject()
    {
        StickingObject stickingObject = stickyQueue.Dequeue();

        if (stickingObject == null)
            return;

        Rigidbody rigidBody = stickingObject.rb;
        rigidBody.velocity = (Random.insideUnitSphere * vectorMagnitude);
        rigidBody.angularVelocity = (Random.insideUnitSphere * spinMagnitude);

        stickingObject.transform.position = Random.insideUnitSphere * radiusSpawn;

        stickingObject.gameObject.SetActive(true);
    }

    void SetToPool()
    {
        StickingObject stickingObject = Instantiate(prefabStickingObjet, transform.position, Quaternion.identity);
        stickingObject.Factory = this;

        int materialRandom = Random.Range(0, materials.Length);
        Material material = materials[materialRandom];
        MeshRenderer mesh = SetMeshChild(stickingObject, material);

        float randomSize = Random.Range(.1f, 2f);
        ObjectStats stats = new ObjectStats();
        stats.SetType((ObjectStats.Type)materialRandom);

        stats *= randomSize;
        stickingObject.SetObjectStats(stats);

        stickingObject.transform.localScale *= randomSize;

        stickingObject.transform.SetParent(transform);

        stickyQueue.Enqueue(stickingObject);
        stickingObject.gameObject.SetActive(false);
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
