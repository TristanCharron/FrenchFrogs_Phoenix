using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass : MonoBehaviour {

    [SerializeField] protected bool attract;
    [SerializeField] protected float attractForce;
    [SerializeField] protected Transform core;
    [SerializeField] protected AnimationCurve attractFactor;
    [SerializeField] protected float startVectorMagnitude;
    [SerializeField] protected float startSpinMagnitude;

    protected SphereCollider sphereCollider;
    protected Rigidbody rigidbody;

    // Use this for initialization
    public virtual void Start () {
        sphereCollider = GetComponent<SphereCollider>();
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.velocity = (Random.insideUnitSphere * startVectorMagnitude);
        rigidbody.angularVelocity = (Random.insideUnitSphere * startSpinMagnitude);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void Attraction(Rigidbody objectRb)
    {
        Vector3 diff = (transform.position - objectRb.transform.position);
        float directionFactor = (attract) ? 1 : -1;
        float inverseRatio = 1 - (diff.magnitude / sphereCollider.radius);
        float evaluatedRatio = attractFactor.Evaluate(inverseRatio);

        objectRb.velocity *= 0.9f;
        objectRb.AddForce(diff * directionFactor * evaluatedRatio * attractForce);
    }
}
