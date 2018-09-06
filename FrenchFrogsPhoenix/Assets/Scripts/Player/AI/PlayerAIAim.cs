using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIAim : MonoBehaviour {

    public Collider currentTarget { get; protected set; }

    Player player;
    float reflexSpeed = 2;
    float radius = 100; //Must be the same as playerAim "aimAheadDistance"
    Collider collider;
    Vector3 currentAimPosition;

    private void Start()
    {
        player = GetComponent<Player>();
        collider = GetComponent<Collider>();
        currentAimPosition = transform.position;
    }

    private void Update()
    {
        UpdateAimPosition();
    }

    void UpdateAimPosition()
    {
        LayerMask ignoreCollision = player.hitScanner.IgnoreCollisionLayerMask;
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, ignoreCollision);

        currentTarget = null;
        for (int i = 0; i < hit.Length; i++)
        {
            if(hit[i] != collider)
            {
                currentTarget = hit[i];
                break;
            }
        }

        Vector3 aimDesiredPosition;
        if(currentTarget == null)
        {
            aimDesiredPosition = transform.forward * radius;
        }
        else
        {
            aimDesiredPosition = currentTarget.transform.position;
        }

        currentAimPosition = Vector3.Lerp(currentAimPosition, aimDesiredPosition, reflexSpeed * Time.deltaTime);

        EventManager.Invoke<Vector3>(EventConst.GetUpdateWorldPosAim(player.ID), currentAimPosition);
    }
}
