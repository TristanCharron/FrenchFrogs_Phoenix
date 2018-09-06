using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBar : MonoBehaviour {

    [SerializeField] Image fillImageBg;
    [SerializeField] Image fillImage;
    [SerializeField] Text ratioText;

    float delayBeforeUpdate = .5f;
    float timerBeforeUpdate = 0;

    void Start () {
		
	}

    void Update()
    {
        UpdateFillDelay();
    }

    void UpdateFillDelay()
    {
        timerBeforeUpdate += Time.deltaTime;
        if (timerBeforeUpdate > delayBeforeUpdate)
        {
            fillImageBg.DOKill();
            fillImageBg.DOFillAmount(fillImage.fillAmount, 0.3f).SetEase(Ease.InQuint);

            timerBeforeUpdate = 0;
        }
    }

    public void UpdateFill(float fillValue)
    {
        //Si le fuel monte automatiquement, on met pas de delay avec le background
        if (fillValue > fillImage.fillAmount)
            fillImageBg.fillAmount = fillValue;
        else
            timerBeforeUpdate = 0;

        fillImage.fillAmount = fillValue;
        UpdatePourcentageText(fillValue);
    }

    void UpdatePourcentageText(float fuelValue)
    {
        int roundValue = Mathf.RoundToInt(fuelValue * 100);

        string stringValue = roundValue.ToString() + "%";
        /*if (roundValue < 100)
            stringValue = "0" + stringValue;
        if (roundValue < 10)
            stringValue = "0" + stringValue;*/

        ratioText.text = stringValue;
    }
}
