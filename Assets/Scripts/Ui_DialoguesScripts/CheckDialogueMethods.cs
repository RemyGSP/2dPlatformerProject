using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CheckDialogueMethods : MonoBehaviour
{
    Story story;
    public UnityEvent end;
    public UnityEvent tutorial;
    public void CheckDialogueTags(string key, string value)
    {
        Debug.Log(key + " " + value);
        if (key  == "Method" && value == "End")
        {
            end.Invoke();
        }
        if (key == "method" && value == "Tutorial")
        {
            tutorial.Invoke();
        }
    }
}
