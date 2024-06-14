using Oculus.Interaction.Samples.PalmMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePen : MonoBehaviour
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
        if (other.gameObject.tag != "TubeMesh" && other.gameObject.name != "Eraser")
        {
            this.transform.parent.GetComponent<PalmMenuButtonHandlers>().ToggleDraw();
        }


        //if (other.gameObject.name == "TubeMesh")
        //{
        //    tubeMesh = other.gameObject;
        //}


    }

}
