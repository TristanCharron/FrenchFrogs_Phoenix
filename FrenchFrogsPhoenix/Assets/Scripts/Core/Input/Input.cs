﻿ using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.Events;


public class InputButton
{
    public string Name { private set; get; }
    public int ID { private set; get; }

    Action OnPress;

    public InputButton(string Name, int ID)
    {
        this.Name = Name;
        this.ID = ID;
    }

    public void AddEvent(Action listener)
    {
        OnPress += listener;
    }

    public void Press()
    {
        if (OnPress != null)
            OnPress();
    }
}

public class InputStick
{
    public string Name { private set; get; }
    public int ID { private set; get; }
    public float X { private set; get; }
    public float Y { private set; get; }

    protected Action<float, float> OnPress;

    public InputStick()
    {
        X = 0;
        Y = 0;
    }

    public void SetX(float x)
    {
        X = x;
    }

    public void SetY(float y)
    {
        Y = y;
    }

    public void AddEvent(Action<float, float> listener)
    {
        OnPress += listener;
    }

    public void Press()
    {
        if(OnPress != null)
            OnPress(X,Y);
    }
}

public abstract class BaseInput
{
    protected bool isActive = false;

    protected Dictionary<string, InputButton> ButtonDictionnary;
    public InputButton FireButton { protected set; get; }
    public InputStick LeftStick { protected set; get; }
    public InputStick RightStick  { protected set; get; }
  
    public abstract void Update();

    public BaseInput()
    {
        ButtonDictionnary = new Dictionary<string, InputButton>();
        FireButton = new InputButton("Fire", RewiredConsts.Action.Fire);
        
        LeftStick = new InputStick();
        RightStick = new InputStick();
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    protected void AddButton(InputButton button)
    {
        ButtonDictionnary.Add(button.Name, button);
    }

    protected void PressButton(string buttonName)
    {
        if (ButtonDictionnary.ContainsKey(buttonName))
        {
            ButtonDictionnary[buttonName].Press();
        }
        else
            Debug.LogError(buttonName + "does not exist on button press");

    }

    public void PressLeftStick(float x, float y)
    {
        LeftStick.SetX(x);
        LeftStick.SetY(y);
        LeftStick.Press();
    }

    public void PressRightStick(float x, float y)
    {
        RightStick.SetX(x);
        RightStick.SetY(y);
        RightStick.Press();
    }
}


