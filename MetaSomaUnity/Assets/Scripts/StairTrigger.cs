using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StairTrigger : MonoBehaviour
{
    [SerializeField] private List<Collider> stairColliderUp;  // Ceilings
    [SerializeField] private List<Collider> stairColliderDown; // Stairs
    
    [FormerlySerializedAs("isStairUp")] [SerializeField] private bool isGoingUp;
    
    // Start is called before the first frame update
    void Start()
    {
        isGoingUp = true;

        SetColliders(stairColliderUp, false);
        SetColliders(stairColliderDown,true);
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
            
            if (isGoingUp)
            {
                SetColliders(stairColliderUp,false);
            }
            else
            {
                SetColliders(stairColliderDown,true);
            }
            
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited stair transition.");

            if (isGoingUp)
            {
                SetColliders(stairColliderUp,true);
                SetColliders(stairColliderDown,false);
            }
            else
            {
                SetColliders(stairColliderUp,false);
                SetColliders(stairColliderDown,true);
            }
            
            isGoingUp = !isGoingUp;
        }
    }
    
    private void SetColliders(List<Collider> colliders, bool state)
    {
        foreach (var col in colliders)
        {
            if (col != null)
            {
                col.enabled = state;
            }
        }
    }
}
