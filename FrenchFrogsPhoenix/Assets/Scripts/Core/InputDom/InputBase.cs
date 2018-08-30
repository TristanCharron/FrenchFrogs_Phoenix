using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public abstract class InputBase
{
    protected enum ButtonStats { None, Up, Down, Hold };
    Dictionary<InputKey, ButtonStats> buttonEventDictionary = new Dictionary<InputKey, ButtonStats>();
    List<InputKey> listKeys = new List<InputKey>();

  //  UnityEvent ButtonDownEvent = new UnityEvent();
  //  UnityEvent ButtonHoldEvent = new UnityEvent();

    //Dictionary<InputKey, ButtonStats> buttonDictionaryUp = new Dictionary<InputKey, ButtonStats>();
    //Dictionary<InputKey, ButtonStats> buttonDictionaryDown = new Dictionary<InputKey, ButtonStats>();
    //Dictionary<InputKey, ButtonStats> buttonDictionaryHold = new Dictionary<InputKey, ButtonStats>();
    //Dictionary<int, Action> floatList = new Dictionary<int, Action>();

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
        InternalSubscribe(idInput, callback, ButtonStats.Up);
    }

    public void SubscribeButtonHold(int idInput, Action callback)
    {
        InternalSubscribe(idInput, callback, ButtonStats.Hold);
    }

    public void SubscribeButtonDown(int idInput, Action callback)
    {
        InternalSubscribe(idInput, callback, ButtonStats.Down);
    }

    void InternalSubscribe(int idInput, Action callback, ButtonStats buttonStats)
    {
        InputKey key = new InputKey(idInput, callback);
        listKeys.Add(key);
        buttonEventDictionary.Add(key, buttonStats);

        string eventName = GetEventName(idInput, buttonStats);
        EventManager.Subscribe(GetEventName(idInput, buttonStats), callback);
    }
    #endregion

    #region unsub
    public void UnubscribeButtonUp(int idInput, Action callback)
    {
        InternalUnsubscribe(idInput, ButtonStats.Up, callback);
    }

    public void UnsubscribeButtonHold(int idInput, Action callback)
    {
        InternalUnsubscribe(idInput, ButtonStats.Up, callback);
    }

    public void UnsubscribeButtonDown(int idInput, Action callback)
    {
        InternalUnsubscribe(idInput, ButtonStats.Down, callback);
    }

    void InternalUnsubscribe(int idInput, ButtonStats buttonStats, Action callback)
    {
        //might bug, si ca bug, faire de la reflexion et save la methode du callback
        InputKey key = new InputKey(idInput, callback);
        listKeys.Remove(key);
        buttonEventDictionary.Remove(key);

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
        //if (buttonInvokeList.Count == 0)
        //    return;

        //IList<int> keys = buttonInvokeList.Keys;
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    int key = keys[i];
        //    if (buttonInvokeList[key] == ButtonStats.Up && GetButtonUp(key))
        //        InvokeButtonUpEvent(key);

        //    else if (buttonInvokeList[key] == ButtonStats.Down && GetButtonDown(key))
        //        InvokeButtonDownEvent(key);

        //    else if(buttonInvokeList[key] == ButtonStats.Hold && GetButton(key))
        //        InvokeButtonHoldEvent(key);
        //}

        InvokeDictionaryButton();
        //InvokeDictionaryButton(buttonDictionaryUp);
        //InvokeDictionaryButton(buttonDictionaryHold);
    }

    void InvokeDictionaryButton()
    {
        if (buttonEventDictionary.Count == 0)
            return;

        IList<InputKey> keys = listKeys;
        for (int i = 0; i < keys.Count; i++)
        {
            InputKey key = keys[i];
            if (buttonEventDictionary[key] == ButtonStats.Up && GetButtonUp(key.keyInt))
                InvokeButtonUpEvent(key.keyInt);

            if (buttonEventDictionary[key] == ButtonStats.Down && GetButtonDown(key.keyInt))
                InvokeButtonDownEvent(key.keyInt);

            if (buttonEventDictionary[key] == ButtonStats.Hold && GetButton(key.keyInt))
                InvokeButtonHoldEvent(key.keyInt);
        }
    }

    public class InputKey
    {
        public int keyInt;
        public Action keyCallBack;

        public InputKey(int keyInt, Action keyCallBack)
        {
            this.keyInt = keyInt;
            this.keyCallBack = keyCallBack;
        }
    }
}
