using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour {

    [SerializeField] Collider laserCollider;
    [SerializeField] Player player;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float laserCostPerSecond = 5;

    LaserData laserData;

    private float timer = 0;
    private float tickTimer = 0.1f;
    bool isOnCooldown = false;
    private void Start()
    {
        laserData = new LaserData();
        laserData.damage = 5;
    }

    private void Update()
    {
       if(player.input != null)
        {
            ShowBeam(player.input.FireButton.IsPressed);
        }
            
    }

    void ShowBeam(bool show)
    {
        laserCollider.enabled = show;
        lineRenderer.enabled = show;

        if(show)
            player.Fuel.RemoveFuel(laserCostPerSecond * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOnCooldown)
            return;

        if (laserData == null)
            return;

        StickingObject stickingObject = other.GetComponent<StickingObject>();
        if (stickingObject != null && stickingObject.PlayerParent != player)
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
