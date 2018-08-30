using System;
using System.Collections.Generic;

public static class EventManager
{
    private static IDictionary<string, List<object>> subscribers = new Dictionary<string, List<object>>();

    public static void Subscribe<T>(string message, Action<T> callback)
    {
        if (subscribers.ContainsKey(message))
        {
            subscribers[message].Add(callback);
        }
        else
        {
            subscribers[message] = new List<object>();
            subscribers[message].Add(callback);
        }
    }

    public static void Subscribe(string message, Action callback)
    {
        if (subscribers.ContainsKey(message))
        {
            subscribers[message].Add(callback);
        }
        else
        {
            subscribers[message] = new List<object>();
            subscribers[message].Add(callback);
        }
    }

    public static void Invoke<T>(string message, T param)
    {

        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {

                Action<T> callback = (Action<T>)callbacks[i];

                callback(param);
            }
        }
    }

    public static void Invoke(string message)
    {

        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {

                Action callback = (Action)callbacks[i];

                callback();
            }
        }
    }

    public static void Unsubscribe<T>(string message, Action<T> callback)
    {
        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {

                Action<T> tmpCallback = (Action<T>)callbacks[i];

                if (tmpCallback == callback)
                {

                    callbacks.RemoveAt(i);

                    break;
                }
            }
        }
    }

    public static void Unsubscribe(string message, Action callback)
    {
        if (subscribers.ContainsKey(message))
        {
            List<object> callbacks = subscribers[message];

            for (int i = 0; i < callbacks.Count; i++)
            {
                Action tmpCallback = (Action)callbacks[i];

                if (tmpCallback == callback)
                {
                    callbacks.RemoveAt(i);
                    break;
                }
            }
        }
    }
}