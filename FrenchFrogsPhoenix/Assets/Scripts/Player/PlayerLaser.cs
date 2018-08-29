using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class PlayerLaser : MonoBehaviour {

    [SerializeField] Collider laserCollider;
    [SerializeField] Player player;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float laserCostPerSecond = 5;

    LaserData laserData;

    private float timer = 0;
    private float tickTimer = 0.1f;
    bool isOnCooldown = false;
    private void Awake()
    {
        laserData = new LaserData();
        laserData.damage = 5;
    }

    void Start()
    {
        player.input.SubscribeButtonDown(Action.Fire, Fire);
    }

    void Fire()
    {
        Debug.Log("Fire test");
    }

    private void Update()
    {
       if(player.input != null)
       {
            ShowBeam(player.input.GetButton(Action.Fire));
       }    
    }

    void ShowBeam(bool show)
    {
        if (show && player.Fuel.CurrentFuel > 0)
        {
            player.Fuel.RemoveFuel(laserCostPerSecond * Time.deltaTime);        
        }
        else
        {
            show = false;
        }

        laserCollider.enabled = show;
        lineRenderer.enabled = show;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOnCooldown)
            return;

        StickingObject stickingObject = other.GetComponent<StickingObject>();
        //ca fait pu de sens lol

        if (stickingObject != null && stickingObject.ObjectParent != player)
        {
            stickingObject.Damage(laserData.damage);
        }
    }

    IEnumerator LaserTickDelay()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(tickTimer);
        isOnCooldown = false;
    }
}
