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
    /*protected DialogueRunner dialogueRunner;*/
    
    // IMERIS
    public GameObject ImerisAnimation;
    
    // DIALOGUE NODES
    /*public string cutSceneDialogueNode = "";
    public string battleDialogueNode = "";*/
    
    // BATTLE TRIGGER
    protected string currentCutSceneDialogueNode;
    protected string currentBattleDialogueNode;
    public float currentBattleDialogueDelay = 0.5f;
    
    // ANIMATION
    /*public Animator viewportAnimator;*/

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // SETUP IMERIS
        ImerisAnimation = GameObject.Find("ImerisAnimation");
        
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
    
    public virtual void RegisterCutSceneAndBattle(string cutSceneDialogueNode, string battleDialogueNode, float delayTime)
    {
        currentCutSceneDialogueNode = cutSceneDialogueNode;
        currentBattleDialogueNode = battleDialogueNode;
        currentBattleDialogueDelay = delayTime;
        
        Debug.Log($"Cut Scene and Battle registered: {cutSceneDialogueNode}, {battleDialogueNode}");
        
        // START CUT SCENE
        StartCutsScene(currentCutSceneDialogueNode);
    }
    
    // CUT SCENE
    public virtual void StartCutsScene(string cutSceneDialogueNode)
    {
        Debug.Log($"Cut Scene Started with node: {cutSceneDialogueNode}");
        
        // START DIALOGUE
        DialogueManager.instance.StartDialogue(cutSceneDialogueNode);
        
        // PAUSE HUNGER
        GameObject.FindObjectOfType<ImerisHunger>().PauseHungerMeter();
        
        // FREEZE CONTROLS
        Invoke(nameof(GameManager.instance.FreezeControls), 0.5f);
    }
    
    // BATTLE SCENE
    public virtual void StartBattleScene()
    {
        StartCoroutine(DelayedStartBattleDialogue(currentBattleDialogueNode, currentBattleDialogueDelay));
    }
    
    private IEnumerator DelayedStartBattleDialogue(string battleDialogueNode, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        
        StartBattleDialogue(battleDialogueNode); // Start the actual battle dialogue
    }
    
    private void StartBattleDialogue(string battleDialogueNode)
    {
        Debug.Log($"Battle Scene Started with node: {battleDialogueNode}");
        
        // START DIALOGUE
        DialogueManager.instance.StartDialogue(battleDialogueNode);
        
        // PAUSE HUNGER
        GameObject.FindObjectOfType<ImerisHunger>().PauseHungerMeter();
        
        // FREEZE CONTROLS
        Invoke(nameof(GameManager.instance.FreezeControls), 0.5f);
    }

    public virtual void ExitBattleDialogue()
    {
        DialogueManager.instance.StopDialogue();
        UIManager.instance.EnableAnimator();
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
