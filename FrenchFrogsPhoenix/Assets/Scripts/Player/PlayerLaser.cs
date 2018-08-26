﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour {

    [SerializeField] Collider laserCollider;
    [SerializeField] Player player;
    [SerializeField] LineRenderer lineRenderer;

    LaserData laserData;
    bool isReady = true;

    private void Start()
    {
        laserData = new LaserData();
        laserData.damage = 5;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ShowBeam(true);
        }
        else
        {
            ShowBeam(false);
        }
    }

    void ShowBeam(bool show)
    {
        laserCollider.enabled = show;
        lineRenderer.enabled = show;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isReady)
            return;

        StickingObject stickingObject = other.GetComponent<StickingObject>();
        if (stickingObject != null && stickingObject.PlayerParent != player)
        {
            Debug.Log("COLLSISION");
        }
    }

    IEnumerator TickDelay()
    {
        isReady = false;
        yield return new WaitForSeconds(.2f);
        isReady = true;
    }
}
