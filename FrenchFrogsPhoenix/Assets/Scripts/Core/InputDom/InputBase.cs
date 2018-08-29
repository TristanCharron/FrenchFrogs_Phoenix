using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class InputBase
{
    protected enum ButtonStats { None, Up, Down, Hold };
    SortedList<int, ButtonStats> buttonInvokeList = new SortedList<int, ButtonStats>();

    public const string ButtonDown = "ButtonDown_";
    public const string ButtonUp = "ButtonUp_";
    public const string ButtonHold= "ButtonHold_";
    public const string Axis = "Axis_";

    protected int ID;
    public abstract void Init(int id);

    public abstract bool GetButtonDown(int button);
    public abstract bool GetButtonUp(int button);
    public abstract bool GetButton(int button);

    public abstract float GetAxis(int button);

    protected bool isEnabled; 

    public void SetActive(bool value)
    {
        isEnabled = value;
    }

    protected void InvokeButtonDownEvent(int idInput)
    {
        string eventName = ButtonDown + ID + "_" + idInput;
        Debug.Log("Invoking eventName " + eventName);
        EventManager.Invoke(eventName);
    }
    protected void InvokeButtonUpEvent(int idInput)
    {
        EventManager.Invoke(ButtonHold + ID + "_" + idInput);
    }
    protected void InvokeButtonHoldEvent(int idInput)
    {
        EventManager.Invoke(ButtonHold + ID + "_" + idInput);
    }
    protected void InvokeAxisEvent(int idInput, float value)
    {
        EventManager.Invoke<float>(Axis + ID + "_" + idInput, value);
    }

    #region subscribe 
    public void SubscribeButtonUp(int idInput, Action callback)
    {
        InternalSubscribe(idInput, ButtonStats.Up, callback);
    }

    public void SubscribeButtonHold(int idInput, Action callback)
    {
        InternalSubscribe(idInput, ButtonStats.Up, callback);
    }

    public void SubscribeButtonDown(int idInput, Action callback)
    {
        InternalSubscribe(idInput, ButtonStats.Down, callback);
    }

    void InternalSubscribe(int idInput, ButtonStats buttonStats, Action callback)
    {
        buttonInvokeList.Add(idInput, buttonStats);
        string eventName = GetEventName(idInput, buttonStats);
        EventManager.Subscribe(eventName, callback);
    }
    #endregion

    #region unsub
    public void UnubscribeButtonUp(int idInput, Action callback)
    {
        InternalSubscribe(idInput, ButtonStats.Up, callback);
    }

    public void UnsubscribeButtonHold(int idInput, Action callback)
    {
        InternalSubscribe(idInput, ButtonStats.Up, callback);
    }

    public void UnsubscribeButtonDown(int idInput, Action callback)
    {
        InternalSubscribe(idInput, ButtonStats.Down, callback);
    }

    void InternalUnsubscribe(int idInput, ButtonStats buttonStats, Action callback)
    {
        //might bug
        //buttonInvokeList.Remove(idInput);
        string eventName = GetEventName(idInput, buttonStats);
        EventManager.Unsubscribe(eventName, callback);
    }

    #endregion
    string GetEventName(int idInput, ButtonStats buttonStats)
    {
        string buttonType = "";
        switch (buttonStats)
        {
            case ButtonStats.Down:
                buttonType = ButtonDown; break;
            case ButtonStats.Up:
                buttonType = ButtonUp; break;
            case ButtonStats.Hold:
                buttonType = ButtonHold; break;
        }

        return buttonType + ID + "_" + idInput;
    }

    public void Update()
    {
        if (buttonInvokeList.Count == 0)
            return;

        IList<int> keys = buttonInvokeList.Keys;
        for (int i = 0; i < keys.Count; i++)
        {
            int key = keys[i];
            if (buttonInvokeList[key] == ButtonStats.Up && GetButtonUp(key))
                InvokeButtonUpEvent(key);

            else if (buttonInvokeList[key] == ButtonStats.Down && GetButtonDown(key))
                InvokeButtonDownEvent(key);

            else if(buttonInvokeList[key] == ButtonStats.Hold && GetButton(key))
                InvokeButtonHoldEvent(key);
        }
    }
}
