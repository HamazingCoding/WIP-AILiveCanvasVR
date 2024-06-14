using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Samples.PalmMenu;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mohammad.VRProject.Scripts.Eraser
{
    public class Eraser : MonoBehaviour
    {

        [Header("Hands & Grabbable")]

        [SerializeField]
        private HandGrabInteractor _rightHandInteractor;

        [SerializeField]
        private HandGrabInteractor _leftHandInteractor;

        [SerializeField]
        private HandGrabInteractable _handGrabInteractable;

        [SerializeField]
        private Transform _drawingContainer;

        //private GameObject _tubeMesh;

        private bool _isErasing;

        private Transform _currentParent;

        private List<GameObject> _erased;

        private ErasedData _erasedData;


        // Start is called before the first frame update
        void Start()
        {
            _currentParent = null;
            _erased = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            bool isRightHandErasing = _rightHandInteractor.HasSelectedInteractable && _rightHandInteractor.SelectedInteractable == _handGrabInteractable;
            bool isLeftHandErasing = _leftHandInteractor.HasSelectedInteractable && _leftHandInteractor.SelectedInteractable == _handGrabInteractable;

            // just started holding
            if ((isRightHandErasing || isLeftHandErasing) && !_isErasing)
            {
                _isErasing = true;
                _currentParent = _drawingContainer.transform.GetChild(_drawingContainer.childCount - 1);
                _currentParent.gameObject.SetActive(false);
                _erasedData = _currentParent.AddComponent<ErasedData>();
                _erasedData._drawingContainer = _drawingContainer;
                _erasedData.children = new List<GameObject>();
                
            }

            // just let go
            if (!(isRightHandErasing || isLeftHandErasing) && _isErasing)
            {
                _isErasing = false;

                
                if(_currentParent.childCount != 0)
                {
                    _currentParent = new GameObject().transform;
                    _currentParent.SetParent(_drawingContainer);
                } else
                {
                    _currentParent.gameObject.SetActive(true);
                    Destroy(_erasedData);
                }
                _erasedData = null;
                _currentParent = null;
                //foreach (GameObject obj in _erased)
                //{
                //    obj.transform.SetParent(currentParent);
                //}
                

            }

        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("THE NAME OF THE OBJECT YOU ARE TOUCHING IS: " + other.gameObject.name);
            if (other.gameObject.tag == "TubeMesh")
            {
                //_erased.Add(other.gameObject);
                if(_currentParent != null)
                {
                    other.transform.SetParent(_currentParent);
                    _erasedData.children.Add(other.gameObject);
                }
                
                //other.gameObject.SetActive(false);
            }


            //if (other.gameObject.name == "TubeMesh")
            //{
            //    tubeMesh = other.gameObject;
            //}


        }

        public void ResetScale()
        {
            transform.localScale = new Vector3(.015f, .05f, .018f);
        } 

        //private void OnTriggerExit(Collider other)
        //{
        //    _tipMaterial.SetColor("_Color", Color.white);
        //}
    }

}