using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErasedData : MonoBehaviour
{

    public List<GameObject> children;
    public Transform _drawingContainer;

    // Start is called before the first frame update
    void Start()
    {
        //children = new List<GameObject>();      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendBack()
    {
        foreach(GameObject child in children)
        {
            child.transform.SetParent(_drawingContainer.GetChild(Int32.Parse(child.name)));
        }
    }
    public void BringBack()
    {
        foreach(GameObject child in children)
        {
            child.transform.SetParent(this.gameObject.transform);
        }
    } 

}
