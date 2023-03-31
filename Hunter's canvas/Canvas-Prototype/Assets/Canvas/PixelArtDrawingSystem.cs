using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils; 

public class PixelArtDrawingSystem : MonoBehaviour 
{

    public static PixelArtDrawingSystem Instance { get; private set; }

    

    [SerializeField] private PixelArtDrawingSystemVisual pixelArtDrawingSystemVisual;
    [SerializeField] private Texture2D colorTexture2D;
    private Grid<GridObject> grid;
    private float CellSize = .5f;
    private string PenType ="Circle";//Circle   Square
    private Vector2 colorUV;

    

    private void Awake() 
    {
        Instance = this;

        grid = new Grid<GridObject>(100, 100, CellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
        colorUV = new Vector2(0, 0);
        
    }

    private void Start() 
    {
        pixelArtDrawingSystemVisual.SetGrid(grid);
    }

    private void Update() 
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

            
            

            int penSize = GetPenSizeInt();
            if(colorUV == new Vector2(0, 1)){
                for (int i=0; i<4; ++i)
                {
                    int ix=1;
                    int iy=1;
                    if(i==1) ix=-1;
                    else if(i==2) iy=-1;
                    else if(i==3) 
                    {
                        ix=-1;
                        iy=-1;
                    }
                    for (int x = 0; (double)x < (((double)penSize)+1.0) / 2.0; x++) {
                        for (int y = 0; (double)y < (((double)penSize)+1.0) / 2.0; y++) {
                            Vector3 gridWorldPosition = mousePosition + new Vector3(x*ix, y*iy) * CellSize;
                            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
                        
                            if (gridObject != null) {
                                gridObject.SetColorUV(colorUV);
                            }
                        }
                    }
                
                
                
                grid.GetGridObject(mousePosition).SetColorUV(colorUV);
                } 
            }

            if(colorUV == new Vector2(.5f, 1)){
            Vector3 vec = mousePosition;
            vec.x +=penSize;
            vec.y +=penSize;
            for (int i=0; i<4; ++i)
            {
                 int ix=1;
                 int iy=1;
                if(i==1) ix=-1;
                else if(i==2) iy=-1;
                else if(i==3) 
                {
                    ix=-1;
                    iy=-1;
                }
                for (int x = 0; (double)x < (((double)penSize)+1.0) / 2.0; x++) {
                    for (int y = 0; (double)y < (((double)penSize)+1.0) / 2.0; y++) {

                        if(2.0*Math.PI*(double)penSize+1.0>x*x+y*y)
                        {
                            Vector3 gridWorldPosition = mousePosition + new Vector3(x*ix, y*iy) * CellSize;
                            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
                        
                            if (gridObject != null) {
                                gridObject.SetColorUV(colorUV);
                            }
                        }
                    }
                }
            }
            }

            if(PenType=="StampGrid"){
            Vector3 vec = mousePosition;// interesting design
            vec.x +=penSize;
            vec.y +=penSize;
            for (int i=0; i<4; ++i)
            {
                 int ix=1;
                 int iy=1;
                if(i==1) ix=-1;
                else if(i==2) iy=-1;
                else if(i==3) 
                {
                    ix=-1;
                    iy=-1;
                }
                for (int x = 0; (double)x < (((double)penSize)+1.0) / 2.0; x++) {
                    for (int y = 0; (double)y < (((double)penSize)+1.0) / 2.0; y++) {

                        if(2.0*Math.PI*(double)penSize>=Math.Sqrt(Math.Sin(x)+Math.Cos(y))){
                            Vector3 gridWorldPosition = mousePosition + new Vector3(x*ix, y*iy) * CellSize;
                            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
                        
                            if (gridObject != null) {
                                gridObject.SetColorUV(colorUV);
                            }
                        }
                    }
                }
            }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.B)) colorUV = new Vector2(.5f, 1);
        if (Input.GetKeyDown(KeyCode.R)) colorUV = new Vector2(0, 1);
        if (Input.GetKeyDown(KeyCode.G)) colorUV = new Vector2(.3f, 1f);
        if (Input.GetKeyDown(KeyCode.W)) colorUV = new Vector2(0, 0);
        

    }



    private int GetPenSizeInt() 
    {
        return 25;
    }

    public class GridObject 
    {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private Vector2 colorUV;

        public GridObject(Grid<GridObject> grid, int x, int y) 
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetColorUV(Vector2 colorUV) 
        {
            this.colorUV = colorUV;
            grid.TriggerGridObjectChanged(x, y);
        }

        public Vector2 GetColorUV() 
        {
            return colorUV;
        }

        public override string ToString() 
        {
            return ((int)colorUV.x).ToString();
        }

    }

}
