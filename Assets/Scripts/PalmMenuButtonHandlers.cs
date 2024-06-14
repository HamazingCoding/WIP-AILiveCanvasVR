/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static OVRPlugin;

namespace Oculus.Interaction.Samples.PalmMenu
{
    /// <summary>
    /// Example of a bespoke behavior created to react to a particular palm menu. This controls the state
    /// of the object that responds to the menu, but also parts of the menu itself, specifically those
    /// which depend on the state of the controlled object (swappable icons, various text boxes, etc.).
    /// </summary>
    public class PalmMenuButtonHandlers : MonoBehaviour
    {

        [Header("Hand Menu Properties")]

        [SerializeField]
        private GameObject _rotationEnabledIcon;

        [SerializeField]
        private GameObject _rotationDisabledIcon;

        //[SerializeField]
        //private float _rotationLerpSpeed = 1f;

        [SerializeField]
        private TMP_Text _rotationDirectionText;

        [SerializeField]
        private string[] _rotationDirectionNames;

        [SerializeField]
        private GameObject[] _rotationDirectionIcons;

        [SerializeField]
        private Quaternion[] _rotationDirections;

        [SerializeField]
        private TMP_Text _elevationText;

        [SerializeField]
        private float _penWidthChangeIncrement;

        //[SerializeField]
        //private float _elevationChangeLerpSpeed = 1f;

        [SerializeField]
        private TMP_Text _shapeNameText;

        [SerializeField]
        private string[] _shapeNames;

        [SerializeField]
        private UnityEngine.Mesh[] _shapes;

        [Header("BuildPen Properties")]

        [SerializeField]
        private GameObject _pen;

        [SerializeField]
        private Transform _tip;

        [SerializeField]
        private Material _drawingMaterial;

        [SerializeField]
        private Material _tipMaterial;

        //[SerializeField]
        //[Range(0.01f, 0.1f)]
        //private float _penWidth = 0.01f;

        [SerializeField]
        private Color[] _penColors;

        [SerializeField]
        private float _tubeRadius = 0.01f; // Radius of the tube

        [SerializeField]
        private int _segments = 8; // Number of segments around the tube

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private Material _maskMaterial;

        [SerializeField]
        private TextMeshProUGUI _thicknessLabel;

        [SerializeField]
        private Image[] _colorIndicators;

        //[SerializeField]
        //private Transform _drawingContainer;

        [Header("Hands & Grabbable")]

        [SerializeField]
        private HandGrabInteractor _rightHandInteractor;

        [SerializeField]
        private HandGrabInteractor _leftHandInteractor;

        [SerializeField]
        private HandGrabInteractable _handGrabInteractable;

        [SerializeField]
        private HandGrabInteractable _eraserInteractable;

        private int _currentColorIdx;
        private bool _eraseEnabled;
        private int _currentRotationDirectionIdx;
        private Vector3 _targetPosition;
        private int _currentShapeIdx;
        private int _index;
        private bool _isDrawing;
        private UnityEngine.Mesh _tubeMesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;
        private LineRenderer _currentDrawing;

        [SerializeField]
        private Transform _drawingContainer;

        private int _currentChild;
        //private bool _isErasing;
        //private int _drawingQueue;
        private int _childrenCapture;
        private Transform _currentParent;


        private void Start()
        {
            //_drawingContainer = new GameObject().transform;

            _currentColorIdx = _penColors.Length;
            CycleColor();

            foreach(Image colorIndicator in _colorIndicators)
            {
                colorIndicator.color = _penColors[_currentColorIdx];
            }

            

            _eraseEnabled = false;
            ToggleRotationEnabled();

            _thicknessLabel.text = _slider.value.ToString("0.000");

            //_currentRotationDirectionIdx = _rotationDirections.Length;
            //CycleRotationDirection();

            //_targetPosition = _controlledObject.transform.position;
            //IncrementElevation(true);
            //IncrementElevation(false);

            //_currentShapeIdx = _shapes.Length;
            //CycleShape(true);

            //_tipMaterial.color = _penColors[_currentColorIdx];

            _isDrawing = true;
            ToggleDraw();
            IncrementPenWidth(true);
            //InitializeMeshComponents();
            _currentChild = -1;
            //_drawingQueue = 5000;

            _currentParent = new GameObject().transform;
            _currentParent.SetParent(_drawingContainer);

            //_isErasing = false;

        }

        private void Update()
        {
            //if (_rotationEnabled)
            //{
            //    var rotation = Quaternion.Slerp(Quaternion.identity, _rotationDirections[_currentRotationDirectionIdx], _rotationLerpSpeed * Time.deltaTime);
            //    _controlledObject.transform.rotation = rotation * _controlledObject.transform.rotation;
            //}

            //_controlledObject.transform.position = Vector3.Lerp(_controlledObject.transform.position, _targetPosition, _elevationChangeLerpSpeed * Time.deltaTime);

            bool isRightHandDrawing = _rightHandInteractor.HasSelectedInteractable && _rightHandInteractor.SelectedInteractable == _handGrabInteractable;
            bool isLeftHandDrawing = _leftHandInteractor.HasSelectedInteractable && _leftHandInteractor.SelectedInteractable == _handGrabInteractable;


            if (_isDrawing && (isRightHandDrawing || isLeftHandDrawing))
            {
                Draw();
            }
            else if (_currentDrawing != null)
            {
                ChangeDraw(false);
                if(_meshCollider != null /*&& !_eraserMode*/)
                {
                    _meshCollider.convex = true;
                    _meshCollider.convex = false;
                }

                for (int i = _drawingContainer.childCount - 2; i > _currentChild; i--)
                {
                    if (!_drawingContainer.GetChild(i).gameObject.activeSelf)
                    {
                        Destroy(_drawingContainer.GetChild(i).gameObject);
                    }
                }

                _currentDrawing = null;
                _meshRenderer = null;

                
                //if(_currentParent.childCount != 0)
                //{
                    _currentParent = new GameObject().transform;
                    _currentParent.SetParent(_drawingContainer);
                    _currentChild = _drawingContainer.childCount - 2;
                //}
                
            } else if (_currentParent != _drawingContainer.GetChild(_drawingContainer.childCount - 1))
            {
                _currentParent = _drawingContainer.GetChild(_drawingContainer.childCount - 1);
                _currentChild = _drawingContainer.childCount - 2;
                
                
            }


            //if(_isDrawing && !(isRightHandDrawing || isLeftHandDrawing))
            //{
            //    ChangeDraw(false);
            //}

            //bool isRightHandErasing = _rightHandInteractor.HasSelectedInteractable && _rightHandInteractor.SelectedInteractable == _eraserInteractable;
            //bool isLeftHandErasing = _leftHandInteractor.HasSelectedInteractable && _leftHandInteractor.SelectedInteractable == _eraserInteractable;

            //ArrayList 

            //if( isRightHandErasing && isLeftHandErasing && !_isErasing)
            //{
            //    _isErasing = true;
            //}

            //if(_isErasing)
            //{

            //}



        }

        public Transform GetCurrentParent()
        {
            return _currentParent;
        }

        /// <summary>
        /// Change the color of the controlled object to the next in the list of allowed colors, looping if the end of the list is reached.
        /// </summary>
        public void CycleColor()
        {
            _currentColorIdx += 1;
            if (_currentColorIdx >= _penColors.Length)
            {
                _currentColorIdx = 0;
            }

            _tipMaterial.SetColor("_Color", _penColors[_currentColorIdx]);
        }

        public void ChangeColor(int index)
        {
            _currentColorIdx = index;
            if(_currentColorIdx >= _penColors.Length)
            {
                _currentColorIdx = 0;
            }
            _tipMaterial.SetColor("_Color", _penColors[_currentColorIdx]);
            foreach (Image colorIndicator in _colorIndicators)
            {
                colorIndicator.color = _penColors[_currentColorIdx];
            }
        }

        public void ChangeColor(Color color)
        {
            _tipMaterial.SetColor("_Color", color);
        }

        //public void ToggleEraserMode()
        //{
        //    _eraserMode = !_eraserMode;
        //    if(_eraserMode) { _childrenCapture = _drawingContainer.childCount; }
        //    if(!_eraserMode && _childrenCapture != _drawingContainer.childCount) { _drawingQueue -= 2; }
        //    Debug.Log("Eraser mode: " + _eraserMode);
        //}

        /// <summary>
        /// Toggle whether or not rotation is enabled, and set the icon of the controlling button to display what will happen next time the button is pressed.
        /// </summary>
        public void ToggleRotationEnabled()
        {
            _eraseEnabled = !_eraseEnabled;
            _rotationEnabledIcon.SetActive(!_eraseEnabled);
            _rotationDisabledIcon.SetActive(_eraseEnabled);
        }

        /// <summary>
        /// Change the rotation direction of the controlled object to the next in the list of allowed directions, looping if the end of the list is reached.
        /// Set the icon of the controlling button to display what will happen next time the button is pressed.
        /// </summary>
        public void CycleRotationDirection()
        {
            //Debug.Assert(_rotationDirectionNames.Length == _rotationDirections.Length);
            //Debug.Assert(_rotationDirectionNames.Length == _rotationDirectionIcons.Length);

            //_currentRotationDirectionIdx += 1;
            //if (_currentRotationDirectionIdx >= _rotationDirections.Length)
            //{
            //    _currentRotationDirectionIdx = 0;
            //}

            //int nextRotationDirectionIdx = _currentRotationDirectionIdx + 1;
            //if (nextRotationDirectionIdx >= _rotationDirections.Length)
            //{
            //    nextRotationDirectionIdx = 0;
            //}

            //_rotationDirectionText.text = _rotationDirectionNames[nextRotationDirectionIdx];
            //for (int idx = 0; idx < _rotationDirections.Length; ++idx)
            //{
            //    _rotationDirectionIcons[idx].SetActive(idx == nextRotationDirectionIdx);
            //}
        }

        /// <summary>
        /// Change the target elevation of the controlled object in the requested direction, within the limits [0.2, 2].
        /// Set the text to display the new target elevation.
        /// </summary>
        public void IncrementPenWidth(bool up)
        {
            //float increment = _elevationChangeIncrement;
            //if (!up)
            //{
            //    increment *= -1f;
            //}
            //_targetPosition = new Vector3(_targetPosition.x, Mathf.Clamp(_targetPosition.y + increment, 0.2f, 2f), _targetPosition.z);
            //_elevationText.text = "Elevation: " + _targetPosition.y.ToString("0.0");

            float increment = _penWidthChangeIncrement;

            if (!up)
            {
                increment *= -1f;
            }
            _tubeRadius += increment;
            //_elevationText.text = "Thickness: " + _tubeRadius.ToString("0.000");

        }

        public void ChangePenWidth(float width)
        {
            _tubeRadius = _slider.value;
            //_elevationText.text = "Thickness: " + _tubeRadius.ToString("0.000");
            _thicknessLabel.text = _tubeRadius.ToString("0.000");
        }

        /// <summary>
        /// Change the shape of the controlled object to the next or previous in the list of allowed shapes, depending on the requested direction, looping beyond the bounds of the list.
        /// Set the text to display the name of the current shape.
        /// </summary>
        public void CycleShape(bool cycleForward)
        {
            //Debug.Assert(_shapeNames.Length == _shapes.Length);

            //_currentShapeIdx += cycleForward ? 1 : -1;
            //if (_currentShapeIdx >= _shapes.Length)
            //{
            //    _currentShapeIdx = 0;
            //}
            //else if (_currentShapeIdx < 0)
            //{
            //    _currentShapeIdx = _shapes.Length - 1;
            //}

            //_shapeNameText.text = _shapeNames[_currentShapeIdx];
            //_controlledObject.GetComponent<MeshFilter>().mesh = _shapes[_currentShapeIdx];
        }


        private void Draw()
        {

            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //sphere.GetComponent<MeshRenderer>().material.color = _tipMaterial.color;
            //sphere.transform.localScale = new Vector3(_penWidth, _penWidth, _penWidth);
            //sphere.transform.position = _tip.position;
            //sphere.GetComponent<Collider>().enabled = false;

            if (_currentDrawing == null)
            {
                _index = 0;
                //_currentParent = _drawingContainer.GetChild(_drawingContainer.childCount - 1);
                //_currentChild = _drawingContainer.childCount - 2;
                _currentDrawing = new GameObject().AddComponent<LineRenderer>();
                //_currentDrawing.material = _drawingMaterial;
                _currentDrawing.material.color = _tipMaterial.color;
                //_currentDrawing.startColor = _currentDrawing.endColor = _penColors[_currentColorIdx];
                _currentDrawing.startColor = _currentDrawing.endColor = _tipMaterial.color;
                _currentDrawing.startWidth = _currentDrawing.endWidth = 0;
                _currentDrawing.positionCount = 1;
                _currentDrawing.SetPosition(0, _tip.position);
                _currentDrawing.transform.SetParent(_currentParent);
                InitializeMeshComponents();
                GenerateTubeMesh();
            }
            else
            {
                var currentPos = _currentDrawing.GetPosition(_index);
                Vector3 previousPos = Vector3.zero;
                if(_index > 1)
                {
                    previousPos = _currentDrawing.GetPosition(_index - 1);
                }
                 
                if (Vector3.Distance(currentPos, _tip.position) > 0.01f)
                {
                    _index++;
                    if (_index >= 5)
                    {
                        
                        _currentDrawing = new GameObject().AddComponent<LineRenderer>();
                        _currentDrawing.material.color = _tipMaterial.color;
                        _currentDrawing.startColor = _currentDrawing.endColor = _tipMaterial.color;
                        _currentDrawing.startWidth = _currentDrawing.endWidth = 0;
                        _currentDrawing.positionCount = 3; 
                        _currentDrawing.SetPosition(0, previousPos);
                        _currentDrawing.SetPosition(1, currentPos);
                        _currentDrawing.SetPosition(2, _tip.position);
                        _currentDrawing.transform.SetParent(_currentParent);
                        _index = 2;
                        InitializeMeshComponents();
                        GenerateTubeMesh();
                    }
                    else
                    {
                        _currentDrawing.positionCount = _index + 1;
                        _currentDrawing.SetPosition(_index, _tip.position);
                        GenerateTubeMesh();
                    }

                }

                //var currentPos = _currentDrawing.GetPosition(_index);
                //if (Vector3.Distance(currentPos, _tip.position) > 0.01f)
                //{
                //    _index++;
                //    _currentDrawing.positionCount = _index + 1;
                //    _currentDrawing.SetPosition(_index, _tip.position);
                //    GenerateTubeMesh();
                //}


            }

            if (_meshCollider != null /*&& !_eraserMode*/)
            {
                _meshCollider.convex = true;
                _meshCollider.convex = false;
            }

            // SEE THIS MAKE SURE ITS ACTUALLY USEFUL AND NOT BAD
            //for (int i = _drawingContainer.childCount - 2; i > _currentChild; i--)
            //{
            //    if(!_drawingContainer.GetChild(i).gameObject.activeSelf)
            //    {
            //        Destroy(_drawingContainer.GetChild(i).gameObject);
            //    }
            //}

            //_currentChild = _drawingContainer.childCount - 1;
            


        }

        public void Undo()
        {
            

            if(_currentDrawing == null && _currentChild >= 0)
            {
                Debug.Log("Undoing child " +  _currentChild);
                Transform child = _drawingContainer.GetChild(_currentChild);
                if(child.TryGetComponent<ErasedData>(out ErasedData data))
                {
                    data.SendBack();
                }
                child.gameObject.SetActive(!child.gameObject.activeSelf);
                _currentChild--;
            }
        }

        public void Redo()
        {
            if(_currentDrawing == null && _currentChild < _drawingContainer.childCount - 2) // you changed this from -1 to -2 btw
            {
                Debug.Log("Redoing child " + (_currentChild + 1));
                Transform child = _drawingContainer.GetChild(_currentChild + 1);
                if (child.TryGetComponent<ErasedData>(out ErasedData data))
                {
                    data.BringBack();
                }
                child.gameObject.SetActive(!child.gameObject.activeSelf);
                _currentChild++;
            }
        }

        public void ClearDrawing()
        {
            //Destroy(_drawingContainer.gameObject);

            for(int i = _drawingContainer.childCount-2; i >= 0; i--)
            {
                Destroy(_drawingContainer.GetChild(i).gameObject);
            }

            //_drawingContainer = new GameObject().transform;
            _currentDrawing = null;
            _currentChild = -1;
            //_currentParent = new GameObject().transform;
            //_currentParent.SetParent(_drawingContainer);
        }

        private void InitializeMeshComponents()
        {
            GameObject meshObject = new GameObject("" + (_drawingContainer.childCount - 1));
            meshObject.transform.SetParent(_currentParent);
            meshObject.tag = "TubeMesh";
            //meshObject.transform.SetParent(this.transform);
            _meshFilter = meshObject.AddComponent<MeshFilter>();
            _meshRenderer = meshObject.AddComponent<MeshRenderer>();
            _meshCollider = meshObject.AddComponent<MeshCollider>();
            //_meshRenderer.material = _drawingMaterial;
            //if(/*_eraserMode*/)
            //{
            //    _meshRenderer.material = _maskMaterial;
            //    meshObject.name = "EraserMesh";
            //    //_meshCollider.convex = true;
            //    //_meshCollider.isTrigger = true;
            //    //for (int i = 1; i < _drawingContainer.childCount-2; i+=2)
            //    //{
            //    //    Transform child = _drawingContainer.GetChild(i);
            //    //    if (child.gameObject.name == "TubeMesh")
            //    //    {
            //    //        _drawingContainer.GetChild(i).GetComponent<MeshRenderer>().material.renderQueue = _drawingQueue;
            //    //    } else if(child.gameObject.name == "EraserMesh")
            //    //    {
            //    //        _drawingContainer.GetChild(i).GetComponent<MeshRenderer>().material.renderQueue = _drawingQueue-1;
            //    //    }

            //    //}
            //    //_meshRenderer.material.renderQueue = _drawingQueue - 1;
            //    //meshObject.AddComponent<PenEraser>();
            //} else
            //{
                //_meshRenderer.material.renderQueue = _drawingQueue;
                _meshRenderer.material.color = _tipMaterial.color;
            //}
            
            _tubeMesh = new UnityEngine.Mesh();
            _meshFilter.mesh = _tubeMesh;
            _meshCollider.sharedMesh = _tubeMesh;


        }

        public void ToggleDraw()
        {
            _isDrawing = !_isDrawing;
            MeshRenderer penRenderer = GetComponent<MeshRenderer>();
            if (_isDrawing)
            {
                penRenderer.material.color = Color.white;
            }
            else
            {
                penRenderer.material.color = Color.gray;
            }
        }

        public void ChangeDraw(bool change)
        {
            
            MeshRenderer penRenderer = GetComponent<MeshRenderer>();
            //bool isRightHandDrawing = _rightHandInteractor.HasSelectedInteractable && _rightHandInteractor.SelectedInteractable == _handGrabInteractable;
            //bool isLeftHandDrawing = _leftHandInteractor.HasSelectedInteractable && _leftHandInteractor.SelectedInteractable == _handGrabInteractable;
            //if (isRightHandDrawing || isLeftHandDrawing)
            //{
                _isDrawing = change;
                if (_isDrawing)
                {
                    penRenderer.material.color = Color.white;
                }
                else
                {
                    penRenderer.material.color = Color.gray;
                }
            //}

            
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    _tipMaterial.SetColor("_Color", Color.black);
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    _tipMaterial.SetColor("_Color", Color.white);
        //}

        private void GenerateTubeMesh()
        {
            int numPoints = _currentDrawing.positionCount;
            if (numPoints < 2) return;

            Vector3[] points = new Vector3[numPoints];
            _currentDrawing.GetPositions(points);

            int vertexCount = numPoints * _segments;
            int triangleCount = (numPoints - 1) * _segments * 2 * 3;

            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];
            int[] triangles = new int[triangleCount];

            for (int i = 0; i < numPoints; i++)
            {
                Vector3 currentPoint = points[i];
                Vector3 forward = Vector3.forward;
                if (i < numPoints - 1)
                    forward = (points[i + 1] - currentPoint).normalized;
                else if (i > 0)
                    forward = (currentPoint - points[i - 1]).normalized;

                Vector3 up = Vector3.Cross(forward, Vector3.up).normalized * _tubeRadius;
                Vector3 right = Vector3.Cross(forward, up).normalized * _tubeRadius;

                for (int j = 0; j < _segments; j++)
                {
                    float angle = (float)j / _segments * Mathf.PI * 2f;
                    Vector3 offset = right * Mathf.Cos(angle) + up * Mathf.Sin(angle);
                    vertices[i * _segments + j] = currentPoint + offset;
                    uvs[i * _segments + j] = new Vector2((float)j / _segments, (float)i / numPoints);
                }
            }

            int triIndex = 0;
            for (int i = 0; i < numPoints - 1; i++)
            {
                for (int j = 0; j < _segments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % _segments;

                    triangles[triIndex++] = i * _segments + j;
                    triangles[triIndex++] = nextI * _segments + j;
                    triangles[triIndex++] = nextI * _segments + nextJ;

                    triangles[triIndex++] = i * _segments + j;
                    triangles[triIndex++] = nextI * _segments + nextJ;
                    triangles[triIndex++] = i * _segments + nextJ;
                }
            }
            
            //InitializeMeshComponents();
            _tubeMesh.Clear();
            
            _tubeMesh.vertices = vertices;
            _tubeMesh.uv = uvs;
            _tubeMesh.triangles = triangles;
            //Debug.Log("length of vertices: " + vertices.Length);
            //Debug.Log("length of uvs: " + uvs.Length);
            //Debug.Log("length of triangles: " + triangles.Length);
            _tubeMesh.RecalculateNormals();
        }

    }
}
