/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using TextMeshProUGUI = TMPro.TextMeshProUGUI;

namespace Yarn.Unity
{
    public class OptionsListView : DialogueViewBase
    {
        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] OptionView optionViewPrefab;

        [SerializeField] MarkupPalette palette;

        [SerializeField] float fadeTime = 0.1f;

        [SerializeField] bool showUnavailableOptions = false;

        [Header("Last Line Components")]
        [SerializeField] TextMeshProUGUI lastLineText;
        [SerializeField] GameObject lastLineContainer;

        [SerializeField] TextMeshProUGUI lastLineCharacterNameText;
        [SerializeField] GameObject lastLineCharacterNameContainer;

        // A cached pool of OptionView objects so that we can reuse them
        [SerializeField]List<OptionView> optionViews = new List<OptionView>();

        // The method we should call when an option has been selected.
        Action<int> OnOptionSelected;

        // The line we saw most recently.
        LocalizedLine lastSeenLine;
        
        // Current selected option index
        private int currentSelectedIndex = 0;
        
        // option sprites
        [SerializeField] private Sprite optionSpriteUp;
        [SerializeField] private Sprite optionSpriteLeft;
        [SerializeField] private Sprite optionSpriteRight;

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
            // Set hover text canvas invisible
            hoverText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        public void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }
        
        private void Update()
        {
            // Check for arrow keys to navigate between options
            if (canvasGroup.interactable)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) //  Block down arrow
                {
                    Debug.Log("DownArrow is disabled, maintaining current selection.");
                    if (currentSelectedIndex >= 0)
                    {
                        EventSystem.current.SetSelectedGameObject(optionViews[currentSelectedIndex].gameObject, null);
                    }
                    return;
                }
                
                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
                    && optionViews[0].gameObject.activeSelf)
                {
                    ChangeSelection(0); // Index 0 is mapped to 'up' option
                }
                else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
                         && optionViews[1].gameObject.activeSelf)
                {
                    ChangeSelection(1); // Index 1 is mapped to 'left' option
                }
                else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) 
                         && optionViews[2].gameObject.activeSelf)
                {
                    ChangeSelection(2); // Index 2 is mapped to 'right' option
                }
                else if (Input.GetKeyDown(KeyCode.Return) && currentSelectedIndex >= 0)
                {
                    ConfirmSelection();
                }
            }
        }
        
        private void ChangeSelection(int newIndex)
        {
            if (newIndex < 0 || 
                newIndex >= optionViews.Count || 
                !optionViews[newIndex].gameObject.activeSelf)
            {
                Debug.Log("Invalid selection index or inactive option, skipping selection change.");
                return;
            }

            if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) return;
            
            // Deactivate the current option
            //DeselectOption(currentSelectedIndex);

            // Update the selected index
            currentSelectedIndex = newIndex;

            // Activate the new option
            optionViews[currentSelectedIndex].Select();
            
            // Update Unity EventSystem's current selection to avoid conflict
            if (currentSelectedIndex >= 0)
            {
                EventSystem.current.SetSelectedGameObject(optionViews[currentSelectedIndex].gameObject, null);
            }
            
            // Update hover text
            UpdateHoverText(currentSelectedIndex);
        }
        
        // HOVER TEXT
        Dictionary<string, string> hoverTexts = new Dictionary<string, string>();

        [YarnCommand ("SetHoverText")]
        public void SetHoverText ()
        {
            hoverTexts.Clear(); // Reset the dictionary

            InMemoryVariableStorage variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
            if (variableStorage == null)
            {
                Debug.LogError("InMemoryVariableStorage not found!");
                return;
            }
            
            variableStorage.TryGetValue("$hover_text_up", out string text_up);
            variableStorage.TryGetValue("$hover_text_left", out string text_left);
            variableStorage.TryGetValue("$hover_text_right", out string text_right);
            //Debug.LogWarning(text_up + "\n" + text_left + "\n" + text_right);
            
            if (!string.IsNullOrEmpty(text_up)) hoverTexts["up"] = text_up;
            if (!string.IsNullOrEmpty(text_left)) hoverTexts["left"] = text_left;
            if (!string.IsNullOrEmpty(text_right)) hoverTexts["right"] = text_right;

            Debug.Log("Hover texts set: " + string.Join(", ", hoverTexts));
        }
        
        // Actual deal with hover text
        public TextMeshProUGUI hoverText;

        private void UpdateHoverText(int optionIndex)
        {
            string key = ""; // this is the key to the hover text dictionary
            
            switch (optionIndex)
            {
                case 0: key = "up"; break;
                case 1: key = "left"; break;
                case 2: key = "right"; break;
                default: key = ""; break;
            }
            
            if (hoverTexts.ContainsKey(key))
            {
                hoverText.gameObject.transform.parent.gameObject.SetActive(true); // Show the hover text canvas
                hoverText.text = hoverTexts[key];
            }
            else
            {
                hoverText.text = "";
                hoverText.gameObject.transform.parent.gameObject.SetActive(false); // Hide the hover text canvas
            }
        }

        private string lastChosenOption = "reason"; // Store the last chosen option
        
        private void ConfirmSelection()
        {
            Debug.Log("Confirming selection.");
            hoverText.gameObject.transform.parent.gameObject.SetActive(false); // Hide the hover text canvas
            
            // call level manager to switch animation state
            var levelManager = GameManager.instance.currentLevelManager;
            if (levelManager == null) GameObject.FindObjectOfType<LevelManagerBase>();

            if (levelManager == null ||
                !levelManager.isBattleAnimationChanging)
            {
                Debug.LogError("LevelManager is null! Cannot switch animation state.");
                return;
            }
            else
            {
                switch (currentSelectedIndex)
                {
                    case 0: // index 0, up, reason
                        if (levelManager is LevelManager_Chapel ||
                            levelManager is LevelManager_Cafeteria)
                        {
                            if (lastChosenOption == "challenge")
                            {
                                levelManager.PlayImerisAnimation("ImerisBattle_ChallengeToReason");
                            }
                            else if (lastChosenOption == "submission")
                            {
                                levelManager.PlayImerisAnimation("ImerisBattle_SubmissionToReason");
                            }
                        }
                        else
                        {
                            Debug.LogError("Cannot find level manager when switching battle animation state.");
                        }
                        
                        lastChosenOption = "reason"; // Store the last chosen option
                        break;
                    
                    case 1: // index 1, left, submission
                        if (levelManager is LevelManager_Chapel ||
                            levelManager is LevelManager_Cafeteria)
                        {
                            if (lastChosenOption == "reason")
                            {
                                levelManager.PlayImerisAnimation("ImerisBattle_TransitionToSubmission");
                            }
                            else if (lastChosenOption == "challenge")
                            {
                                levelManager.PlayImerisAnimation("ImerisBattle_ChallengeToSubmission");
                            }
                        }
                        else
                        {
                            Debug.LogError("Cannot find level manager when switching battle animation state.");
                        }
                        
                        lastChosenOption = "submission"; // Store the last chosen option
                        break;
                    
                    case 2: // index 2, right, challenge
                        if (levelManager is LevelManager_Chapel ||
                            levelManager is LevelManager_Cafeteria)
                        {
                            if (lastChosenOption == "reason")
                            {
                                levelManager.PlayImerisAnimation("ImerisBattle_TransitionToChallenge");
                            }
                            else if (lastChosenOption == "submission")
                            {
                                levelManager.PlayImerisAnimation("ImerisBattle_SubmissionToChallenge");
                            }
                        }
                        else
                        {
                            Debug.LogError("Cannot find level manager when switching battle animation state.");
                        }
                        
                        lastChosenOption = "challenge"; // Store the last chosen option
                        break;
                    
                    default:
                        Debug.LogError("Invalid option index selected.");
                        break;
                }
            }
        }
        
        [YarnCommand("ResetImerisAnimation")]
        public void ResetImerisAnimation()
        {
            switch (lastChosenOption)
            {
                case "reason":
                    // do nothing
                    break;
                case "submission":
                    // Reset
                    GameManager.instance.currentLevelManager.
                        PlayImerisAnimation("ImerisBattle_SubmissionToReason");
                    break;
                case "challenge":
                    // Reset to challenge animation
                    GameManager.instance.currentLevelManager.
                        PlayImerisAnimation("ImerisBattle_ChallengeToReason");
                    break;
                default:
                    Debug.LogError("Invalid last chosen option.");
                    break;
            }
            
            lastChosenOption = "reason"; // Reset the last chosen option
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Don't do anything with this line except note it and
            // immediately indicate that we're finished with it. RunOptions
            // will use it to display the text of the previous line.
            lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }
        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            Debug.Log($"RunOptions called, {dialogueOptions.Length} options available.");
            
            canvasGroup.alpha = 1;
            
            canvasGroup.blocksRaycasts = true;

            int maxOptions = 3; // Limit to 3 options
            
            // If we don't already have enough option views, create more
            while (optionViews.Count < maxOptions)
            {
                var optionView = CreateNewOptionView();
                optionView.gameObject.SetActive(false);
            }

            int[] optionOrder = new[] { 0, 1, 2 }; // Order of options
            
            // Set up all of the option views
            int optionViewsCreated = 0;

            /*for (int i = 0; i < dialogueOptions.Length; i++)*/ // original
            for (int i = 0; i < maxOptions; i++)
            {
                int optionIndex = optionOrder[i];
                
                if (optionIndex >= dialogueOptions.Length) break;
                
                var optionView = optionViews[i];
                var option = dialogueOptions[optionIndex];

                if (!option.IsAvailable && !showUnavailableOptions)
                {
                    // Don't show this option.
                    continue;
                }

                optionView.gameObject.SetActive(true);

                optionView.palette = this.palette;
                optionView.Option = option;

                /*// The first available option is selected by default
                if (optionViewsCreated == 0)
                {
                    optionView.Select();
                }*/
                
                // set option sprite
                var image = optionView.GetComponent<UnityEngine.UI.Image>();
                
                switch (i)
                {
                    case 0: image.sprite = optionSpriteUp; break;
                    case 1: image.sprite = optionSpriteLeft; break;
                    case 2: image.sprite = optionSpriteRight; break;
                }

                optionViewsCreated += 1;
            }

            // Update the last line, if one is configured
            if (lastLineContainer != null)
            {
                if (lastSeenLine != null)
                {
                    // if we have a last line character name container
                    // and the last line has a character then we show the nameplate
                    // otherwise we turn off the nameplate
                    var line = lastSeenLine.Text;
                    if (lastLineCharacterNameContainer != null)
                    {
                        if (string.IsNullOrWhiteSpace(lastSeenLine.CharacterName))
                        {
                            lastLineCharacterNameContainer.SetActive(false);
                        }
                        else
                        {
                            line = lastSeenLine.TextWithoutCharacterName;
                            lastLineCharacterNameContainer.SetActive(true);
                            lastLineCharacterNameText.text = lastSeenLine.CharacterName;
                        }
                    }

                    if (palette != null)
                    {
                        lastLineText.text = LineView.PaletteMarkedUpText(line, palette);
                    }
                    else
                    {
                        lastLineText.text = line.Text;
                    }

                    lastLineContainer.SetActive(true);
                }
                else
                {
                    lastLineContainer.SetActive(false);
                }
            }

            for (int i = maxOptions; i < optionViews.Count; i++)
            {
                optionViews[i].gameObject.SetActive(false);
            }
            
            currentSelectedIndex = -1; // No option selected by default

            // Note the delegate to call when an option is selected
            OnOptionSelected = onOptionSelected;

            // sometimes (not always) the TMP layout in conjunction with the
            // content size fitters doesn't update the rect transform
            // until the next frame, and you get a weird pop as it resizes
            // just forcing this to happen now instead of then
            ReLayout();

            // Fade it all in
            StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));
            
            EventSystem.current.SetSelectedGameObject(null);

            /// <summary>
            /// Creates and configures a new <see cref="OptionView"/>, and adds
            /// it to <see cref="optionViews"/>.
            /// </summary>
            OptionView CreateNewOptionView()
            {
                var optionView = Instantiate(optionViewPrefab);
                optionView.transform.SetParent(transform, false);
                optionView.transform.SetAsLastSibling();

                optionView.OnOptionSelected = OptionViewWasSelected;
                optionViews.Add(optionView);

                return optionView;
            }
        }
        
        /// <summary>
        /// Called by <see cref="OptionView"/> objects.
        /// </summary>
        void OptionViewWasSelected(DialogueOption option)
        {
            StartCoroutine(OptionViewWasSelectedInternal(option));

            IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
            {
                yield return StartCoroutine(FadeAndDisableOptionViews(canvasGroup, 1, 0, fadeTime));
                Debug.Log("On Option Selected: " + selectedOption.DialogueOptionID);
                OnOptionSelected(selectedOption.DialogueOptionID);
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// If options are still shown dismisses them.
        /// </remarks>
        public override void DialogueComplete()
        {   
            // do we still have any options being shown?
            if (canvasGroup.alpha > 0)
            {
                StopAllCoroutines();
                lastSeenLine = null;
                OnOptionSelected = null;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                StartCoroutine(FadeAndDisableOptionViews(canvasGroup, canvasGroup.alpha, 0, fadeTime));
            }
        }

        /// <summary>
        /// Fades canvas and then disables all option views.
        /// </summary>
        private IEnumerator FadeAndDisableOptionViews(CanvasGroup canvasGroup, float from, float to, float fadeTime)
        {
            yield return Effects.FadeAlpha(canvasGroup, from, to, fadeTime);

            // Hide all existing option views
            foreach (var optionView in optionViews)
            {
                optionView.gameObject.SetActive(false);
            }
        }

        private void ReLayout()
        {
            if (optionViews.Count !=3) Debug.LogWarning("OptionViews count is not 3");
            
            float radius = 300f;
            
            Vector2[] positions = new Vector2[]
            {
                new Vector2(0, radius),  //up
                new Vector2(-radius, 0), //left
                new Vector2(radius, 0),  //right
            };

            for (int i = 0; i < 3; i++)
            {
                RectTransform optionTransform = optionViews[i].GetComponent<RectTransform>();
                optionTransform.anchoredPosition = positions[i];
            }
        }
        private void CustomizeOptionAppearance(OptionView optionView, int index)
        {
            // Example customization: change the size and color based on the index
            float scale = 1f + (index * 0.1f); // Increase size for each subsequent option
            optionView.transform.localScale = new Vector3(scale, scale, scale);

            Color color = Color.Lerp(Color.red, Color.blue, (float)index / optionViews.Count); // Gradient from red to blue
            optionView.GetComponent<UnityEngine.UI.Image>().color = color;
        }
    }
}
