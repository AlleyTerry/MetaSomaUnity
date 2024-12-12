using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_Graybox : LevelManagerBase
{
   protected override void Start()
   {
      base.Start();
      
      GameManager.instance.HUD.SetActive(true);
   }
}
