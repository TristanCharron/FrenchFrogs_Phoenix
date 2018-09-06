using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputAI : InputBase
{
    Dictionary<int, float> axisDictionary = new Dictionary<int, float>();
    Dictionary<int, ButtonStats> buttonDictionary = new Dictionary<int, ButtonStats>();

    public override void Init(int id)
    {
        ID = id;
    }

    public void SetButton(int input, bool value)
    {
        if (buttonDictionary.ContainsKey(input))
        {
            if (value)
            {
                //Si last frame y'était press, devient Hold etc
                if (buttonDictionary[input] == ButtonStats.Down)
                    buttonDictionary[input] = ButtonStats.Hold;
                else if (buttonDictionary[input] == ButtonStats.None)
                    buttonDictionary[input] = ButtonStats.Down;
                else if (buttonDictionary[input] == ButtonStats.Up)
                    buttonDictionary[input] = ButtonStats.Down;
            }
            else
            {
                if (buttonDictionary[input] == ButtonStats.Down)
                    buttonDictionary[input] = ButtonStats.Up;
                else if (buttonDictionary[input] == ButtonStats.Hold)
                    buttonDictionary[input] = ButtonStats.Up;
                else if (buttonDictionary[input] == ButtonStats.Up)
                    buttonDictionary[input] = ButtonStats.None;
            }
        }
        else
        {
            buttonDictionary.Add(input, ButtonStats.Down);
        }
    }
    public void SetAxis(int input, float value)
    {
        if (axisDictionary.ContainsKey(input))
        {
            axisDictionary[input] = value;
        }
        else
        {
            axisDictionary.Add(input, value);
        }
    }

    public override float GetAxis(int input)
    {
        if (!isEnabled || !axisDictionary.ContainsKey(input))
            return 0;

        float value = axisDictionary[input];
        return value;
    }

    public override bool GetButton(int input)
    {
        if (!isEnabled || !axisDictionary.ContainsKey(input))
            return false;

        bool value = buttonDictionary[input] == ButtonStats.Hold;
        return value;
    }

    public override bool GetButtonDown(int input)
    {
        if (!isEnabled || !axisDictionary.ContainsKey(input))
            return false;

        bool value = buttonDictionary[input] == ButtonStats.Down;
        return value;
    }

    public override bool GetButtonUp(int input)
    {
        if (!isEnabled || !axisDictionary.ContainsKey(input))
            return false;

        bool value = buttonDictionary[input] == ButtonStats.Up;

        return value;
    }
}
