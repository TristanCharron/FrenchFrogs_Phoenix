using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnvironmentObject : MonoBehaviour {

    [SerializeField] float randomSpeed;
    [SerializeField] float rotateSpeed = 0.5f;
    Vector3 startPosition;
    Vector3 rotationAxis = new Vector3(1,1,0);

    // Use this for initialization
    void Start() {
        randomSpeed = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(rotationAxis, rotateSpeed * Time.deltaTime);
    }

    public void SetPosition(Vector3 newPosition)
    {
        startPosition = newPosition;
        transform.position = startPosition;
        StartMovement();
    }

    public void StartMovement()
    {
        transform.DOLocalMove(new Vector3(startPosition.x + Random.Range(-1,2),startPosition.y + Random.Range(-1, 2), startPosition.z + Random.Range(-1, 2)) ,randomSpeed).SetEase(Ease.InOutQuad).OnComplete(ResetMovement);
    }

    public void ResetMovement()
    {
        transform.DOLocalMove(startPosition,randomSpeed).OnComplete(StartMovement);
    }
        
}
