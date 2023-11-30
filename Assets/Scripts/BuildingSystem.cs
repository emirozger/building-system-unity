using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDatabase database;
    private int selectedObjectIndex = -1;
    [SerializeField] private GameObject gridVisualization;
    private GridData floorData, furnitureData;

    private List<GameObject> buildingItems = new List<GameObject>();
    [SerializeField] private PreviewSystem previewSystem;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;


    void Start()
    {
        StopBuilding();
        floorData = new GridData();
        furnitureData = new GridData();
    }

    public void StartBuilding(int ID)
    {
        StopBuilding();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID Found {ID}");
            return;
        }

        gridVisualization.SetActive(true);
        previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
            database.objectsData[selectedObjectIndex].Size);
        inputManager.onClicked += PlaceStructure;
        inputManager.onExit += StopBuilding;
    }

    private void PlaceStructure()
    {
        if (inputManager.isPointerOverUI())
            return;
        var mousePosition = inputManager.GetSelectedMapPosition();
        var gridPosition = grid.WorldToCell(mousePosition);

        var buildingValidity = CheckBuildingValidity(gridPosition, selectedObjectIndex);
        if (!buildingValidity)
            return;
        var newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        buildingItems.Add(newObject);
        GridData selectedItemData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedItemData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID, buildingItems.Count - 1);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),false);
    }

    private bool CheckBuildingValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedItemData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        return selectedItemData.CanPlaceObejctAt(gridPosition, database.objectsData[this.selectedObjectIndex].Size);
    }


    private void StopBuilding()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        previewSystem.StopShowingPreview();
        inputManager.onClicked -= PlaceStructure;
        inputManager.onExit -= StopBuilding;
        lastDetectedPosition = Vector3Int.zero;
    }


    void Update()
    {
        if (selectedObjectIndex < 0)
            return;
        var mousePosition = inputManager.GetSelectedMapPosition();
        var gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            var buildingValidity = CheckBuildingValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition),buildingValidity);
            lastDetectedPosition += gridPosition;
        }
        
    }
}