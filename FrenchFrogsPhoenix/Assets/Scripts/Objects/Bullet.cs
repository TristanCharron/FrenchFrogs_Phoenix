using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public static string poolName = "Bullet";
    [SerializeField] float delayDestroy;
    Coroutine delayDestroyCoroutine;
    CannonData cannonData;
    Rigidbody rigidbody;
    Player player;

    public void Initialize(Player player, CannonData cannonData, Vector3 direction)
    {
        this.player = player;
        this.cannonData = cannonData;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * cannonData.speed;

        delayDestroyCoroutine = StartCoroutine(DelayCoroutineDestroy());
    }

    IEnumerator DelayCoroutineDestroy()
    {
        yield return new WaitForSeconds(delayDestroy);
        PoolManager.instance.ReturnObject(poolName, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
     //to redo
    }
}
