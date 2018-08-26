using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Pulse : MonoBehaviour {

	public float speed = 0.5f;
	public Vector3 normal = new Vector3(1,1,1);
	public Vector3 min = new Vector3(0.8f, 0.8f, 0.8f);
	public bool animOnStart = true;
	public bool hidden;

	void Start () {
		if(animOnStart){
			StartPulse();
		}
	}

	void OnDisable(){
		DOTween.Kill(transform);
	}

	public void StartPulse(){
		DOTween.Kill(transform);
		transform.localScale = normal;
		transform.DOScale(min, speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
	}

	public void StopPulse(){
		DOTween.Kill(transform);
		transform.DOScale(normal, speed).SetEase(Ease.InOutQuad);
	}

	public void Show(){

		if(hidden){
			DOTween.Kill(transform);
			transform.DOScale(normal, speed).SetEase(Ease.InOutQuad).OnComplete(StartPulse);
		}

		hidden = false;
	}

	public void Hide(){

		if(!hidden){
			DOTween.Kill(transform);
			transform.DOScale(Vector3.zero, speed).SetEase(Ease.InOutQuad);
		}

		hidden = true;
	}
}
