using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    private CheckDialogueMethods checkMethods;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentStory;
    private bool dialogueIsPlaying;
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI characterName;

    [Header("Choices")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
            Debug.Log("Error");
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        if (InputCollector.instance.canSubmit)
        {
            if (currentStory.currentChoices.Count <= 0)
                ContinueDialogue();
            InputCollector.instance.canSubmit = false;
        }
    }
    private void Start()
    {
        checkMethods = GetComponent<CheckDialogueMethods>();
        instance = this;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }
    private void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            HandleTags(currentStory.currentTags);
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void ContinueDialogue(InputAction.CallbackContext value)
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            HandleTags(currentStory.currentTags);

            DisplayChoices();

        }
        else
        {
            ExitDialogueMode();
        }
    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        GameState.CanMove = false;
        GameState.canThrow = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueDialogue();

    }

    public void EnterDialogueMode(TextAsset inkJSON, bool StopPlayer)
    {
        if (StopPlayer)
        {
            GameState.CanMove = false;
            GameState.canThrow = false;
        }
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueDialogue();

    }

    public void ExitDialogueMode()
    {

        GameState.canThrow = true;
        GameState.CanMove = true;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices than UI can support :" + currentChoices.Count);
        }
        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        SelectFirstChoice();
    }
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }


    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueDialogue();
    }

    public void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            Debug.Log(splitTag[0] + " " + splitTag[1]);
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            else
            {
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();
                if (tagKey == "Speaker")
                {
                    DisplaySpeakerName(tagValue);
                }
                else
                    checkMethods.CheckDialogueTags(tagKey, tagValue);
            }

        }
    }

    public void DisplaySpeakerName(string name)
    {
        characterName.text = name;
    }
}
