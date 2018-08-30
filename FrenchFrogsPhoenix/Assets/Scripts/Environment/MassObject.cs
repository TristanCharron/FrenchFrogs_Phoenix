using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MassObject : MonoBehaviour {
    [SerializeField] bool attract;
    [SerializeField] float attractForce;
    [SerializeField] Transform core;
    [SerializeField] StickingObject stickingObject;
    [SerializeField] AnimationCurve attractFactor;

    [SerializeField] float startVectorMagnitude;
    [SerializeField] float startSpinMagnitude;

    SphereCollider sphereCollider;
    Rigidbody rigidbody;
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        rigidbody = GetComponent<Rigidbody>();
        stickingObject.SetObjectStats(new ObjectStats());
        stickingObject.SetFirstStickingchild(this);
        stickingObject.SetMeshChild(core);

        rigidbody.velocity = (Random.insideUnitSphere * startVectorMagnitude);
        rigidbody.angularVelocity = (Random.insideUnitSphere * startSpinMagnitude);
    }

    void Attraction(StickingObject stickingObject)
    {
        Vector3 diff = (transform.position - stickingObject.transform.position);
        float directionFactor = (attract) ? 1 : -1;
        float inverseRatio = 1 - (diff.magnitude / sphereCollider.radius);
        float evaluatedRatio = attractFactor.Evaluate(inverseRatio);

        stickingObject.rb.velocity *= 0.9f;
        stickingObject.rb.AddForce(diff * directionFactor * evaluatedRatio * attractForce);
    }

    private void OnTriggerStay(Collider other)
    {
        StickingObject stickingObject = other.GetComponent<StickingObject>();
        if(stickingObject)
        {
            Attraction(stickingObject);
        }
    }
}
