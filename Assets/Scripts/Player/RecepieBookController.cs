using System;
using UnityEngine;

public class RecepieBookController : MonoBehaviour
{
    private Vector3 upPos;
    private Vector3 downPos;
    private Vector3 targPos;
    [SerializeField] GameObject canvas;
    void Start()
    {
        EventBus.RequestEvent("ShiftPressed", true).ping += RaiseRecepieBook;
        EventBus.RequestEvent("ShiftReleased", true).ping += LowerRecepieBook;

        downPos = canvas.transform.position - new Vector3(0,Screen.height,0);
        upPos = canvas.transform.position;

        canvas.transform.position = downPos;
        targPos = downPos;

    }
    private void RaiseRecepieBook()
    {
        targPos = upPos;
    }
    private void LowerRecepieBook()
    {
        targPos = downPos;
    }

    void Update()
    {
        canvas.transform.position = Vector3.Lerp(canvas.transform.position, targPos, Time.deltaTime * 10f);
    }
}
