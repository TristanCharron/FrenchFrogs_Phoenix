using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {

    private static EnvironmentController instance;

    [SerializeField] float respawnSpeed = 1.5f;
    [SerializeField] float maxNumbEnvironmentObject = 50;
    [SerializeField] List<EnvironmentObject> newEnvironmentObjectsRef = new List<EnvironmentObject>();
    [SerializeField] Vector3 maxBounds = new Vector3(1000f, 1000f, 1000f);
    public List<EnvironmentObject> currentObjects = new List<EnvironmentObject>();
    public Transform environmentContainer;

    WaitForSeconds delayRespawn;

    // Use this for initialization
    void Start() {
        instance = this;
        delayRespawn = new WaitForSeconds(respawnSpeed);
        StartSpawnEnvironmentObject();
    }

    // Update is called once per frame
    void Update() {

    }

    public static EnvironmentController GetInstance()
    {
        return instance;
    }

    public void StartSpawnEnvironmentObject()
    {
        if(currentObjects.Count < maxNumbEnvironmentObject)
        {
            StartCoroutine(DelaySpawnEnvironmentObject());
        }
    }

    IEnumerator DelaySpawnEnvironmentObject()
    {
        SpawnEnvironmentObject();
        yield return delayRespawn;
        StartSpawnEnvironmentObject();
    }

    void SpawnEnvironmentObject()
    {
        EnvironmentObject newEnvironmentObject = Instantiate(newEnvironmentObjectsRef[Random.Range(0, newEnvironmentObjectsRef.Count)], environmentContainer);
        newEnvironmentObject.SetPosition(new Vector3(Random.Range(0, maxBounds.x), Random.Range(0, maxBounds.y), Random.Range(0, maxBounds.z)));
    }

    public void ResetEnvironment()
    {
        foreach(EnvironmentObject environmentObject in currentObjects)
        {
            Destroy(environmentObject.gameObject);
        }

        currentObjects.Clear();
    }
}
