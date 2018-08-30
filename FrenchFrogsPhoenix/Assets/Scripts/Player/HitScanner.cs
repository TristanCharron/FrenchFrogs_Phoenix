using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanner : MonoBehaviour {

    [SerializeField] protected DamageData damageData;
    [SerializeField] protected Player player;

    protected Vector3 aimDirection;
    protected Vector3 targetPosition;

    RaycastHit previousScanHit;
    RaycastHit scanHit;

    protected void Start()
    {
        damageData.owner = gameObject;
        EventManager.Subscribe<Vector3>(EventConst.GetUpdateWorldPosAim(player.ID), (aimPosition) => UpdateAimDirection(aimPosition));
    }

    void UpdateAimDirection(Vector3 aimPosition)
    {
        targetPosition = aimPosition;
        aimDirection = (aimPosition - transform.position).normalized;
    }

    protected void HitScanDamage()
    {
        if (scanHit.collider != null)
        {
            HealthComponent health = scanHit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.Damge(damageData);
            }
        }
    }

    protected void HitScanAnalyse()
    {
        Vector3 diff = (targetPosition - transform.position);
        float distance = diff.magnitude;
        Vector3 direction = diff.normalized;

        float sphereCastRadius = 1;
        //Physics.Raycast(transform.position, direction, out scanHit, distance);
        Physics.SphereCast(transform.position, sphereCastRadius, direction, out scanHit, distance);
        Debug.DrawRay(transform.position, direction * distance);
        InvokeEventIfNewTargetInSight();


        if(scanHit.collider != null)
            Debug.Log(scanHit.collider.name);

        previousScanHit = scanHit;
    }

    private void InvokeEventIfNewTargetInSight()
    {
        if (scanHit.collider != null)
        {
            if (previousScanHit.collider == null)
            {
                EventManager.Invoke<bool>(EventConst.GetUpdateAimTargetInSight(player.ID), true);
            }
        }
        else
        {
            if (previousScanHit.collider != null)
            {
                EventManager.Invoke<bool>(EventConst.GetUpdateAimTargetInSight(player.ID), false);
            }
        }
    }
}
