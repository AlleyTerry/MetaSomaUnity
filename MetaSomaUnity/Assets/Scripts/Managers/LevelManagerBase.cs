using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class LevelManagerBase : MonoBehaviour
{
    // DIALOGUE RUNNER
    protected DialogueRunner dialogueRunner;
    
    // IMERIS
    public GameObject ImerisAnimation;
    
    // DIALOGUE NODES
    public string cutSceneDialogueNode = "";
    public string battleDialogueNode = "";
    
    // BATTLE TRIGGER
    public GameObject battleTrigger;
    public bool isBattleTriggered = false;
    
    // ANIMATION
    public Animator viewportAnimator;

    private void Awake()
    {
        // SETUP DIALOGUE RUNNER
        GameManager.instance.GetDialogueRunner();
        dialogueRunner = GameManager.instance.dialogueRunner;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // SETUP IMERIS
        ImerisAnimation = GameObject.Find("ImerisAnimation");
        
        // ANIMATION
        /*viewportAnimator = GameManager.instance.HUD.transform.GetChild(0).gameObject.GetComponent<Animator>();*/
        
        if (GameManager.instance.HUD != null)
        {
            Debug.Log("HUD found in GameManager.");
            viewportAnimator = GameManager.instance.HUD.transform.GetChild(0).gameObject.GetComponent<Animator>();
            if (viewportAnimator != null)
            {
                Debug.Log("viewportAnimator successfully assigned in Start.");
            }
            else
            {
                Debug.LogError("viewportAnimator is null in Start! Check if HUD's first child has an Animator component.");
            }
        }
        else
        {
            Debug.LogError("HUD is null in GameManager! viewportAnimator cannot be assigned.");
        }
        
        Debug.Log("LevelManagerBase.Start finished.");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void Initialize()
    {
        Debug.Log($"{GetType().Name} initialized.");
    }
    
    public virtual void CutsScene()
    {
        dialogueRunner.StartDialogue(cutSceneDialogueNode);
        
        // PAUSE HUNGER
        GameObject.FindObjectOfType<ImerisHunger>().PauseHungerMeter();
        
        // FREEZE CONTROLS
        Invoke(nameof(GameManager.instance.FreezeControls), 0.5f);
    }
    
    public virtual void BattleScene()
    {
        Invoke(nameof(StartBattleDialogue), 0.5f);
    }
    
    private void StartBattleDialogue()
    {
        dialogueRunner.StartDialogue(battleDialogueNode);
        
        // PAUSE HUNGER
        GameObject.FindObjectOfType<ImerisHunger>().PauseHungerMeter();
        
        // FREEZE CONTROLS
        Invoke(nameof(GameManager.instance.FreezeControls), 0.5f);
    }
    
    public void DisableAnimator()
    {
        viewportAnimator.enabled = false;
    }

    public virtual void ExitBattleDialogue()
    {
        dialogueRunner.Stop();
        viewportAnimator.enabled = true;
    }

    public virtual void DeadScene()
    {
        if (GameManager.instance.CurrentGameState == GameState.IsDead)
        {
            ImerisAnimation.SetActive(false);
        }
        else
        {
            Debug.LogWarning("DeadScene called but CurrentGameState is not IsDead.");
        }
    }
}
