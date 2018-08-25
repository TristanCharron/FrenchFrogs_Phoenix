using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class StickingObject : MonoBehaviour {

    //Accesors
    public Rigidbody rb { get; protected set; }
    public bool IsSticked { get { return playerParent != null; } }

    [SerializeField] float impactBlobiness;
    [SerializeField, Range(0, 1)] float impactPropagation = 0.8f;


    //Life
    int maxLife = 20;
    int currentLife;

    ObjectStats objectStats;
    SinRotation sinRotation;
    Transform childMeshTransform;

    StickingObject stickingObjectParent;
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

    public void SetMeshChild(Transform childMeshTransform)
    {
        this.childMeshTransform = childMeshTransform;
    }

    public void SetParent(Player playerParent, StickingObject stickingObjectParent)
    {
        sinRotation.Initialize();
        this.stickingObjectParent = stickingObjectParent;
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
        SetParent(player, null);
        playerParent.OnNewStickingObject.Invoke(this);
    }

    public void StickingNewChild(StickingObject stickingChild)
    {
        stickingChild.transform.SetParent(transform, true);

        stickingChild.SetParent(playerParent, this);

        stickingObjectChilds.Add(stickingChild);

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

                ShakeScale(impactBlobiness, this);
            }
        }
    }

    public void ShakeScale(float impact, StickingObject objectToIgnore)
    {
        if (impact < 0.01)
            return;

        childMeshTransform.DOKill();
        childMeshTransform.localScale = Vector3.one;
        childMeshTransform.DOShakeScale(1, impact, 20);

        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            if(stickingChild != objectToIgnore)
                stickingChild.ShakeScale(impact * impactPropagation, this);
        }

        if (stickingObjectParent != null && stickingObjectParent != objectToIgnore)
            stickingObjectParent.ShakeScale(impact * impactPropagation, this);
    }
}
