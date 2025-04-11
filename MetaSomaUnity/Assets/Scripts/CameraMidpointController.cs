using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMidpointController : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;
    public float xOffset = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetA != null && 
            targetB != null)
        {
            Vector3 midpoint = (targetA.position + targetB.position) / 2f;
            midpoint.y = Mathf.Min(targetA.position.y, targetB.position.y); 
            midpoint.x += xOffset; // Apply the xOffset to the midpoint
            transform.position = midpoint;
        }
    }
}
