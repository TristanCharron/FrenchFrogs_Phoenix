using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class StickingObject : MonoBehaviour {

    //Accesors
    public Rigidbody rb { get; protected set; }
    public bool IsSticked { get { return PlayerParent != null; } }
    public Player PlayerParent { get; protected set; }

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



    private void Awake()
    {
        ////Test
        //objectStats = new ObjectStats();
        //objectStats.damage = Random.Range(0, 3);
        //objectStats.speed = Random.Range(0, 3);
    
        currentLife = maxLife;
        rb = GetComponent<Rigidbody>();
        sinRotation = GetComponent<SinRotation>();
        Wiggle();
    }

    public void SetMeshChild(Transform childMeshTransform)
    {
        this.childMeshTransform = childMeshTransform;
    }

    public void SetParent(Player playerParent, StickingObject stickingObjectParent)
    {
        sinRotation.Initialize();
        this.stickingObjectParent = stickingObjectParent;
        this.PlayerParent = playerParent;
    }

    public void SetObjectStats(ObjectStats objectStats)
    {
        this.objectStats = objectStats;
    }

    public void RecrusiveCalculateStats(ObjectStats playerStats)
    {
        playerStats += objectStats;
        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            stickingChild.RecrusiveCalculateStats(playerStats);
        }
    }

    public void Damage(int damage)
    {
        childMeshTransform.DOKill();
        childMeshTransform.DOShakeScale(1, 1, 20).OnComplete(Wiggle);

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
        PlayerParent.OnNewStickingObject.Invoke(this);
    }

    public void StickingNewChild(StickingObject stickingChild)
    {
        stickingChild.transform.SetParent(transform, true);

        stickingChild.SetParent(PlayerParent, this);

        stickingObjectChilds.Add(stickingChild);

        stickingChild.rb.velocity = Vector3.zero;
        stickingChild.rb.angularVelocity = Vector3.zero;

        PlayerParent.OnNewStickingObject.Invoke(this);
    }

    public void DetatchFromParent()
    {
        transform.SetParent(null, true);
        PlayerParent = null;

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
        //childMeshTransform.localScale = Vector3.one;
        childMeshTransform.DOShakeScale(1, impact, 20).OnComplete(Wiggle);

        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            if(stickingChild != objectToIgnore)
                stickingChild.ShakeScale(impact * impactPropagation, this);
        }

        if (stickingObjectParent != null && stickingObjectParent != objectToIgnore)
            stickingObjectParent.ShakeScale(impact * impactPropagation, this);
    }

    void Wiggle()
    {
        childMeshTransform.DOShakeScale(1, 0.05f, 20).SetDelay(.2f).SetLoops(-1, LoopType.Yoyo);

    }
}
