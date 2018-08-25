using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StickingObject : MonoBehaviour {

    //Accesors
    public Rigidbody rb { get; protected set; }
    public bool IsSticked { get { return StickingObjectParent != null; } }
    public StickingObject StickingObjectParent { get; protected set; }

    //Life
    int maxLife = 20;
    int currentLife;

    ObjectStats objectStats;
    List<StickingObject> stickingObjectChilds = new List<StickingObject>();
    Player playerParent;

    private void Awake()
    {
        //Test
        objectStats = new ObjectStats();
        objectStats.damage = Random.Range(0, 3);
        objectStats.speed = Random.Range(0, 3);
    
        currentLife = maxLife;
        rb = GetComponent<Rigidbody>();
    }

    public void RecrusiveCalculateStats(ObjectStats playerStats)
    {
        playerStats += objectStats;
        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            stickingChild.RecrusiveCalculateStats(playerStats);
        }
    }

    public void SetParent(StickingObject stickingParent, Player playerParent)
    {
        StickingObjectParent = stickingParent;
        this.playerParent = playerParent;
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
        stickingChild.SetParent(this, playerParent);

        stickingObjectChilds.Add(stickingChild);
        stickingChild.transform.SetParent(transform, true);

        stickingChild.rb.velocity = Vector3.zero;
        stickingChild.rb.angularVelocity = Vector3.zero;

        playerParent.OnNewStickingObject.Invoke(this);
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
