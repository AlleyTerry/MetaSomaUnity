using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;
using Yarn;
using Yarn.Unity;

public class LevelManagerBase : MonoBehaviour
{
    // DIALOGUE RUNNER
    /*protected DialogueRunner dialogueRunner;*/
    
    // IMERIS
    public GameObject Imeris;
    public GameObject ImerisAnimation;
    
    // NPC
    public GameObject NPCAnimation;
    
    // BATTLE TRIGGER
    protected string currentCutSceneDialogueNode;
    protected string currentBattleDialogueNode;
    public float currentBattleDialogueDelay = 0.5f;
    
    // PLAYING CG
    [SerializeField] protected Animator CGDisplayAnimator;
    [SerializeField] protected RuntimeAnimatorController CGDisplayAnimatorController;
    
    // CAMERA
    public CinemachineVirtualCamera virtualCamera;
    public float damping = 1.0f;

    // BATTLE UI ANIMATION
    public bool isBattleAnimationChanging = false;
    
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("LevelManagerBase.Start finished.");
    }

    private IEnumerator RestoreDamping()
    {
        yield return new WaitForSeconds(0.1f);
        
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = damping;
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = damping;
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void Initialize()
    {
        // SETUP IMERIS
        if (SceneManager.GetActiveScene().name != "MainMenu" && 
            SceneManager.GetActiveScene().name != "Level_Intro")
        {
            Imeris = FindObjectOfType<ImerisMovement>().gameObject;
        }
        
        ImerisAnimation = GameObject.Find("ImerisAnimation");
        
        // SETUP NPC
        NPCAnimation = GameObject.Find("NPCAnimation");
        
        // CG PLAYER SETUP
        CGDisplayAnimator = transform.Find("CGDisplay").GetComponentInChildren<Animator>();
        CGDisplayAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/OpenCrawl/Open_Crawl");
        CGDisplayAnimator.runtimeAnimatorController = CGDisplayAnimatorController;
        
        Debug.Log($"{GetType().Name} initialized.");

        if (SceneManager.GetActiveScene().name != "MainMenu" && 
                SceneManager.GetActiveScene().name != "Level_Intro")
        {
            if (virtualCamera == null) virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            
        }
        if (virtualCamera != null)
        {
            virtualCamera.OnTargetObjectWarped(Imeris.transform, new Vector3(0, 2, 0));
            
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0.0f;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0.0f;

            StartCoroutine(RestoreDamping());
        }
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
        StartCoroutine(DelayedFreezeControls());
    }
    
    private IEnumerator DelayedFreezeControls()
    {
        yield return new WaitForSeconds(0.5f);
        
        GameManager.instance.FreezeControls();
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
        //StartCoroutine(DelayedFreezeControls());
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
            DisableImerisAnimation();
        }
        else
        {
            Debug.LogWarning("DeadScene called but CurrentGameState is not IsDead.");
        }
    }
    
    
    // IMERIS ANIMATION
    [YarnCommand("EnableImerisAnimation")]
    public void EnableImerisAnimation()
    {
        ImerisAnimation.SetActive(true);
    }
    
    [YarnCommand("DisableImerisAnimation")]
    public void DisableImerisAnimation()
    {
        ImerisAnimation.SetActive(false);
    }
    
    [YarnCommand("PlayImerisAnimation")]
    public void PlayImerisAnimation(string animationName)
    {
        Animator animator = ImerisAnimation.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(animationName);
        }
        else
        {
            Debug.LogWarning("ImerisAnimation Animator not found.");
        }
    }
    
    // NPC ANIMATION
    [YarnCommand("EnableNPCAnimation")]
    public void EnableNPCAnimation()
    {
        NPCAnimation.SetActive(true);
    }
    
    [YarnCommand("DisableNPCAnimation")]
    public void DisableNPCAnimation()
    {
        NPCAnimation.SetActive(false);
    }

    [YarnCommand("PlayNPCAnimation")]
    public void PlayNPCAnimation(string animationName)
    {
        Animator animator = NPCAnimation.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(animationName);
        }
        else
        {
            Debug.LogWarning("NPCAnimation Animator not found.");
        }
    }
    
    // CG DISPLAY
    [YarnCommand("PlayAnimation")]
    public void AnimationState(string state)
    {
        CGDisplayAnimator.Play(state);
    }
}
