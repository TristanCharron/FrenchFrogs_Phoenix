using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanner : MonoBehaviour {

    public Player player { get; set; }

    public LayerMask IgnoreCollisionLayerMask {get; protected set;}

    public Vector3 AimDirection { get; protected set; }
    public Vector3 TargetPosition { get; protected set; }

    RaycastHit previousScanHit;
    RaycastHit scanHit;

    float aimStickTimer = 0.3f;
    float aimStickCurrentTimer = 0;

    string layerMaskName = "Interactable";

    public Vector3 GetAssistAimPosition()
    {
        if(scanHit.collider != null)
        {
            return scanHit.collider.transform.position;
        }
        else
        {
            return TargetPosition;
        }
    }

    protected void Start()
    {
        IgnoreCollisionLayerMask = (1 << LayerMask.NameToLayer(layerMaskName));
        EventManager.Subscribe<Vector3>(EventConst.GetUpdateWorldPosAim(player.ID), (aimPosition) => UpdateAimDirection(aimPosition));
    }

    void UpdateAimDirection(Vector3 aimPosition)
    {
        TargetPosition = aimPosition;
        AimDirection = (aimPosition - transform.position).normalized;
    }

    public bool HitScanDamage(DamageData damageData)
    {
        if (scanHit.collider != null)
        {
            HealthComponent health = scanHit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.Damage(damageData);
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        HitScanAnalyse();
    }

    void HitScanAnalyse()
    {
        float minDistance = 3;
        Vector3 startPosition = transform.position + transform.forward * minDistance;
        Vector3 diff = (TargetPosition - startPosition);
        float distance = diff.magnitude;
        Vector3 direction = diff.normalized;

        RaycastHit[] hits = Physics.RaycastAll(startPosition, direction, distance, IgnoreCollisionLayerMask);

        SetAimStickness(hits);

        Debug.DrawRay(startPosition, direction * distance);
        InvokeEventIfNewTargetInSight();

        previousScanHit = scanHit;
    }

    private void SetAimStickness(RaycastHit[] hits)
    {
        if (hits.Length == 0)
        {
            aimStickCurrentTimer += Time.deltaTime;
            if (aimStickCurrentTimer > aimStickTimer && scanHit.collider != null)
            {
                scanHit = new RaycastHit();
            }
        }
        else
        {
            aimStickCurrentTimer = 0;
            FilterGoodHits(hits);
        }
    }

    private void FilterGoodHits(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != player.gameObject)
            {
                scanHit = hits[i];
                break;
            }
        }
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
