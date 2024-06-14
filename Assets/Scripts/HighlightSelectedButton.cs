using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mohammad.VRProject.Scripts.HighlightSelectedButton
{


public class HighlightSelectedButton : MonoBehaviour
{

    [SerializeField]
    private GameObject[] buttons;

    [SerializeField]
    private GameObject[] detachedButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHighlightedButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
                buttons[i].GetComponent<UnityEngine.UI.Outline>().enabled = false;
        }
        GetComponent<UnityEngine.UI.Outline>().enabled = true;
    }

}
}
