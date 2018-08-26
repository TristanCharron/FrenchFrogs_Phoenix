using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Glitch : MonoBehaviour {

	public float min = 0f;
	public float max = 1f;
	public float interval;
	public float speed;
	public bool playOnStart;

	private bool glitching = false;
	private float delay;
	private CanvasGroup grp;
    private CanvasRenderer canvasRender;
	private float opacity;
	private float timer;

    void Start(){
		grp = GetComponent<CanvasGroup>();
        canvasRender = GetComponent<CanvasRenderer>();

        if (playOnStart){
			StartGlitch();
		} else {
			grp.alpha = 0f;
		}
	}

	void Update(){
		if(glitching){

            timer += Time.deltaTime;
			grp.alpha = Mathf.Lerp(grp.alpha, opacity, Time.deltaTime * speed);

			if(delay < timer){
				opacity = Random.Range(min, max);
				delay = Random.Range(0f, interval);
				timer = 0f;
			}
		}
	}

	void OnDestroy(){
		DOTween.Kill(grp);
	}

	public void Show(float delay = 0f){
        gameObject.SetActive(true);
        grp.alpha = 0f;
        canvasRender.SetAlpha(1);

		DOTween.Kill(grp);
		grp.DOFade(1f, 0.05f).SetLoops(3, LoopType.Yoyo).SetDelay(delay).OnComplete(StartGlitch);
	}

	public void Hide(float delay = 0f){
		StopGlitch();

		if(grp != null){
	        grp.alpha = 1f;

			DOTween.Kill(grp);
			grp.DOFade(0f, 0.05f).SetLoops(3, LoopType.Yoyo).SetDelay(delay);
        	canvasRender.SetAlpha(0);
        	gameObject.SetActive(false);
        }
    }

	public void StartGlitch(){
		glitching = true;
	}

	public void StopGlitch(){
		glitching = false;
		if(grp != null){
			grp.alpha = 0f;
		}
	}
}
