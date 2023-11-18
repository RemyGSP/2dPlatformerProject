using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextDisplayer : MonoBehaviour
{
    [SerializeField] public float  writtingSpeed = 0.05f;
    private static TextMeshProUGUI text;
    private Queue allTexts;
    [SerializeField] private string completeText;
    [SerializeField] TextMeshProUGUI textToChange;

    void Start()
    {
        text = textToChange.GetComponent<TextMeshProUGUI>();
        text.text = ""; // Limpiamos el texto inicialmente
        StartCoroutine(EscribirTexto());
    }

    public void DisplayText()
    {
        StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        for (int i = 0; i <= completeText.Length; i++)
        {
            text.text = completeText.Substring(0, i);
            yield return new WaitForSeconds(writtingSpeed);
        }
    }

    public void ChangeTextDisplayed(string newText)
    {
        completeText = newText;
    }
}
