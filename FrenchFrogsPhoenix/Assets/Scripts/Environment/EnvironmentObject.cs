using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnvironmentObject : MonoBehaviour {

    [SerializeField] protected float randomSpeed;
    [SerializeField] protected float rotateSpeed = 250f;
    protected Vector3 rotationAxis = new Vector3(1, 1, 0);
    protected Vector3 startPosition;

    public virtual void Start() {
        randomSpeed = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void SetPosition(Vector3 newPosition)
    {
        startPosition = newPosition;
        transform.position = startPosition;
    }       
}
