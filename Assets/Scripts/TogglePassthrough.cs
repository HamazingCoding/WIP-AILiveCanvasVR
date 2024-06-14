using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mohammad.VRProject.Scripts.TogglePassthrough
{
    public class TogglePassthrough : MonoBehaviour
    {
        [SerializeField]
        private OVRPassthroughLayer _passthroughLayer;

        [SerializeField]
        private OVRManager _manager;

        [SerializeField]
        private GameObject _room;

        private bool _transitioning = false;
        private int direction = -1;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_transitioning)
            {
                _passthroughLayer.textureOpacity += 0.01f * direction;

                if (_passthroughLayer.textureOpacity == 1)
                {
                    _transitioning = false;
                    direction = -1;
                } else if(_passthroughLayer.textureOpacity == 0)
                {
                    _transitioning = false;
                    direction = 1;
                    _room.SetActive(true);
                    //_passthroughLayer.enabled = false;
                    //_manager.isInsightPassthroughEnabled = true;
                }
            }
        }

        public void Toggle()
        {
            if(!_transitioning)
            {
                if(direction == 1)
                {
                    _room.SetActive(false);
                }
                _transitioning = true;
                //if(!_passthroughLayer.enabled)
                //{
                //    _passthroughLayer.enabled = true;
                //}
            }

        }


    }

}