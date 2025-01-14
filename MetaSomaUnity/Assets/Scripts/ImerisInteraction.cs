using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;


public class ImerisInteraction : ImerisMovement
{
    // NOTE: THIS IS A SUBCLASS OF IMERIS MOVEMENT SCRIPT,
    //       MOSTLY FOR IMERIS INTERACTION, INCLUDING GETTING IN/OUT OF THE BATTLE
    
    // GET INTO BATTLE
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BattleTrigger"))
        {
            GameManager.instance.isInBattle = true;
            GameManager.instance.currentLevelManager.StartBattleScene();
            
            other.gameObject.SetActive(false); // Disable the trigger
        }
    }
    
    // EXIT BATTLE
    //[YarnCommand("ExitBattle")]
    public void ExitBattle()
    {
        Debug.Log("Exiting battle...");
        GameManager.instance.isInBattle = false;
        
        
    }
}
