using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomLocalMovement : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float rotateSpeed = 250f;
    Vector3 rotationAxis = new Vector3(1, 1, 0);
    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.localPosition;
        StartAnim();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationAxis, rotateSpeed * Time.deltaTime);
    }

    void StartAnim()
    {
        transform.DOLocalMove(new Vector3(Random.Range(-startPos.x - 5,startPos.x + 5), Random.Range(-startPos.x - 5, startPos.x + 5), Random.Range(-startPos.x - 5, startPos.x + 5)),speed)
            .SetEase(Ease.InOutQuart).OnComplete(StartAnim);
    }
}
