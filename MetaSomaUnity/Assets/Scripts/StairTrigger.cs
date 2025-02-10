using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    [SerializeField] private Collider stairColliderUp;
    [SerializeField] private Collider stairColliderDown;
    
    [SerializeField] private bool isStairUp;
    
    // Start is called before the first frame update
    void Start()
    {
        isStairUp = true;
        
        if (stairColliderUp != null)
        {
            stairColliderUp.enabled = false; // DISABLE COLLIDER BY DEFAULT
        }
        else
        {
            Debug.LogWarning("StairColliderUp is not set.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered stair transition.");
            
            if (stairColliderUp != null && isStairUp)
            {
                stairColliderUp.enabled = true;
            }
            else if (stairColliderDown != null && !isStairUp)
            {
                stairColliderDown.enabled = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited stair transition.");
            
            if (stairColliderDown != null && isStairUp)
            {
                stairColliderDown.enabled = false;
                isStairUp = false;
            }
            else if (stairColliderUp != null && !isStairUp)
            {
                stairColliderUp.enabled = false;
                isStairUp = true;
            }
        }
    }
}
