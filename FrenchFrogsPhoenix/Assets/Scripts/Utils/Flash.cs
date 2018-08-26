using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Flash : MonoBehaviour {

	public CanvasGroup grp;
	public float defaultOpacity = 1f;
	public float min = 0f;
	public float max = 1f;
	public float speed = 0.5f;
	public bool randomDelay;
	public bool visibleOnStart = false;
	public bool visibleOnStop = false;
	public bool animOnStart = false;

	void Start(){
		if(visibleOnStart){
			grp.alpha = defaultOpacity;
		} else {
			grp.alpha = 0f;
		}

		if(animOnStart){
			StartFlash();
		}
	}

	void OnDestroy(){
		DOTween.Kill(grp);
	}

	public void StartFlash(){
		float delay = 0f;
		if(randomDelay){
			delay = Random.Range(0f, 0.5f);
		}

		grp.alpha = min;

		DOTween.Kill(grp);
		grp.DOFade(max, speed).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
	}

	public void StopFlash(bool d = true){
		DOTween.Kill(grp);

		float opacity = defaultOpacity;
		if(!d){
			opacity = 1f;
		}

		if(visibleOnStop){
			grp.DOFade(opacity, 0.2f);
		} else {
			grp.DOFade(0f, 0.2f);
		}
	}

	public void TempFlash()
	{
		float delay = 0f;
		DOTween.Kill(grp);

		if(randomDelay)
		{
			delay = Random.Range(0f, 0.5f);
		}

		grp.alpha = min;
		grp.DOFade(max, speed).SetLoops(10, LoopType.Yoyo).SetDelay(delay);
	}
}
