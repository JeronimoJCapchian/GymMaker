using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSystem : MonoBehaviour
{
    [SerializeField] ObjectDatabase dataBase;
    [SerializeField] Renderer floorRenderer;
    [SerializeField] int previewsIndex;
    [SerializeField] int selectedFloorIndex;

    private void Start() {
        previewsIndex = -1;
    }

    public void ChangeFloor(int floorIndex)
    {
        if(floorIndex == previewsIndex)
            return;

        selectedFloorIndex = dataBase.floors.FindIndex(data => data.ID == floorIndex);
        if (selectedFloorIndex < 0)
        {
            Debug.LogWarning($"No ID Found{floorIndex}");
            return;
        }

        floorRenderer.material = dataBase.floors[floorIndex].Material;
        previewsIndex = floorIndex;

        //gridVisualization.SetActive(true);
        // preview.StartShowingPlacementPreview(
        //     dataBase.machines[selectedFloorIndex].Prefab,
        //     dataBase.machines[selectedFloorIndex].Size);

        // inputManager.OnClicked += PlaceStructure;
        // inputManager.OnExit += StopPlacement;
    }
}
