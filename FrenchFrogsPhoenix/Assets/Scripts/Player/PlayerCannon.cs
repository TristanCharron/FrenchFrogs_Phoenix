using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;
public class PlayerCannon : HitScanner
{
    [SerializeField] Transform cannonTip;
    [SerializeField] CannonData cannonData;
    [SerializeField] bool useHitScan= true;

    InputBase input; 
    bool canFire = true;

    void Start()
    {
        base.Start();

        input = player.input;

        //input.SubscribeButtonDown(Action.Fire, Fire);
        input.SubscribeButtonHold(Action.Fire, Fire);
    }

    void Update()
    {
        HitScanAnalyse();
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
        Vector3 dir = (GetAssistAimPosition() - transform.position) .normalized;
        bullet.Initialize(player, damageData, dir, cannonData.speed);

        StartCoroutine(CooldownFire());

        if(useHitScan)
        {
            HitScanDamage();
            bullet.useHitScan = true;
        }
    }
}
