using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBlock : EnvironmentObject {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationAxis, rotateSpeed * Time.deltaTime);
    }

    public override void SetPosition(Vector3 newPosition)
    {
        base.SetPosition(newPosition);
        StartMovement();
    }

    public void StartMovement()
    {
        float radiusMove = 1;
        //Vector3 randomVector3 = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
        transform.DOLocalMove(startPosition + Random.onUnitSphere * radiusMove, randomSpeed).SetEase(Ease.InOutQuad).OnComplete(ResetMovement);
    }


    public void ResetMovement()
    {
        transform.DOLocalMove(startPosition, randomSpeed).OnComplete(StartMovement);
    }
}
