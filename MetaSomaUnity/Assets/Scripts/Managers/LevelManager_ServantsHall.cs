using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class LevelManager_ServantsHall : LevelManagerBase
{
   public GameObject LedgerMinigameCanvas;
   public GameObject LedgerMinigameManager;
   public GameObject LinnButton;
   protected override void Start()
   {
      base.Start();
      
      ImerisMovement.instance.currentState = new BeforeAnyEvolutionState(SubState.Healthy);
   }

   public override void Initialize()
   {
      base.Initialize();
      
      GameManager.instance.HUD.SetActive(true);
      GameManager.instance.CGDisplay.SetActive(false);
      
      EnableImerisAnimation();
      DisableNPCAnimation();
      LedgerMinigameCanvas = GameObject.Find("LedgerMinigameCanvas");
      LedgerMinigameManager = GameObject.Find("LedgerMinigameManager");
      LinnButton = GameObject.Find("Linn");
      LedgerMinigameCanvas.SetActive(false);
      
      // Play BGM
      /*gameObject.GetComponent<AudioManager>().PlayMusic("level1");*/
   }
   [YarnCommand("StartLedgerMinigame")]
   public void StartLedgerMinigame()
   {
      EventSystem.current.SetSelectedGameObject(LinnButton);
      LedgerMinigameCanvas.SetActive(true);
      GameManager.instance.FreezeControls();
   }
   [YarnCommand("EndLedgerMinigame")]
   public void EndLedgerMinigame()
   {
      LedgerMinigameCanvas.SetActive(false);
      GameManager.instance.ResumeControls();
      
   }
}
