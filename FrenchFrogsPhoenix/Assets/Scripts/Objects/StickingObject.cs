using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StickingObject : MonoBehaviour {

    public Rigidbody rb { get; protected set; }
    public bool IsSticked {
        get { return StickingObjectParent != null; }
    }

    public StickingObject StickingObjectParent { get; protected set; }

    int maxLife = 20;
    int currentLife;

    List<StickingObject> stickingObjectChilds = new List<StickingObject>();

    private void Awake()
    {
        currentLife = maxLife;
        rb = GetComponent<Rigidbody>();
    }

    public void SetParent(StickingObject stickingParent)
    {
        StickingObjectParent = stickingParent;
    }

    public void Damage(int damage)
    {
        currentLife -= damage;
        if(currentLife < 0)
        {
            Destroy();
        }
    }

    [ContextMenu("Destroy")]
    void Destroy()
    {
        DetatchChilds();
        Destroy(gameObject);
    }

    void StickingNewChild(StickingObject stickingChild)
    {
        stickingChild.StickingObjectParent = this;

        stickingObjectChilds.Add(stickingChild);
        stickingChild.transform.SetParent(transform, true);

        stickingChild.rb.velocity = Vector3.zero;
        stickingChild.rb.angularVelocity = Vector3.zero;
    }

    public void DetatchFromParent()
    {
        transform.SetParent(null, true);
        StickingObjectParent = null;

        DetatchChilds();

        //testing
        rb.velocity = (Random.insideUnitSphere * 2);
        rb.angularVelocity = (Random.insideUnitSphere * 5);
    }

    void DetatchChilds()
    {
        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            stickingChild.DetatchFromParent();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        StickingObject stickingObject = collision.transform.GetComponent<StickingObject>();
        if (stickingObject != null)
        {
            if(IsSticked && !stickingObject.IsSticked)
            {
                Debug.Log("collision");
                StickingNewChild(stickingObject);
            }
        }
    }
}
