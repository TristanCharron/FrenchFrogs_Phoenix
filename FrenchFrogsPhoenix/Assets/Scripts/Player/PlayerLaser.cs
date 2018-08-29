using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class PlayerLaser : MonoBehaviour {

    [SerializeField] Collider laserCollider;
    [SerializeField] float laserSpeed;
    [SerializeField] float aimingEase = 5;
    [SerializeField] float shootAheadDistance = 50;
    [SerializeField] Player player;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float laserCostPerSecond = 5;

    DamageData damageData;

    private float timer = 0;
    private float tickTimer = 0.1f;
    bool isOnCooldown = false;

    Vector3 targetPosition;
    private void Awake()
    {
        damageData = new DamageData();
        damageData.owner = gameObject;
        damageData.damage = 5;
    }

    void Start()
    {
        //player.input.SubscribeButtonDown(Action.Fire, Fire);
        EventManager.Subscribe<Vector2>("UpdateCenterMousePosition", (mPos) => UpdateTargetPosition(mPos));
    }

    void Fire()
    {
        Debug.Log("Fire test");
    }

    private void Update()
    {
       if(player.input != null)
       {
            ShowBeam(player.input.GetButton(Action.Fire));


       }    
    }

    void ShowBeam(bool show)
    {
        if (show && player.Fuel.CurrentFuel > 0)
        {
            player.Fuel.RemoveFuel(laserCostPerSecond * Time.deltaTime);
            CalculateBeam();
        }
        else
        {
            show = false;
        }

        laserCollider.enabled = show;
        lineRenderer.enabled = show;
    }

    void CalculateBeam()
    {
        if (isOnCooldown)
            return;

        Vector3 diff = (targetPosition - transform.position);
        float distance = diff.magnitude;
        Vector3 direction = diff.normalized;

        RaycastHit hit;
        Physics.Raycast(transform.position, direction, out hit, distance);

        if(hit.collider != null)
        {
            HealthComponent health = hit.collider.GetComponent<HealthComponent>();
            if(health != null)
            {
                health.Damge(damageData);
            }
        }
    }

    void UpdateTargetPosition(Vector2 mousePosition)
    {
        Vector3 targetPosition = transform.position + Camera.main.transform.forward * shootAheadDistance;
        targetPosition += (mousePosition.x * Camera.main.transform.right + mousePosition.y * Camera.main.transform.up) * aimingEase;

        //Vector3 normalisedPosition = (transform.position - targetPosition).normalized * shootAheadDistance;
        // Vector3 cameraPoint = Camera.main.WorldToScreenPoint(mousePosition);
        //Vector3 direction = (cameraPoint - transform.position);
        //Vector3 targetPosition = cameraPoint + direction + player.transform.forward * shootAheadDistance;
        //Debug.DrawRay(transform.position, targetPosition - transform.position);

        Vector3 oldPosition = lineRenderer.GetPosition(1);
        this.targetPosition = Vector3.Lerp(oldPosition, targetPosition, Time.deltaTime * laserSpeed);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (isOnCooldown)
    //        return;

    //    StickingObject stickingObject = other.GetComponent<StickingObject>();
    //    //ca fait pu de sens lol

    //    if (stickingObject != null && stickingObject.ObjectParent != player)
    //    {
    //        stickingObject.Damage(laserData.damage);
    //    }
    //}

    IEnumerator LaserTickDelay()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(tickTimer);
        isOnCooldown = false;
    }
}
