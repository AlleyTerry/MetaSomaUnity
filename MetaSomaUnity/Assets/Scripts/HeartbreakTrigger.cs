using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class HeartbreakTrigger : MonoBehaviour
{
    public Image heartbreakImage;
    public List<Sprite> heartbreakSprites;
    public int currentSpriteIndex = 0;
    
    public VisualEffect heartbreakVFX;
    
    // Start is called before the first frame update
    void Start()
    {
        heartbreakImage.sprite = heartbreakSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Heartbreak triggered!");
            
            // Play the heartbreak VFX through event
            heartbreakVFX.SendEvent("StartHeartbreak");
            
            // Change the sprite to the next one in the list
            if (currentSpriteIndex < heartbreakSprites.Count - 1)
            {
                currentSpriteIndex++;
            }
            else
            {
                // Reset to the first sprite if at the end of the list
                currentSpriteIndex = 0;
            }
            
            heartbreakImage.sprite = heartbreakSprites[currentSpriteIndex];
        }
    }
}
