using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCGTriggers : MonoBehaviour
{
    public GameObject AudioGameObject;
    // Start is called before the first frame update
    void Start()
    {
        //find the audio game object on the parent object
        AudioGameObject = GameObject.Find("ManagerHolder");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CrackSFX()
    {
        // Play the crack sound effect
        AudioGameObject.GetComponent<AudioManager>().PlaySFX("heartBreaking");
    }

    public void StabSFX()
    {
        // Play the crack sound effect
        AudioGameObject.GetComponent<AudioManager>().PlaySFX("KnifeStab");
    }
    
    public void Eatbread1()
    {
        // Play the first dialogue
        DialogueManager.instance.dialogueRunner.StartDialogue("Eatbread1");
    }

    public void Eatbread2()
    {
        // Play the second dialogue
        DialogueManager.instance.dialogueRunner.StartDialogue("Eatbread2");
    }
    
    public void Eatbread3()
    {
        // Play the third dialogue
        DialogueManager.instance.dialogueRunner.StartDialogue("Eatbread3");
    }
    
    public void Eatbread4()
    {
        // Play the fourth dialogue
        DialogueManager.instance.dialogueRunner.StartDialogue("Eatbread4");
    }

    public void EndEatLinn()
    {
        // Play the last eatLinn dialogue
        DialogueManager.instance.dialogueRunner.StartDialogue("EndEatLinn");
    }
    
    public void EndLinnAttack()
    {
        //close cgplayer
        GameManager.instance.CGDisplay.SetActive(false);
        // Play the last eatImeris dialogue
        DialogueManager.instance.dialogueRunner.StartDialogue("EndLinnAttack");
    }
}
