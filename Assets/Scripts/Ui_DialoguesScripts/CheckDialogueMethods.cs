using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Events;

public class CheckDialogueMethods : MonoBehaviour
{
    Story story;
    public UnityEvent end;
    public void CheckDialogueTags(Story currentStory)
    {
        if (currentStory.currentTags[0] == "End")
        {
            end.Invoke();
        }
    }
}
