using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public static string poolName = "Bullet";

    CannonData cannonData;
    Rigidbody rigidbody;
    Player player;

    public void Initialize(Player player, CannonData cannonData, Vector3 direction)
    {
        this.player = player;
        this.cannonData = cannonData;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * cannonData.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        StickingObject stickingObject = GetComponent<StickingObject>();
        if(stickingObject != null && stickingObject.PlayerParent != player)
        {
            stickingObject.Damage(cannonData.damage);
            PoolManager.instance.ReturnObject(poolName, gameObject);
        }
    }
}
