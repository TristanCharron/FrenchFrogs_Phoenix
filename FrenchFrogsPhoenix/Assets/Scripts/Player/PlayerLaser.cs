using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class PlayerLaser : HitScanner
{
    [SerializeField] float shootAheadDistance = 50;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float laserCostPerSecond = 5;

    private float timer = 0;
    private float tickTimer = 0.1f;
    bool isOnCooldown = false;

    //Vector3 targetPosition;

    void Start()
    {
        base.Start();

        //player.input.SubscribeButtonDown(Action.Fire, Fire);
        // EventManager.Subscribe<Vector3>(EventConst.GetUpdateWorldPosAim(player.ID), (aimPos) => targetPosition = aimPos);
    }

    void Fire()
    {
        Debug.Log("Fire test");
    }

    private void Update()
    {
       if(player.input != null)
       {
            ShowBeam(player.input.GetButton(Action.Fire2));
            HitScanAnalyse();
       }    
    }

    void ShowBeam(bool show)
    {
        if (show && player.Fuel.CurrentFuel > 0)
        {
            player.Fuel.RemoveFuel(laserCostPerSecond * Time.deltaTime);
            CalculateBeam();
            UpdateLaserPosition();
        }
        else
        {
            show = false;
        }
        lineRenderer.enabled = show;
    }

    void CalculateBeam()
    {
        if (isOnCooldown)
            return;

        HitScanDamage();
    }

    void UpdateLaserPosition()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }

    IEnumerator LaserTickDelay()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(tickTimer);
        isOnCooldown = false;
    }
}
