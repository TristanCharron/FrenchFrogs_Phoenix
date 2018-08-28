using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class StickingObject : MonoBehaviour {

    public StickyObjectFactory Factory { get; set; }

    //Accesors
    public Rigidbody rb { get; protected set; }
    public bool IsSticked { get { return ObjectParent != null; } }
    public MassObject ObjectParent { get; protected set; }

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

    Vector3 size;

    bool isInvincible = false;
    float invisibilityTime = 0.5f;

    private void Awake()
    {   
        rb = GetComponent<Rigidbody>();
        sinRotation = GetComponent<SinRotation>();
    }

    public void Init()
    {
        currentLife = maxLife;
    }

    private void Start()
    {
        Wiggle();
    }

    public void SetMeshChild(Transform childMeshTransform)
    {
        this.childMeshTransform = childMeshTransform;
    }

    public void SetParent(MassObject ObjectParent, StickingObject stickingObjectParent)
    {
        sinRotation.Initialize();
        this.stickingObjectParent = stickingObjectParent;
        this.ObjectParent = ObjectParent;
        size = transform.localScale;
        rb.isKinematic = true;
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
            if (stickingChild == this)
                Debug.LogError("HUGE MISTAKE");

            stickingChild.RecrusiveCalculateStats(playerStats);
        }
    }

    public void Damage(int damage)
    {
        if (isInvincible)
            return;

        childMeshTransform.DOKill();
        childMeshTransform.DOShakeScale(1, 1, 20).OnComplete(Wiggle);

        currentLife -= damage;
        StartCoroutine(InvincibilityDelay());

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
        rb.isKinematic = false;

        //Destroy(gameObject);

        //if (ObjectParent != null)
        //    ObjectParent.OnDestroyStickingObject.Invoke(this);

        if (Factory != null)
            Factory.DestroyObject(this);
    }

    
    IEnumerator InvincibilityDelay()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invisibilityTime);
        isInvincible = false;
    }

    public void SetFirstStickingchild(MassObject parent)
    {
        SetParent(parent, null);
        //ObjectParent.OnNewStickingObject.Invoke(this);
    }

    public void StickingNewChild(StickingObject stickingChild)
    {
        stickingChild.transform.SetParent(transform, true);

        stickingChild.SetParent(ObjectParent, this);

        stickingObjectChilds.Add(stickingChild);

        stickingChild.rb.velocity = Vector3.zero;
        stickingChild.rb.angularVelocity = Vector3.zero;

        //PlayerParent.OnNewStickingObject.Invoke(this);

        AkSoundEngine.PostEvent("Stick", gameObject);
    }

    public void DetatchFromParent()
    {
        transform.SetParent(null, true);
        ObjectParent = null;
        stickingObjectParent = null;

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
        childMeshTransform.DOShakeScale(1, impact, 20).OnComplete(ResetSizeWiggle);

        foreach (StickingObject stickingChild in stickingObjectChilds)
        {
            if(stickingChild != objectToIgnore)
                stickingChild.ShakeScale(impact * impactPropagation, this);
        }

        if (stickingObjectParent != null && stickingObjectParent != objectToIgnore)
            stickingObjectParent.ShakeScale(impact * impactPropagation, this);
    }

    void ResetSizeWiggle()
    {
        transform.localScale = size;
        Wiggle();
    }

    void Wiggle()
    {
        float randomDelay = Random.Range(0, 3f);
        float randomWiggle = Random.Range(0.005f, 0.02f);
        float randomWiggleDuration = Random.Range(.2f, 2f);
        childMeshTransform.DOShakeScale(randomWiggleDuration, randomWiggle, 20)
            .SetDelay(randomDelay)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
