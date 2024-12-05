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
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // SETUP DIALOGUE RUNNER
        dialogueRunner = GameManager.instance.dialogueRunner;
        
        // SETUP IMERIS
        ImerisAnimation = dialogueRunner.gameObject.transform.GetChild(0).GetChild(2).gameObject;
        
        // ANIMATION
        viewportAnimator = GameManager.instance.HUD.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    
    public virtual void CutsScene()
    {
        dialogueRunner.StartDialogue(cutSceneDialogueNode);
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
    
    public void StopAnimationPlaying()
    {
        
    }
}
