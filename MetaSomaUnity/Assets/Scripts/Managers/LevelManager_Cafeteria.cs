using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LevelManager_Cafeteria : LevelManagerBase
{
    [SerializeField]Animator imerisAnimator; 
    [SerializeField]Animator npcAnimator;
    
    [SerializeField] private Transform defaultSpawnPoint;

    public override void Initialize()
    {
        base.Initialize();
        
        GameManager.instance.HUD.SetActive(true);
        GameManager.instance.CGDisplay.SetActive(false);
        
        //set the npc animation controller on the NPCAnimation object
        npcAnimator = NPCAnimation.GetComponent<Animator>();
        RuntimeAnimatorController npcAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/NPCEyes/NPCController");
        //load the animator controller
        npcAnimator.runtimeAnimatorController = npcAnimatorController; 
        
        // Disable NPC animation display
        NPCAnimation.SetActive(false);
        
        //set the imeris animation controller on the ImerisAnimation object
        imerisAnimator = ImerisAnimation.GetComponent<Animator>();
        
        // Set up spawn points
        defaultSpawnPoint = GameObject.Find("SpawnPoint").transform;
        
        // Set up Imeris respawn position
        ImerisMovement imerisMovement = GameObject.FindObjectOfType<ImerisMovement>();
        imerisMovement.gameObject.transform.position = defaultSpawnPoint.position;
    }
    
    
    [YarnCommand ("StartCower")]
    public void PlayImerisAnimation()
    {
        NPCAnimation.SetActive(true);
        imerisAnimator.Play("CoweringTransition");
        npcAnimator.Play("NPCEyesTransition");
    }
    
    [YarnCommand ("EndCower")]
    public void EndImerisAnimation()
    {
        //set the animation to play in reverse
        imerisAnimator.Play("CoweringTransitionReverse");       
        //set the NPC animation to play in reverse
        npcAnimator.Play("NPCEyesTransitionReverse");
        //set npc animation to false after playing
        //NPCAnimation.SetActive(false);
    }
}
