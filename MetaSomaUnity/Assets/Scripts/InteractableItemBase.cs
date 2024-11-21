using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemBase : MonoBehaviour
{
    // VISUAL CUE
    public GameObject visualCue;
    
    // Start is called before the first frame update
    void Start()
    {
        visualCue = transform.GetChild(0).gameObject;
        visualCue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
