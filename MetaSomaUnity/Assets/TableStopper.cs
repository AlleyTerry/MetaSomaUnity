using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableStopper : MonoBehaviour
{
    public GameObject table;
    public GameObject player;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "TableForCommonRoom")
        {
            table.GetComponent<Rigidbody>().isKinematic = true;
        }
        //ignore player collider
        if (other.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(table.GetComponent<Collider>(), player.GetComponent<Collider>());
        }
        {
            
        }
        
    }
}
