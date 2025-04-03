using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PrologueMosiacs : MonoBehaviour
{
    public GameObject Mosiacs;
    public GameObject Mosiac1;
    public GameObject Mosiac2;
    public GameObject Mosiac3;
    public GameObject Mosiac4;
    public GameObject Mosiac5;
    public GameObject Mosiac6;

    public GameObject ImerisEyes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
    [YarnCommand("ShowMosiac1")]
    public void ShowMosiac1()
    {
        
        Mosiac1.SetActive(true);
        Mosiac2.SetActive(false);
        Mosiac3.SetActive(false);
        Mosiac4.SetActive(false);
        Mosiac5.SetActive(false);
        Mosiac6.SetActive(false);
    }
    [YarnCommand("ShowMosiac2")]
    public void ShowMosiac2()
    {
        Mosiac1.SetActive(false);
        Mosiac2.SetActive(true);
  
    }
    [YarnCommand("ShowMosiac3")]
    public void ShowMosiac3()
    {
        Mosiac2.SetActive(false);
        Mosiac3.SetActive(true);
       
    }
    [YarnCommand("ShowMosiac4")]
    public void ShowMosiac4()
    {
        Mosiac3.SetActive(false);
        Mosiac4.SetActive(true);
       
    }
    [YarnCommand("ShowMosiac5")]
    public void ShowMosiac5()
    {
        Mosiac4.SetActive(false);
        Mosiac5.SetActive(true);
       
    }
    [YarnCommand("ShowMosiac6")]
    public void ShowMosiac6()
    {
        Mosiac5.SetActive(false);
        Mosiac6.SetActive(true);
       
    }
    [YarnCommand("CloseMosiacs")]
    public void closeMosiacs()
    {
        Mosiac6.SetActive(false);
        ImerisEyes.SetActive(true);
    }

    [YarnCommand("FinalAnimation")]
    public void FinalAnimation()
    {
        ImerisEyes.SetActive(false);
        UIManager.instance.EnableAnimator();
        UIManager.instance.PlayAnimation("FinalPrologueAnimation");
    }
}
