using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelArtDrawingSystemVisual : MonoBehaviour {

    [SerializeField] private PixelArtDrawingSystem pixelArtDrawingSystem;

    private Grid<PixelArtDrawingSystem.GridObject> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Start() {
        SetGrid(pixelArtDrawingSystem.GetGrid());
    }

    public void SetGrid(Grid<PixelArtDrawingSystem.GridObject> grid) {
        this.grid = grid;
        UpdateVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<PixelArtDrawingSystem.GridObject>.OnGridObjectChangedEventArgs e) {
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateVisual();
        }
    }

    private void UpdateVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                PixelArtDrawingSystem.GridObject gridObject = grid.GetGridObject(x, y);
                Vector2 gridUV00, gridUV11;

                gridUV00 = gridObject.GetColorUV();
                gridUV11 = gridObject.GetColorUV();
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridUV00, gridUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}
