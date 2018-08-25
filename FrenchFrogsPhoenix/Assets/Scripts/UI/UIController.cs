using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private UIController instance;

    [SerializeField] Text powerTxt;
    [SerializeField] Text speedTxt;
    [SerializeField] Text massTxt;
    [SerializeField] CanvasFader leftUIFader;
    [SerializeField] CanvasFader RightUIFader;

    // Use this for initialization
    void Start() {
        instance = this;
        DeactivateInGameUI();
    }

    // Update is called once per frame
    void Update() {

    }

    public void ActivateStartMenuUI()
    {

    }

    public void DeactivateStartMenuUI()
    {

    }

    public void ActivateInGameUI()
    {
        leftUIFader.GlitchIn();
        RightUIFader.GlitchIn();
    }

    public void DeactivateInGameUI()
    {
        leftUIFader.Hide();
        RightUIFader.Hide();
    }

    public UIController GetInstance()
    {
        return instance;
    }

    public void SetPower(float powerValue)
    {
        powerTxt.text = "Power: " + powerValue.ToString();
    }

    public void SetSpeed(float speedValue)
    {
        speedTxt.text = "Speed: " + speedValue.ToString();
    }

    public void SetMass(float massValue)
    {
        massTxt.text = "Mass: " + massValue.ToString();
    }
}
