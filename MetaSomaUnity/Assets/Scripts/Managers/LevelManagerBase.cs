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
        viewportAnimator = GameManager.instance.HUD.transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void Setup()
    {
        // NOT IN USE RIGHT NOW
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
        viewportAnimator.enabled = true;
    }
}
