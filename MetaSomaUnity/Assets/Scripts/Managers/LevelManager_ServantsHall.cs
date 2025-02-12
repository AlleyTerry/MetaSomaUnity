using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_ServantsHall : LevelManagerBase
{
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
      
      // Play BGM
      /*gameObject.GetComponent<AudioManager>().PlayMusic("level1");*/
   }
}
