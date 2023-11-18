using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChangeText : MonoBehaviour
{

    [SerializeField] string text;
    [SerializeField] GameObject textDisplay;
    [SerializeField] private bool oneTimeText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        textDisplay.GetComponent<TextDisplayer>().ChangeTextDisplayed(text);
        textDisplay.GetComponent<TextDisplayer>().DisplayText();
        if (oneTimeText)
        {
            this.gameObject.SetActive(false);
        }
    }
}
