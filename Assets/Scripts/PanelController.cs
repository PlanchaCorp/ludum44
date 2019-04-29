using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{

    [SerializeField]
    GameObject pillCanvas;
    [SerializeField]
    GameObject hospitalCanvas;
    // Use this for initialization
    Dictionary<string, GameObject> canvasList;

    private void Start()
    {
        canvasList = new Dictionary<string, GameObject>()
        {
            {pillCanvas.name,pillCanvas },
            {hospitalCanvas.name,hospitalCanvas },
        };
    }
    public void Toggle(string canvasName)
    {
        Vanish();
        Show(canvasList[canvasName]);
    }

    public void Vanish()
    {
        foreach (GameObject canvas in canvasList.Values)
        {
            Hide(canvas);
        }
    }

    public void Hide(GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvas.GetComponent<CanvasGroup>().interactable = false;
    }
    public void Show(GameObject canvas)
    {
        canvas.GetComponent<CanvasGroup>().alpha = 1;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
        canvas.GetComponent<CanvasGroup>().interactable = true;
    }
}
