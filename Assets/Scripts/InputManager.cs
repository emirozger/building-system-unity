using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask buildingLayerMask;
    private Vector3 lastPosition;
    public event Action onClicked, onExit;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClicked?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onExit?.Invoke();
        }
    }

    public bool isPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 GetSelectedMapPosition()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = mainCam.nearClipPlane;
        Ray ray = mainCam.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, buildingLayerMask))
            lastPosition = hit.point;
        return lastPosition;
    }
}