using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static string poolName = "Bullet";
    float duration;
    bool inMotion = false;

    Vector3 velocity;
    Coroutine delayDestroyCoroutine;
    Player player;
    DamageData damageData;
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if(inMotion)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    public void Initialize(Player player, DamageData damageData, Vector3 direction, float speed, float duration)
    {
        this.player = player;
        this.damageData = damageData;
        this.duration = duration;
        inMotion = true;
        velocity = direction * speed;

        delayDestroyCoroutine = StartCoroutine(DelayCoroutineDestroy());

        trail.Clear();
    }

    IEnumerator DelayCoroutineDestroy()
    {
        yield return new WaitForSeconds(duration);
        //End trail
        inMotion = false;

        yield return new WaitForSeconds(.1f);
        PoolManager.instance.ReturnObject(poolName, gameObject);

    }
}
