using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections.Generic;

public class Pen : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f;
    public Color[] penColors;

    [Header("Hands & Grabbable")]
    public HandGrabInteractor rightHandInteractor;
    public HandGrabInteractor leftHandInteractor;
    public HandGrabInteractable handGrabInteractable;

    private LineRenderer currentDrawing;
    private int index;
    private int currentColorIndex;
    private bool isDrawing;
    //private Material defaultMaterial;

    private void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];
        isDrawing = false;
        GetComponent<MeshRenderer>().material.color = Color.gray;
        //defaultMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        bool isRightHandDrawing = rightHandInteractor.HasSelectedInteractable && rightHandInteractor.SelectedInteractable == handGrabInteractable;
        bool isLeftHandDrawing = leftHandInteractor.HasSelectedInteractable && leftHandInteractor.SelectedInteractable == handGrabInteractable;

        if (Input.GetKeyDown(KeyCode.D)) // Replace with appropriate button check for your setup
        {
            isDrawing = !isDrawing;
            MeshRenderer penRenderer = GetComponent<MeshRenderer>();
            if (/*penRenderer.material == defaultMaterial*/ isDrawing)
            {
                penRenderer.material.color = Color.white;
            } else
            {
                penRenderer.material.color = Color.gray;
            }

        }

        if (isDrawing && (isRightHandDrawing || isLeftHandDrawing))
        {
            Draw();
        }
        else if (currentDrawing != null)
        {
            currentDrawing = null;
        }
        else if (Input.GetKeyDown(KeyCode.C)) // Replace with appropriate button check for your setup
        {
            SwitchColor();
        }
    }

    private void Draw()
    {
        if (currentDrawing == null)
        {
            index = 0;
            currentDrawing = new GameObject().AddComponent<LineRenderer>();
            currentDrawing.material = drawingMaterial;
            currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
            currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.position);
        }
        else
        {
            var currentPos = currentDrawing.GetPosition(index);
            if (Vector3.Distance(currentPos, tip.position) > 0.01f)
            {
                index++;
                currentDrawing.positionCount = index + 1;
                currentDrawing.SetPosition(index, tip.position);
            }
        }
    }

    private void SwitchColor()
    {
        if (currentColorIndex == penColors.Length - 1)
        {
            currentColorIndex = 0;
        }
        else
        {
            currentColorIndex++;
        }
        tipMaterial.color = penColors[currentColorIndex];
    }
}
