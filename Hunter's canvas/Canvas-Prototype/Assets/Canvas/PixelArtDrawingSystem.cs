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

    private void bucketY(Vector3 mousePosition, Vector2 colorOrigin){
        Vector3 gridWorldPosition = mousePosition * CellSize;
        GridObject gridObject = grid.GetGridObject(gridWorldPosition);
        if (gridObject != null){
            if(colorOrigin!=colorUV){
                

                int penSize = GetPenSizeInt();

                if ( gridObject != null && colorOrigin == gridObject.GetColorUV()) {
                    gridObject.SetColorUV(colorUV);
                    grid.GetGridObject(mousePosition).SetColorUV(colorUV);
                }

                gridWorldPosition = mousePosition + new Vector3(0, 1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject.GetColorUV() == colorOrigin && gridObject != null){
                    bucketY( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin);
                }

                gridWorldPosition = mousePosition + new Vector3(0, -1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject.GetColorUV() == colorOrigin && gridObject != null){
                    bucketY( mousePosition + new Vector3(0, -1) * CellSize,  colorOrigin);
                }


            }
        }
    }
    // private void bucketX(Vector3 mousePosition, Vector2 colorOrigin){
    //     int penSize = GetPenSizeInt();

    //     bucketY( mousePosition + new Vector3(0, 1),  colorOrigin);
    //     bucketY( mousePosition + new Vector3(0, 1),  colorOrigin);
    // }
    

    private void bucket(Vector3 mousePosition, Vector2 colorOrigin){
        if(colorOrigin!=colorUV){

        int penSize = GetPenSizeInt();
        
            
        Vector3 gridWorldPosition = mousePosition ;
        GridObject gridObject = grid.GetGridObject(gridWorldPosition);
        if (gridObject != null){
            
            
            if ( gridObject != null && colorOrigin ==gridObject.GetColorUV()) {
                gridObject.SetColorUV(colorUV);
                grid.GetGridObject(mousePosition).SetColorUV(colorUV);

                Vector3 gridWorldPosition1 = mousePosition + new Vector3(1, 0) * CellSize;
                GridObject gridObject1 = grid.GetGridObject(gridWorldPosition1);
                if(gridObject1 != null && colorOrigin == gridObject1.GetColorUV()){
                    bucket(mousePosition + new Vector3(1, 0) * CellSize,  colorOrigin);// right
                }

                Vector3 gridWorldPosition2 = mousePosition + new Vector3(0, 1) * CellSize;
                GridObject gridObject2 = grid.GetGridObject(gridWorldPosition2);
                if(gridObject2 != null && colorOrigin ==gridObject2.GetColorUV()){
                    bucket(mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin);//up
                }

                Vector3 gridWorldPosition3 = mousePosition + new Vector3(-1, 0) * CellSize;
                GridObject gridObject3 = grid.GetGridObject(gridWorldPosition3);
                if(gridObject3 != null && colorOrigin == gridObject3.GetColorUV()){
                    bucket(mousePosition + new Vector3(0, -1) * CellSize, colorOrigin);//down
                }
                
                Vector3 gridWorldPosition4 = mousePosition + new Vector3(-1, 0) * CellSize;
                GridObject gridObject4 = grid.GetGridObject(gridWorldPosition4);
                if(gridObject4 != null && colorOrigin == gridObject4.GetColorUV()){
                    bucket(mousePosition + new Vector3(-1, 0) * CellSize, colorOrigin);//left
                }
                // Vector3 gridWorldPosition1 = mousePosition + new Vector3(0, 0) * CellSize;
                // GridObject gridObject1 = grid.GetGridObject(gridWorldPosition1);
                // for (int x = 0; (double)x < 100; x++) {
                //     gridWorldPosition1 = mousePosition + new Vector3(x, 0) * CellSize;
                //     gridObject1 = grid.GetGridObject(gridWorldPosition1);
                //     if(colorOrigin != gridObject1.GetColorUV()){
                //         y=99;
                //     }
                //     for (int y = 0; (double)y < 100; y++) {
                //         gridWorldPosition1 = mousePosition + new Vector3(x, y) * CellSize;
                //         gridObject1 = grid.GetGridObject(gridWorldPosition1);
                    
                //         if (gridObject != null && colorOrigin == gridObject1.GetColorUV()) {
                //             gridObject1.SetColorUV(colorUV);
                //         }
                //         else if(colorOrigin != gridObject1.GetColorUV()){
                //             y=99;
                //         }
                //     }
                // }
                
                
            }
            
        }
        }
    }
        
    


    


    private void Update() 
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

            
            

            int penSize = GetPenSizeInt();

            if(colorUV == new Vector2(.3f, 1f)){
                Vector3 gridWorldPositionOrigin = mousePosition * CellSize;
                GridObject gridObjectOrigin = grid.GetGridObject(gridWorldPositionOrigin);
                bucketY(mousePosition, gridObjectOrigin.GetColorUV());
            }


            if(colorUV == new Vector2(0, 1)||colorUV == new Vector2(0, 0)){
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


    public Vector2 GetColorUV() {
        return colorUV;
    }
    private int GetPenSizeInt() 
    {
        return 25;
    }
    private void SetPenSizeInt() 
    {
        
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
