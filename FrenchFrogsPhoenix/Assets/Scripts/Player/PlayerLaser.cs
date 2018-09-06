using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class PlayerLaser : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player player; 
    [SerializeField] LineRenderer lineRenderer;

    [Header("Stats")]
    [SerializeField] float shootAheadDistance = 50;
    [SerializeField] float laserCostPerSecond = 5;
    [SerializeField] DamageData damageData;

    [SerializeField] LaserInfo laserInfo;

    HitScanner hitScanner;
    private float timeLaserActivated = 0;
    private float timer = 0;
    private float tickTimer = 0.1f;
    bool isOnCooldown = false;

    void Start()
    {
        damageData.owner = player.Health;
        hitScanner = player.hitScanner;
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
            UpdateLaserWidth();

            timeLaserActivated += Time.deltaTime;
        }
        else
        {
            timeLaserActivated = 0;
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

    void UpdateLaserWidth()
    {

        float width = laserInfo.GetWidth(timeLaserActivated);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
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

    [System.Serializable]
    public class LaserInfo
    {
        public float widthBeamStartSize;
        public float widthBeamIncreasePerSecond;
        public float widthBeamSinAmplitude;
        public float widthBeamSinSpeed;

        public float GetWidth(float timeLaserActivated)
        {
            float sizeOverTime = widthBeamStartSize + (timeLaserActivated * widthBeamIncreasePerSecond);
            float sizeVibration = widthBeamSinAmplitude * Mathf.Sin(timeLaserActivated * widthBeamSinSpeed);

            return sizeOverTime + sizeVibration;
        }
    }

}
