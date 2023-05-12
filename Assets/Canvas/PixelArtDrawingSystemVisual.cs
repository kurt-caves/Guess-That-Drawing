using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
        PixelArtDrawingSystemVisual

        Applies updates made from the "PixelArtDrawingSystem" onto the mesh to display the canvas.
    */
    public class PixelArtDrawingSystemVisual : MonoBehaviour {

        [SerializeField] private  PixelArtDrawingSystem drawPixels;

        public static  PixelArtDrawingSystemVisual Instance { get; private set; }

        private Grid< PixelArtDrawingSystem.GridObject> grid;
        private Mesh mesh;
        private bool updateMesh;

        private void Awake() {
            Instance = this; 
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void Start() {
            SetGrid(PixelArtDrawingSystem.Instance.GetGrid());
        }

        public void SetGrid(Grid<PixelArtDrawingSystem.GridObject> grid) {
            this.grid = grid;
            UpdateVisual();

            grid.OnGridObjectChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, Grid< PixelArtDrawingSystem.GridObject>.OnGridObjectChangedEventArgs e) {
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

                     PixelArtDrawingSystem.GridObject gridObject = grid.GetGridObject(x, y);
                    Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();
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
