using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;
public class PlayerCannon : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform cannonTip;
    [SerializeField] HitScanner hitScanner;
    [SerializeField] Player player;

    [Header("Stats")]
    [SerializeField] CannonData cannonData;
    [SerializeField] DamageData damageData;

    InputBase input; 
    bool canFire = true;

    void Start()
    {
        damageData.owner = player.Health;
        input = player.input;
        hitScanner = player.CameraFlight.hitScanner;

        input.SubscribeButtonHold(Action.Fire, Fire);
    }

    IEnumerator CooldownFire()
    {
        canFire = false;
        yield return new WaitForSeconds(cannonData.fireRate);
        canFire = true;
    }

    void Fire()
    {
        if (!canFire)
            return;

        GameObject bulletObject = PoolManager.instance.GetObject(Bullet.poolName);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.transform.position = cannonTip.position;

        ///Vector3 realAimDirection = Aim
        Vector3 diff = (hitScanner.GetAssistAimPosition() - transform.position);
        Vector3 dir = diff.normalized;

        bool hasHit = hitScanner.HitScanDamage(damageData);

        float duration = (hasHit) ? (diff.magnitude / cannonData.speed) :  5;
        bullet.Initialize(player, damageData, dir, cannonData.speed, duration);

        StartCoroutine(CooldownFire());


         bullet.useHitScan = true;
    }
}
