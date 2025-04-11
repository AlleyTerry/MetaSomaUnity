using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LedgerMinigame : MonoBehaviour
{
    public GameObject snapPoint1;
    public GameObject snapPoint2;
    public GameObject snapPoint3;
    //list of snap points
    public List<GameObject> snapPoints = new List<GameObject>();
    public List<GameObject> ButtonsNamePlates = new List<GameObject>();
    public GameObject clickedObject;
    public bool isSnapped = false;
    public int currentSnapPointIndex = 0;
    public bool snapPoint1Clicked = true;

    public int enterIndex;
    // Start is called before the first frame update
    void Start()
    {
        snapPoint1 = GameObject.Find("SnapPoint1");
        snapPoint2 = GameObject.Find("SnapPoint2");
        snapPoint3 = GameObject.Find("SnapPoint3");
        //add snap points to the list
        snapPoints.Add(snapPoint1);
        snapPoints.Add(snapPoint2);
        snapPoints.Add(snapPoint3);
    }

    // Update is called once per frame
    void Update()
    {
        
        //when the player presses up or down arrow key then move the clickedobject next snap point cycling through the list
        if (Input.GetKeyDown(KeyCode.UpArrow) && isSnapped)
        {
            if (currentSnapPointIndex == 0)
            {
                currentSnapPointIndex = 2;
            }
            else
            {
                currentSnapPointIndex--;
            }

            if (clickedObject != null)
            {
                clickedObject.transform.position = snapPoints[currentSnapPointIndex].transform.position;
                clickedObject.transform.SetParent(snapPoints[currentSnapPointIndex].transform);
            }
           
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && isSnapped)
        {
            if (currentSnapPointIndex == 2)
            {
                currentSnapPointIndex = 0;
            }
            else
            {
                currentSnapPointIndex++;
            }

            if (clickedObject != null)
            {
                clickedObject.transform.position = snapPoints[currentSnapPointIndex].transform.position;
                clickedObject.transform.SetParent(snapPoints[currentSnapPointIndex].transform);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Return) )
        {
            enterIndex++;

            if (enterIndex == 2)
            {
                //if index is 0 and the clicked object button name is "Linn" debug log correct
                if (currentSnapPointIndex == 0 && clickedObject.name == "Linn")
                {
                    Debug.Log("Correct");
                    //no longer allow the button to be chosen
                    clickedObject.GetComponent<Button>().interactable = false;
                    //reset the enter index
                    enterIndex = 0;
                    snapPoint1Clicked = true;
                    //remove the clicked object from the button list
                    ButtonsNamePlates.Remove(clickedObject);
                    clickedObject = null;
                    //set eventsystem first selected to the first button in the button list
                    EventSystem.current.SetSelectedGameObject(ButtonsNamePlates[0]);
                
                }
                else if (currentSnapPointIndex == 1 && clickedObject.name == "Galleria")
                {
                    Debug.Log("Correct");
                    //stop controlling the button
                    clickedObject.GetComponent<Button>().interactable = false;
                    //reset the enter index
                    enterIndex = 0;
                    snapPoint1Clicked = true;
                    //remove the clicked object from the button list
                    ButtonsNamePlates.Remove(clickedObject);
                    clickedObject = null;
                    //set eventsystem first selected to the first button in the button list
                    EventSystem.current.SetSelectedGameObject(ButtonsNamePlates[0]);
                
                }
                else if (currentSnapPointIndex == 2 && clickedObject.name == "Imeris")
                {
                    Debug.Log("Correct");
                    //stop controlling the button
                    clickedObject.GetComponent<Button>().interactable = false;
                    //reset the enter index
                    enterIndex = 0;
                    snapPoint1Clicked = true;
                    //remove the clicked object from the button list
                    ButtonsNamePlates.Remove(clickedObject);
                    clickedObject = null;
                    //set eventsystem first selected to the first button in the button list
                    EventSystem.current.SetSelectedGameObject(ButtonsNamePlates[0]);
                    
                }
                else
                {
                    Debug.Log("Incorrect");
                    //reset the enter index
                    enterIndex--;

                }
              
            }
        }
        
    }
    
    public void ToSnapPoint1()
    {
        if (snapPoint1Clicked)
        {
            Debug.Log("Snap Point 1 clicked");
            //when the button is clicked on get its name
            clickedObject = EventSystem.current.currentSelectedGameObject;
            //set the position of the clicked object to the position of snapPoint1
            clickedObject.transform.position = snapPoint1.transform.position;
            //set the parent of the clicked object to snapPoint1
            clickedObject.transform.SetParent(snapPoint1.transform);
            isSnapped = true;
            snapPoint1Clicked = false;
            // disable the button
            EventSystem.current.SetSelectedGameObject(null);
        }
        
    }
}
