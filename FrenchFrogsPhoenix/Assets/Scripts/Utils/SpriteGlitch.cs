using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteGlitch : MonoBehaviour {

	public float min = 0f;
	public float max = 1f;
	public float speed = 0.1f;

	SpriteRenderer spriteRenderer;
	Color color = Color.white;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		Anim();
	}

	void Anim(){
		color = spriteRenderer.color;
		color.a = Random.Range(min,max);

		spriteRenderer.DOColor(color, speed).OnComplete(Anim);
	}
}
