using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSystem : MonoBehaviour
{
    [SerializeField] ObjectDatabase dataBase;
    [SerializeField] Renderer floorRenderer;
    [SerializeField] Renderer wallRenderer;
    [SerializeField] int previewsIndex;
    [SerializeField] int previewsEnum;
    [SerializeField] int selectedFloorIndex;

    private void Start() {
        previewsIndex = -1;
    }

    public void ChangeFloor(int floorIndex, Panel.MeshToPaint mesh)
    {
        if(floorIndex == previewsIndex && previewsEnum.Equals(mesh))
            return;

        selectedFloorIndex = dataBase.floors.FindIndex(data => data.ID == floorIndex);
        if (selectedFloorIndex < 0)
        {
            Debug.LogWarning($"No ID Found{floorIndex}");
            return;
        }

        if(mesh.Equals(Panel.MeshToPaint.Floor)) floorRenderer.material = dataBase.floors[floorIndex].Material;
        else wallRenderer.material = dataBase.floors[floorIndex].Material;
        previewsIndex = floorIndex;
        previewsEnum = (int)mesh;

        //gridVisualization.SetActive(true);
        // preview.StartShowingPlacementPreview(
        //     dataBase.machines[selectedFloorIndex].Prefab,
        //     dataBase.machines[selectedFloorIndex].Size);

        // inputManager.OnClicked += PlaceStructure;
        // inputManager.OnExit += StopPlacement;
    }
}
