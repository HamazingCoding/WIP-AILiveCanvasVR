using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenEraser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("THE NAME OF THE OBJECT YOU ARE TOUCHING IS: " + other.gameObject.name);
        if (other.gameObject.name == "TubeMesh")
        {
            other.GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }


}
