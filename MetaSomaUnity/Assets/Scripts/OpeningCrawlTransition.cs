using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class OpeningCrawlTransition : MonoBehaviour
{
   /*[YarnCommand("LeaveOpeningCrawl")]  */
   public void LeaveOpeningCrawl()
   {
      SceneManager.LoadScene("Level_ServantsHall");
   }
   
}
