using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static string poolName = "Bullet";
    public bool useHitScan = true;
    float duration;

    Coroutine delayDestroyCoroutine;
    Rigidbody rigidbody;
    Player player;
    DamageData damageData;
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public void Initialize(Player player, DamageData damageData, Vector3 direction, float speed, float duration)
    {
        this.player = player;
        this.damageData = damageData;
        this.duration = duration;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * speed;

        delayDestroyCoroutine = StartCoroutine(DelayCoroutineDestroy());

        trail.Clear();
    }

    IEnumerator DelayCoroutineDestroy()
    {
        yield return new WaitForSeconds(duration);
        //End trail
        rigidbody.velocity = Vector3.zero;

       // yield return new WaitForSeconds(.1f);
        PoolManager.instance.ReturnObject(poolName, gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (useHitScan)
            return;

        HealthComponent health = other.GetComponent<HealthComponent>();
        if(health)
        {
            health.Damage(damageData);
        }
    }
}
