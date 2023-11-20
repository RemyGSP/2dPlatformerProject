using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Ink.Runtime;

public class DialogueTrigger : MonoBehaviour
{

    [Header("VisualFeedback")]
    [SerializeField] private GameObject visualFeedback;
    private bool isPlayerInRange;
    [SerializeField]private bool nonInteractableDialogue;

    
    [SerializeField] private TextAsset dialogue;
    private void Awake()
    {
        isPlayerInRange = false;
        visualFeedback.SetActive(false);
    }
    private void OnNextDialogue()
    {
        if (!nonInteractableDialogue)
        {
            if (isPlayerInRange)
            {
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                this.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!nonInteractableDialogue)
        {
            if (collision.tag == "Player")
            {
                isPlayerInRange = true;
                visualFeedback.SetActive(true);
            }
        }
        else
        {
            DialogueManager.GetInstance().EnterDialogueMode(dialogue);
            this.gameObject.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!nonInteractableDialogue)
        {
            if (collision.tag == "Player")
            {
                isPlayerInRange = false;
                visualFeedback.SetActive(false);
            }
        }
    }

}
