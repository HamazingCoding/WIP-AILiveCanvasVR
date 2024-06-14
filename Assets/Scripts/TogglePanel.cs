using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace Mohammad.VRProject.Scripts.TogglePanel
{

    public class TogglePanel : MonoBehaviour
    {

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private Transform playerCamera;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void PanelToggle()
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
            else
            {
                Vector3 newObjectLocation = (playerCamera.forward * .3f) + playerCamera.position;
                panel.transform.position = newObjectLocation;
                panel.transform.LookAt(playerCamera);
                panel.SetActive(true);
            }
        }

    }

}