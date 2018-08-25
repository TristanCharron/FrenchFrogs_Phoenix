using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class AutoRotate : MonoBehaviour {

	public float animSpeed = 2f;
	public bool animOnStart = true, animActivated, animInPlay;
	private Vector3 rot = Vector3.zero;

	void Start () {
		if(animOnStart){
			StartRotation();
		}
	}

    public void StartRotation()
    {
        animActivated = true;
    }

    public void StopRotation()
    {
        animActivated = false;
    }

    void Update()
    {
        if(animActivated)
        {

            animInPlay = true;
            activateAnim();
        }
    }

    void activateAnim()
    {
        if(animInPlay)
        {
            animActivated = false;
            
            rot.z = Random.Range(0f, 360f);

			DOTween.Kill(transform);
            DOTween.To(() => transform.localRotation, trotate => transform.localRotation = trotate, rot,animSpeed).SetEase(Ease.InOutQuad).OnComplete(CurrentAnim);
			//transform.DOLocalRotate(rot, animSpeed).SetEase(Ease.InOutQuad).OnComplete(CurrentAnim);
            animInPlay = false;
        }
       
    }

    void CurrentAnim()
    {
        animActivated = true;
    }
}
