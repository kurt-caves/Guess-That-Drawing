using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils; 
using Unity.Netcode;

/*
    PixelArtDrawingSystem

    Controls the behaviour of the drawing board
*/
public class PixelArtDrawingSystem : NetworkBehaviour 
{

    public static PixelArtDrawingSystem Instance { get; private set; }
    
    [SerializeField] private PixelArtDrawingSystemVisual pixelArtDrawingSystemVisual;
    [SerializeField] private Texture2D colorTexture2D;
    private Grid<GridObject> grid;
    private Grid<GridObject> colorPicker;
    private int[] gridSize={100,100};
    private float CellSize = 1f;//size of grid pixel
    private string PenType ="Circle";//Circle   Square
    private string ToolType="Pen";// Pen  Eraser  Bucket
    private Vector2 colorUV;//current pen color
    private Vector2[] ColorUVA= new Vector2[5];
    private int PenSize= 15;

    public event EventHandler OnColorChanged;
    

    private void Awake() 
    {
        Instance = this;

        grid = new Grid<GridObject>(100, 100, CellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
        
        colorPicker = new Grid<GridObject>(255, 255, CellSize, new Vector3(-397, -256, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        colorUV = new Vector2(0, 0);
        
    }

    /*
        Creates a new blank drawing board
    */
    public void clearGrid(){
        grid = new Grid<GridObject>(100, 100, CellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
        
       
        colorUV = new Vector2(0, 0);
        PixelArtDrawingSystemVisual.Instance.SetGrid(grid);

    }


    /*
        If the artist clicks on the drawing board, update the canvas  to reflect their actions.
    */
    private void Update() {
            if (PlayerList.Instance.getIsArtist() && Input.GetMouseButton(0)) {
               
                // Paint on grid
                Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            
                UpdateDrawingServerRpc(mouseWorldPosition, PenSize, ToolType, PenType, colorUV, NetworkManager.Singleton.LocalClientId);

                // Color picker
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f)) {
                    colorUV = raycastHit.textureCoord;
                    OnColorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
    }

    /*
        Allows artist to draw on the canvas, and share their  image with the other players.
    */
    private void UpdateDrawing(Vector3 mousePosition, int penSize, string ToolType, string PenType, Vector2 colorUV, ulong senderPlayerId) {
        Vector3 gridWorldPositionA = mousePosition;
        GridObject gridObjectA = grid.GetGridObject(gridWorldPositionA);
        
        if (gridObjectA != null){
          
            if(ToolType == "Bucket"){
                    
                Vector3 gridWorldPositionOrigin = mousePosition;
                GridObject gridObjectOrigin = grid.GetGridObject(gridWorldPositionOrigin);
                edgeBucket(mousePosition, gridObjectOrigin.GetColorUV(), colorUV);
            }
            else{
                if(PenType == "Square"){
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
                            
                                if (gridObject != null && ToolType=="Pen") {
                                    gridObject.SetColorUV(colorUV);
                                }
                                else if(gridObject != null && ToolType=="Eraser"){
                                    gridObject.SetColorUV(new Vector2(0, 0));
                                }
                            }
                        }

                    } 
                }
            }

            if(PenType == "Circle"){
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
                    for (int x = 0; x < ((penSize)+1) / 2; x++) {
                        for (int y = 0; y < ((penSize)+1) / 2; y++) {

                            if(3.14*(double)penSize+1.0 > x*x+y*y)
                            {
                                Vector3 gridWorldPosition = mousePosition + new Vector3(x*ix, y*iy) * CellSize;
                                GridObject gridObject = grid.GetGridObject(gridWorldPosition);
                        
                                if (gridObject != null && ToolType=="Pen") {
                                    gridObject.SetColorUV(colorUV);
                                }
                                else if(gridObject != null && ToolType=="Eraser"){
                                    gridObject.SetColorUV(new Vector2(0, 0));
                                }
                            }
                        }
                    }
                }
            }
        }
  
    }

    //added for multiplayer
    [ClientRpc]
    private void ReceiveUpdateDrawingClientRpc(Vector3 mousePosition,  int penSize, string ToolType, string PenType, Vector2 colorUV, ulong senderPlayerId) {
        UpdateDrawing(mousePosition, penSize, ToolType, PenType, colorUV, senderPlayerId);
        
    }

    //added for multiplayer
    [ServerRpc(RequireOwnership = false)]
    private void UpdateDrawingServerRpc(Vector3 mousePosition,  int penSize, string ToolType, string PenType, Vector2 colorUV, ulong senderPlayerId) {
        ReceiveUpdateDrawingClientRpc(mousePosition, penSize, ToolType, PenType, colorUV,  senderPlayerId);
    }

    public Vector2 GetColorUV() {
        return colorUV;
    }
    public void ChangeColorUV(int UVA) {
        colorUV = ColorUVA[UVA];
    }
 
    public Grid<GridObject> GetGrid() {
        return grid;
    }
    public int GetPenSizeInt() 
    {
        return PenSize;
    }
    public void SetPenSizeInt(int Size) 
    {
        PenSize = Size;
    }
    public string GetPenType() 
    {
        return PenType;
    }
    public void SetPenType(string Type) 
    {
        PenType=Type;
    }
    public string GetToolType() 
    {
        return ToolType;
    }
    public void SetToolType(string Type) 
    {
        ToolType=Type;
    }
    
   
    /*
        Defines the drawing board
    */
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


    /*
  
    ****************************
    Recursive methods to handle the fill bucket tool
    ****************************

    */

    private void edgeBucket(Vector3 mousePosition, Vector2 colorOrigin, Vector2 colorUV){
        if(colorOrigin!=colorUV){
            Vector3 gridWorldPositionRight = mousePosition;
            GridObject gridObjectRight = grid.GetGridObject(gridWorldPositionRight);// defines origin

            

        do {// find edge of space
            if(colorOrigin!=colorUV){
                      
                gridObjectRight.SetColorUV(colorUV);
                grid.GetGridObject(gridWorldPositionRight).SetColorUV(colorUV);//color pixel
                    
                gridWorldPositionRight += new Vector3(1, 0) * CellSize;//next pixel
                gridObjectRight = grid.GetGridObject(gridWorldPositionRight);
            }
        }
        while(gridObjectRight != null && colorOrigin == gridObjectRight.GetColorUV());


        Vector3 gridWorldPositionLeft = mousePosition;
        GridObject gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);// defines origin

        do{// find edge of space
            if(colorOrigin!=colorUV){            
                   gridObjectLeft.SetColorUV(colorUV);
                   grid.GetGridObject(gridWorldPositionLeft).SetColorUV(colorUV);//color pixel

                gridWorldPositionLeft += new Vector3(-1, 0) * CellSize;//next pixel
                gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);
            }
        }
        while(gridObjectLeft != null && colorOrigin == gridObjectLeft.GetColorUV());

        Vector3 gridWorldPositionUp = mousePosition;
        GridObject gridObjectUp = grid.GetGridObject(gridWorldPositionUp);// defines origin

        do{// find edge of space
            if(colorOrigin!=colorUV){            
                   gridObjectUp.SetColorUV(colorUV);
                   grid.GetGridObject(gridWorldPositionUp).SetColorUV(colorUV);//color pixel

                gridWorldPositionUp += new Vector3(0, 1) * CellSize;//next pixel
                gridObjectUp = grid.GetGridObject(gridWorldPositionUp);
            }
        }
        while(gridObjectUp != null && colorOrigin == gridObjectUp.GetColorUV());

        Vector3 gridWorldPositionDown = mousePosition;
        GridObject gridObjectDown = grid.GetGridObject(gridWorldPositionDown);// defines origin

        do{// find edge of space
            if(colorOrigin!=colorUV){            
                   gridObjectDown.SetColorUV(colorUV);
                   grid.GetGridObject(gridWorldPositionDown).SetColorUV(colorUV);//color pixel

                gridWorldPositionDown += new Vector3(0, -1) * CellSize;//next pixel
                gridObjectDown = grid.GetGridObject(gridWorldPositionDown);
            }
        }
        while(gridObjectDown != null && colorOrigin == gridObjectDown.GetColorUV());

        gridWorldPositionRight = gridWorldPositionRight + new Vector3(-1, 1) * CellSize;// prepare to wrap edge
        gridWorldPositionLeft = gridWorldPositionLeft + new Vector3(1, -1) * CellSize;// prepare to wrap edge
        gridWorldPositionUp = gridWorldPositionUp + new Vector3(-1, -1) * CellSize;// prepare to wrap edge
        gridWorldPositionDown = gridWorldPositionDown + new Vector3(1, 1) * CellSize;// prepare to wrap edge
        gridObjectRight = grid.GetGridObject(gridWorldPositionRight);// prepare to wrap edge
        gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);// prepare to wrap edge
        gridObjectUp = grid.GetGridObject(gridWorldPositionUp);// prepare to wrap edge
        gridObjectDown = grid.GetGridObject(gridWorldPositionDown);// prepare to wrap edge

        

        edgeBucketWrapUp( gridWorldPositionRight + new Vector3(0, 0) * CellSize,  colorOrigin, colorUV,gridWorldPositionUp + new Vector3(1, -1) * CellSize);// wrap around top right edge 
        edgeBucketWrapDown( gridWorldPositionLeft + new Vector3(0, 0) * CellSize,  colorOrigin,colorUV, gridWorldPositionDown + new Vector3(-1, 1) * CellSize);// wrap around bottom left edge 
        edgeBucketWrapLeft( gridWorldPositionUp + new Vector3(0, 0) * CellSize,  colorOrigin,colorUV, gridWorldPositionLeft + new Vector3(1, 1) * CellSize);// wrap around top left edge 
        edgeBucketWrapRight( gridWorldPositionDown + new Vector3(0, 0) * CellSize,  colorOrigin,colorUV, gridWorldPositionRight + new Vector3(-1, -1) * CellSize);// wrap around bottom right edge 
        
        if(gridWorldPositionRight != null && gridObjectRight.GetColorUV() == colorOrigin){// checks for failure
        edgeBucket(gridWorldPositionRight + new Vector3(0, 0) * CellSize, colorOrigin, colorUV);
        }
        if(gridWorldPositionLeft != null && gridObjectLeft.GetColorUV() == colorOrigin){// checks for failure
        edgeBucket(gridWorldPositionLeft + new Vector3(0, 0) * CellSize, colorOrigin, colorUV);
        }
        if(gridWorldPositionUp != null && gridObjectUp.GetColorUV() == colorOrigin){// checks for failure
        edgeBucket(gridWorldPositionUp + new Vector3(0, 0) * CellSize, colorOrigin, colorUV);
        }
        if(gridWorldPositionDown != null && gridObjectDown.GetColorUV() == colorOrigin){// checks for failure
        edgeBucket(gridWorldPositionDown + new Vector3(0, 0) * CellSize, colorOrigin, colorUV);
        }
        }

    }


    private void edgeBucketWrapRight(Vector3 mousePosition, Vector2 colorOrigin, Vector2 colorUV, Vector3 edgePoint){
        if(mousePosition!=edgePoint){
            
            Vector3 gridWorldPosition = mousePosition;
            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
            
            if(gridObject != null && gridWorldPosition != edgePoint){
                
                if(colorOrigin!=colorUV){
                    gridObject.SetColorUV(colorUV);
                    grid.GetGridObject(mousePosition).SetColorUV(colorUV);// color pixel
                }

                Vector3 gridWorldPositionRight = mousePosition + new Vector3(1, 0) * CellSize;// find right pixel
                GridObject gridObjectRight = grid.GetGridObject(gridWorldPositionRight);
                Vector3 gridWorldPositionUp = mousePosition + new Vector3(0, 1) * CellSize;// find up pixel
                GridObject gridObjectUp = grid.GetGridObject(gridWorldPositionUp);
                Vector3 gridWorldPositionLeft = mousePosition + new Vector3(-1, 0) * CellSize;// find left pixel
                GridObject gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);
                Vector3 gridWorldPositionDown = mousePosition + new Vector3(0, -1) * CellSize;// find down pixel
                GridObject gridObjectDown = grid.GetGridObject(gridWorldPositionDown);
                
                if(gridWorldPositionDown == edgePoint || gridObjectDown != null && colorOrigin == gridObjectDown.GetColorUV()){
                    edgeBucketWrapDown( mousePosition + new Vector3(0, - 1) * CellSize,  colorOrigin, colorUV, edgePoint);// check down
                }
                else if(gridWorldPositionRight == edgePoint || (gridObjectRight != null && colorOrigin == gridObjectRight.GetColorUV())){
                    edgeBucketWrapRight( mousePosition + new Vector3(1, 0) * CellSize,  colorOrigin, colorUV, edgePoint);// check right
                }
                else if(gridWorldPositionUp == edgePoint || (gridObjectUp != null && colorOrigin == gridObjectUp.GetColorUV())){
                    edgeBucketWrapUp( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV, edgePoint);// check up
                }
                else{
                    edgeBucketWrapLeft( mousePosition + new Vector3(-1, 0) * CellSize,  colorOrigin, colorUV, edgePoint);// check left
                }
                
                gridWorldPosition = mousePosition + new Vector3(0, 1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV);//fill center
                }

                gridWorldPosition = mousePosition + new Vector3(0, -1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, -1) * CellSize,  colorOrigin, colorUV);//fill center
                }
            }   
        }
    }


    private void edgeBucketWrapUp(Vector3 mousePosition, Vector2 colorOrigin, Vector2 colorUV, Vector3 edgePoint){
        if(mousePosition!=edgePoint){
            
            Vector3 gridWorldPosition = mousePosition;
            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
            
            if(gridObject != null && gridWorldPosition != edgePoint){
                
                if(colorOrigin!=colorUV){
                    gridObject.SetColorUV(colorUV);
                    grid.GetGridObject(mousePosition).SetColorUV(colorUV);// color pixel
                }

                Vector3 gridWorldPositionRight = mousePosition + new Vector3(1, 0) * CellSize;// find right pixel
                GridObject gridObjectRight = grid.GetGridObject(gridWorldPositionRight);
                Vector3 gridWorldPositionUp = mousePosition + new Vector3(0, 1) * CellSize;// find up pixel
                GridObject gridObjectUp = grid.GetGridObject(gridWorldPositionUp);
                Vector3 gridWorldPositionLeft = mousePosition + new Vector3(-1, 0) * CellSize;// find left pixel
                GridObject gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);
                Vector3 gridWorldPositionDown = mousePosition + new Vector3(0, -1) * CellSize;// find down pixel
                GridObject gridObjectDown = grid.GetGridObject(gridWorldPositionDown);

                if(gridObjectRight != null && colorOrigin == gridObjectRight.GetColorUV() || gridWorldPositionRight == edgePoint){
                    edgeBucketWrapRight( gridWorldPositionRight,  colorOrigin, colorUV, edgePoint);// check right
                }
                else if(gridObjectUp != null && colorOrigin == gridObjectUp.GetColorUV() || gridWorldPositionUp == edgePoint){
                    edgeBucketWrapUp( gridWorldPositionUp,  colorOrigin, colorUV, edgePoint);// check up
                }
                else if(gridObjectLeft != null && colorOrigin == gridObjectLeft.GetColorUV() || gridWorldPositionLeft == edgePoint){
                    edgeBucketWrapLeft( gridWorldPositionLeft,  colorOrigin, colorUV, edgePoint);// check left
                }
                else{
                    edgeBucketWrapDown( gridWorldPositionDown,  colorOrigin, colorUV, edgePoint);// check down
                }
                
                
                gridWorldPosition = mousePosition + new Vector3(0, 1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV);//fill center
                }

                gridWorldPosition = mousePosition + new Vector3(0, -1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, -1) * CellSize,  colorOrigin, colorUV);//fill center
                }
            }
        }
    }

    private void edgeBucketWrapLeft(Vector3 mousePosition, Vector2 colorOrigin, Vector2 colorUV, Vector3 edgePoint){
        if(mousePosition!=edgePoint){
            
            Vector3 gridWorldPosition = mousePosition;
            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
            
            if(gridObject != null && gridWorldPosition != edgePoint){
                
                if(colorOrigin!=colorUV){
                    gridObject.SetColorUV(colorUV);
                    grid.GetGridObject(mousePosition).SetColorUV(colorUV);// color pixel
                }

                Vector3 gridWorldPositionRight = mousePosition + new Vector3(1, 0) * CellSize;// find right pixel
                GridObject gridObjectRight = grid.GetGridObject(gridWorldPositionRight);
                Vector3 gridWorldPositionUp = mousePosition + new Vector3(0, 1) * CellSize;// find up pixel
                GridObject gridObjectUp = grid.GetGridObject(gridWorldPositionUp);
                Vector3 gridWorldPositionLeft = mousePosition + new Vector3(-1, 0) * CellSize;// find left pixel
                GridObject gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);
                Vector3 gridWorldPositionDown = mousePosition + new Vector3(0, -1) * CellSize;// find down pixel
                GridObject gridObjectDown = grid.GetGridObject(gridWorldPositionDown);
                
                if(gridObjectUp != null && colorOrigin == gridObjectUp.GetColorUV() || gridWorldPositionUp == edgePoint){
                    edgeBucketWrapUp( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV, edgePoint);// check up
                }
                else if(gridObjectLeft != null && colorOrigin == gridObjectLeft.GetColorUV() || gridWorldPositionLeft == edgePoint){
                    edgeBucketWrapLeft( mousePosition + new Vector3(-1, 0) * CellSize,  colorOrigin, colorUV, edgePoint);// check left
                }
                else if(gridObjectDown != null && colorOrigin == gridObjectDown.GetColorUV() || gridWorldPositionDown == edgePoint){
                    edgeBucketWrapDown( mousePosition + new Vector3(0, - 1) * CellSize,  colorOrigin, colorUV, edgePoint);// check down
                }
                else{
                    edgeBucketWrapRight( mousePosition + new Vector3(1, 0) * CellSize,  colorOrigin, colorUV, edgePoint);// check right
                }
                
                
                
                
                
                gridWorldPosition = mousePosition + new Vector3(0, 1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV);//fill center
                }

                gridWorldPosition = mousePosition + new Vector3(0, -1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, -1) * CellSize,  colorOrigin, colorUV);//fill center
                }
            }
        }   
    }

    private void edgeBucketWrapDown(Vector3 mousePosition, Vector2 colorOrigin, Vector2 colorUV,Vector3 edgePoint){
        if(mousePosition!=edgePoint){
            
            Vector3 gridWorldPosition = mousePosition;
            GridObject gridObject = grid.GetGridObject(gridWorldPosition);
            
            if(gridObject != null && gridWorldPosition != edgePoint){
                
                if(colorOrigin!=colorUV){
                    gridObject.SetColorUV(colorUV);
                    grid.GetGridObject(mousePosition).SetColorUV(colorUV);// color pixel
                }

                Vector3 gridWorldPositionRight = mousePosition + new Vector3(1, 0) * CellSize;// find right pixel
                GridObject gridObjectRight = grid.GetGridObject(gridWorldPositionRight);
                Vector3 gridWorldPositionUp = mousePosition + new Vector3(0, 1) * CellSize;// find up pixel
                GridObject gridObjectUp = grid.GetGridObject(gridWorldPositionUp);
                Vector3 gridWorldPositionLeft = mousePosition + new Vector3(-1, 0) * CellSize;// find left pixel
                GridObject gridObjectLeft = grid.GetGridObject(gridWorldPositionLeft);
                Vector3 gridWorldPositionDown = mousePosition + new Vector3(0, -1) * CellSize;// find down pixel
                GridObject gridObjectDown = grid.GetGridObject(gridWorldPositionDown);

                if(gridObjectLeft != null && colorOrigin == gridObjectLeft.GetColorUV() || gridWorldPositionLeft == edgePoint){
                    edgeBucketWrapLeft( gridWorldPositionLeft,  colorOrigin, colorUV, edgePoint);// check left
                }
                else if(gridObjectDown != null && colorOrigin == gridObjectDown.GetColorUV() || gridWorldPositionDown == edgePoint){
                    edgeBucketWrapDown( gridWorldPositionDown,  colorOrigin,colorUV, edgePoint);// check down
                }
                else if(gridObjectRight != null && colorOrigin == gridObjectRight.GetColorUV() || gridWorldPositionRight == edgePoint){
                    edgeBucketWrapRight( gridWorldPositionRight,  colorOrigin,colorUV, edgePoint);// check right
                }
                else{
                    edgeBucketWrapUp( gridWorldPositionUp,  colorOrigin, colorUV, edgePoint);// check up
                }
                
                
                
                
                gridWorldPosition = mousePosition + new Vector3(0, 1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV);//fill center
                }
                gridWorldPosition = mousePosition + new Vector3(0, -1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, -1) * CellSize,  colorOrigin, colorUV);//fill center
                }
            }
        }
    }

    private void bucketY(Vector3 mousePosition, Vector2 colorOrigin, Vector2 colorUV){
        Vector3 gridWorldPosition = mousePosition;
        GridObject gridObject = grid.GetGridObject(gridWorldPosition);
        if (gridObject != null){
            if(colorOrigin!=colorUV){
                gridObject.SetColorUV(colorUV);
                grid.GetGridObject(mousePosition).SetColorUV(colorUV);
                

                gridWorldPosition = mousePosition + new Vector3(0, 1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, 1) * CellSize,  colorOrigin, colorUV);//fill Vertical
                }

                gridWorldPosition = mousePosition + new Vector3(0, -1) * CellSize;
                gridObject = grid.GetGridObject(gridWorldPosition);
                if(gridObject != null && colorOrigin == gridObject.GetColorUV()){
                    bucketY( mousePosition + new Vector3(0, -1) * CellSize,  colorOrigin, colorUV);//fill Vertical
                }
            }
        }
    }

    
}
