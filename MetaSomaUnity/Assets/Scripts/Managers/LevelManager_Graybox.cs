using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_Graybox : LevelManagerBase
{
   protected override void Start()
   {
      base.Start();
      ImerisMovement.instance.currentState = new BeforeAnyEvolutionState(SubState.Healthy);
      GameManager.instance.HUD.SetActive(true);
   }
}
