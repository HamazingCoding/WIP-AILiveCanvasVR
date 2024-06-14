using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohammad.VRProject.Scripts.ToggleContainer
{
    public class ToggleContainer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Toggle()
        {
            if(this.gameObject.activeInHierarchy)
            {
                this.gameObject.SetActive(false);
            } else
            {
                this.gameObject.SetActive(true);
            }
        }


    }

}
