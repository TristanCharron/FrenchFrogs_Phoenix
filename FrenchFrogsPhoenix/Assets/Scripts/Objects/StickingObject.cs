using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class StickingObject : MonoBehaviour {

    //Accesors
    public Rigidbody rb { get; protected set; }
    public bool IsSticked { get { return playerParent != null; } }

    [SerializeField] float onImpactBlobiness;

    //Life
    int maxLife = 20;
    int currentLife;

    ObjectStats objectStats;
    SinRotation sinRotation;

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
        sinRotation = GetComponent<SinRotation>();
    }

    public void RecrusiveCalculateStats(ObjectStats playerStats)
    {
        playerStats += objectStats;
        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            stickingChild.RecrusiveCalculateStats(playerStats);
        }
    }

    public void SetPlayerParent(Player playerParent)
    {
        sinRotation.Initialize();
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
        stickingObjectChilds.Clear();
        Destroy(gameObject);
    }

    public void SetFirstStickingchild(Player player)
    {
        SetPlayerParent(player);
        playerParent.OnNewStickingObject.Invoke(this);
    }

    public void StickingNewChild(StickingObject stickingChild)
    {
        stickingChild.SetPlayerParent(playerParent);

        stickingObjectChilds.Add(stickingChild);
        stickingChild.transform.SetParent(transform, true);


        stickingChild.rb.velocity = Vector3.zero;
        stickingChild.rb.angularVelocity = Vector3.zero;

        playerParent.OnNewStickingObject.Invoke(this);
    }

    public void DetatchFromParent()
    {
        transform.SetParent(null, true);
        playerParent = null;

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

                ShakeScale(transform);
                ShakeScale(stickingObject.transform);
            }
        }
    }

    void ShakeScale(Transform transform)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOShakeScale(1, onImpactBlobiness, 20);
    }
}
