using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] Transform cannonTip;
    CannonData cannonData;

    bool canFire = true;

	void Start ()
    {
        //player.input.FireButton.AddEvent(Fire);
        cannonData = new CannonData();
        cannonData.damage = 5;
        cannonData.speed = 50;
        cannonData.fireRate = .5f;
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

        bullet.Initialize(player, cannonData, player.transform.forward);

        StartCoroutine(CooldownFire());
    }
}
