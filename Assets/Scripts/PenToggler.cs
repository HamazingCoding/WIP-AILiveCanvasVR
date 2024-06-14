using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Samples.PalmMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohammad.VRProject.Scripts.PenToggler
{

    public class PenToggler : MonoBehaviour, IHandGrabUseDelegate
    {

        [SerializeField]
        private PalmMenuButtonHandlers _handlers;
        public void BeginUse()
        {
            Debug.Log("begin use");
            _handlers.ChangeDraw(true);
        }

        public float ComputeUseStrength(float strength)
        {
            //Debug.Log("strength: " + strength);
            return strength;
        }

        public void EndUse()
        {
            Debug.Log("end use");
            _handlers.ChangeDraw(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}