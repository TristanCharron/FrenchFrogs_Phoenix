using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A centraliser, pas que le cannon/laser/etc herite de ca
public class HitScanner : MonoBehaviour {

    [SerializeField] bool hitFromCamera;
    [SerializeField] protected DamageData damageData;
    [SerializeField] protected Player player;

    [SerializeField] LayerMask ignoreCollision;

    protected Vector3 aimDirection;
    protected Vector3 targetPosition;

    RaycastHit previousScanHit;
    RaycastHit scanHit;

    float aimStickTimer = 0.3f;
    float aimStickCurrentTimer = 0;

    CameraFlightFollow cameraFligth;

    public Vector3 GetAssistAimPosition()
    {
        if(scanHit.collider != null)
        {
            return scanHit.collider.transform.position;
        }
        else
        {
            return targetPosition;
        }
    }

    protected void Start()
    {      
        damageData.owner = player.Health;
        cameraFligth = GetComponent<CameraFlightFollow>();
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
        Vector3 startPosition = (hitFromCamera) ? Camera.main.transform.position + Camera.main.transform.forward * 5 : transform.transform.position;

        Vector3 diff = (targetPosition - startPosition);
        float distance = diff.magnitude;
        Vector3 direction = diff.normalized;

        float sphereCastRadius = 1;
        //Physics.Raycast(transform.position, direction, out scanHit, distance);
        //Physics.SphereCast(startPosition, sphereCastRadius, direction, out scanHit, distance, ignoreCollision);
        RaycastHit[] hits = Physics.RaycastAll(startPosition, direction, distance, ignoreCollision);

        SetAimStickness(hits);

        Debug.DrawRay(startPosition, direction * distance);
        InvokeEventIfNewTargetInSight();

        //if(scanHit.collider != null)
        //    Debug.Log(scanHit.collider.name);

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
