using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class BuildPen : MonoBehaviour
{
    [Header("BuildPen Properties")]

    [SerializeField]
    private Transform tip;

    [SerializeField]
    private Material drawingMaterial;

    [SerializeField]
    private Material tipMaterial;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float penWidth = 0.01f;

    [SerializeField]
    private Color[] penColors;

    [Header("Hands & Grabbable")]

    [SerializeField]
    private HandGrabInteractor rightHandInteractor;

    [SerializeField]
    private HandGrabInteractor leftHandInteractor;

    [SerializeField]
    private HandGrabInteractable handGrabInteractable;

    //private LineRenderer currentDrawing;
    private MeshRenderer currentBuild;
    private int index;
    private int currentColorIndex;
    private bool isDrawing;

    private void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];
    }

    private void Update()
    {
        bool isRightHandDrawing = rightHandInteractor.HasSelectedInteractable && rightHandInteractor.SelectedInteractable == handGrabInteractable;
        bool isLeftHandDrawing = leftHandInteractor.HasSelectedInteractable && leftHandInteractor.SelectedInteractable == handGrabInteractable;

        if (Input.GetKeyDown(KeyCode.D)) // Replace with appropriate button check for your setup
        {
            isDrawing = !isDrawing;
        }

        if (isDrawing && (isRightHandDrawing || isLeftHandDrawing))
        {
            Draw();
        }
        else if (currentBuild != null)
        {
            currentBuild = null;
        }
        else if (Input.GetKeyDown(KeyCode.C)) // Replace with appropriate button check for your setup
        {
            SwitchColor();
        }
    }

    private void Draw()
    {
        //if (currentBuild == null)
        //{
        //    index = 0;

        //    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    sphere.GetComponent<MeshRenderer>().material.color = penColors[currentColorIndex];
        //    sphere.transform.localScale = new Vector3(penWidth, penWidth, penWidth);
        //    sphere.transform.position = new Vector3(0, 1.5f, 0);

        //    //currentDrawing = new GameObject().AddComponent<LineRenderer>();
        //    //currentDrawing.material = drawingMaterial;
        //    //currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
        //    //currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
        //    //currentDrawing.positionCount = 1;
        //    //currentDrawing.SetPosition(0, tip.position);
        //}
        //else
        //{
        //    var currentPos = currentDrawing.GetPosition(index);
        //    if (Vector3.Distance(currentPos, tip.position) > 0.01f)
        //    {
        //        index++;
        //        currentDrawing.positionCount = index + 1;
        //        currentDrawing.SetPosition(index, tip.position);
        //    }
        //}


        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.GetComponent<MeshRenderer>().material.color = tipMaterial.color;
        sphere.transform.localScale = new Vector3(penWidth, penWidth, penWidth);
        sphere.transform.position = tip.position;

        //GameObject _SpherePrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //GameObject sphere = new GameObject();

        //_Sphere.AddComponent<MeshFilter>();

        //MeshRenderer nRenderer = _Sphere.AddComponent<MeshRenderer>();
        //nRenderer.material = 

        //nMat.color = Color.red;
        //nRenderer.material = nMat;

        //_Sphere.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        //_Sphere.transform.position = new Vector3(0, 0, 0);


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
