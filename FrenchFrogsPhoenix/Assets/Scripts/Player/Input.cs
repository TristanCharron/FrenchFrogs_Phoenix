 using System;
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
        OnPress(X,Y);
    }
}




public abstract class BaseInput
{
    protected bool isActive = false;

    public InputButton FireButton { protected set; get; }
    public InputStick LeftStick { protected set; get; }
    public InputStick RightStick  { protected set; get; }

    public abstract void Update();

    public BaseInput()
    {
        FireButton = new InputButton("Fire", RewiredConsts.Action.Fire);
        LeftStick = new InputStick();
        RightStick = new InputStick();
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}


public class PlayerInput : BaseInput
{
    int ID = 0;

    public override void Update()
    {
        
    }

    public PlayerInput(int ID) : base()
    {



        EventManager.Subscribe<InputActionEventData>(RewiredInputProvider.EVT_INPUT_PRESS_DOWN, (input) =>
        {


            if (isActive)
            {
                switch (input.actionId)
                {
                    case RewiredConsts.Action.Fire:
                        FireButton.Press();
                        break;
                }

            }

        });
        EventManager.Subscribe<InputActionEventData>(RewiredInputProvider.EVT_INPUT_PRESS, (input) =>
        {
            if(isActive)
            {
                switch(input.actionId)
                {
                    case RewiredConsts.Action.MoveHorizontal:
                        LeftStick.SetX(input.GetAxis());
                        LeftStick.Press();
                        break;
                    case RewiredConsts.Action.MoveVertical:
                        LeftStick.SetY(input.GetAxis());
                        LeftStick.Press();
                        break;
                    case RewiredConsts.Action.CameraHorizontal:
                        RightStick.SetX(input.GetAxis());
                        RightStick.Press();
                        break;
                    case RewiredConsts.Action.CameraVertical:
                        RightStick.SetY(input.GetAxis());
                        RightStick.Press();
                        break;
                }
                 
            }
             
        });
    }

    
}
