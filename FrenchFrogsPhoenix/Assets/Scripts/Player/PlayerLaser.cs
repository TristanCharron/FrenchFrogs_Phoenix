using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour {

    [SerializeField] Collider laserCollider;
    [SerializeField] Player player;
    [SerializeField] LineRenderer lineRenderer;

    LaserData laserData;

    private float timer = 0;

    private void Start()
    {
        laserData = new LaserData();
        laserData.damage = 5;

        player.input.FireButton.AddEvent(()=> {
            ShowBeam(true);
        });
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ShowBeam(false);
        }
            
    }

    void ShowBeam(bool show)
    {
        laserCollider.enabled = show;
        lineRenderer.enabled = show;

        if (show)
        {
            timer = 0.02f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StickingObject stickingObject = other.GetComponent<StickingObject>();
        if (stickingObject != null && stickingObject.PlayerParent != player)
        {
            //stickingObject.Damage(laserData.damage);
        }
    }
}
