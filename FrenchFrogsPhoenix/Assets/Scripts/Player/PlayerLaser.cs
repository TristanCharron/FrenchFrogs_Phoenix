using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class PlayerLaser : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player player;
    [SerializeField] HitScanner hitScanner;
    [SerializeField] LineRenderer lineRenderer;

    [Header("Stats")]
    [SerializeField] float shootAheadDistance = 50;
    [SerializeField] float laserCostPerSecond = 5;
    [SerializeField] DamageData damageData;

    private float timer = 0;
    private float tickTimer = 0.1f;
    bool isOnCooldown = false;

    void Start()
    {
        damageData.owner = player.Health;
        hitScanner = player.CameraFlight.hitScanner;
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

        hitScanner.HitScanDamage(damageData);
    }

    void UpdateLaserPosition()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hitScanner.TargetPosition);
    }

    IEnumerator LaserTickDelay()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(tickTimer);
        isOnCooldown = false;
    }
}
