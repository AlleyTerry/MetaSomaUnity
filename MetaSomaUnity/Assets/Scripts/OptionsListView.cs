/*
Yarn Spinner is licensed to you under the terms found in the file LICENSE.md.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
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
                if (Input.GetKeyDown(KeyCode.UpArrow) && optionViews.Count > 1 && optionViews[1].gameObject.activeSelf && currentSelectedIndex != 1)
                {
                    ChangeSelection(1); // Index 1 is mapped to 'up' option
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && optionViews.Count > 3 && optionViews[3].gameObject.activeSelf && currentSelectedIndex != 3)
                {
                    ChangeSelection(3); // Index 3 is mapped to 'down' option
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && optionViews.Count > 2 && optionViews[2].gameObject.activeSelf && currentSelectedIndex != 2)
                {
                    ChangeSelection(2); // Index 2 is mapped to 'left' option
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && optionViews.Count > 0 && optionViews[0].gameObject.activeSelf && currentSelectedIndex != 0)
                {
                    ChangeSelection(0); // Index 0 is mapped to 'right' option
                }
                else if (Input.GetKeyDown(KeyCode.Return))
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
            
            if (newIndex >= 0 && newIndex < optionViews.Count)
            {
                // Deactivate the current option
                //DeselectOption(currentSelectedIndex);

                // Update the selected index
                currentSelectedIndex = newIndex;

                // Activate the new option
                optionViews[currentSelectedIndex].Select();
                // Update Unity EventSystem's current selection to avoid conflict
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(optionViews[currentSelectedIndex].gameObject, null);
            }
        }

        private void ConfirmSelection()
        {
            Debug.Log("Confirming selection.");
            //var selectedOption = optionViews[currentSelectedIndex].Option;
            //OptionViewWasSelected(selectedOption);
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
            
            canvasGroup.blocksRaycasts = true;
            
            // If we don't already have enough option views, create more
            while (dialogueOptions.Length > optionViews.Count)
            {
                var optionView = CreateNewOptionView();
                optionView.gameObject.SetActive(false);
            }

            // Set up all of the option views
            int optionViewsCreated = 0;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = optionViews[i];
                var option = dialogueOptions[i];

                if (option.IsAvailable == false && showUnavailableOptions == false)
                {
                    // Don't show this option.
                    continue;
                }

                optionView.gameObject.SetActive(true);

                optionView.palette = this.palette;
                optionView.Option = option;

                // The first available option is selected by default
                if (optionViewsCreated == 0)
                {
                    optionView.Select();
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

            // Note the delegate to call when an option is selected
            OnOptionSelected = onOptionSelected;

            // sometimes (not always) the TMP layout in conjunction with the
            // content size fitters doesn't update the rect transform
            // until the next frame, and you get a weird pop as it resizes
            // just forcing this to happen now instead of then
            Relayout();

            // Fade it all in
            StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));
            
            // Prevent mouse clicks from changing selection
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(optionViews[currentSelectedIndex].gameObject);

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

        public void OnEnable()
        {
            Relayout();
        }

        private void Relayout()
        {
            // Get the number of options
            int optionCount = optionViews.Count;
            if (optionCount == 0) return;

            // Calculate the angle between each option
            float angleStep = 360f / optionCount;
            float radius = 300f; // Adjust the radius as needed

            for (int i = 0; i < optionCount; i++)
            {
                // Calculate the position of each option
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                // Set the position of the option
                RectTransform optionTransform = optionViews[i].GetComponent<RectTransform>();
                optionTransform.anchoredPosition = new Vector2(x, y);

                // Customize the appearance of each option
                //CustomizeOptionAppearance(optionViews[i], i);
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
