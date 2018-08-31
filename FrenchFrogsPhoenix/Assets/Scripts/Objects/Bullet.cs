using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static string poolName = "Bullet";
    [SerializeField] float delayDestroy;
    public bool useHitScan = true;
    Coroutine delayDestroyCoroutine;
    Rigidbody rigidbody;
    Player player;
    DamageData damageData;
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public void Initialize(Player player, DamageData damageData, Vector3 direction, float speed)
    {
        this.player = player;
        this.damageData = damageData;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * speed;

        delayDestroyCoroutine = StartCoroutine(DelayCoroutineDestroy());

        trail.Clear();
    }

    IEnumerator DelayCoroutineDestroy()
    {
        yield return new WaitForSeconds(delayDestroy);

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
