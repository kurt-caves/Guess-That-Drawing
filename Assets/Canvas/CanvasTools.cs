using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTools : MonoBehaviour
{
    public static CanvasTools Instance { get; private set; }


    //added
    [SerializeField] private Texture2D colorsTexture;
    [SerializeField] private Button smallButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button largeButton;

     private Image selectedColorImage;

 
    [SerializeField] private Button BucketFill;
    [SerializeField] private Button Pen;
    [SerializeField] private Button Eraser;
    [SerializeField] private Button PenSquare;
    [SerializeField] private Button PenCircle;
    //[SerializeField] private Button undo;
    //[SerializeField] private Button redo;
    [SerializeField] private Button Picker;
    

    [SerializeField] private Slider penBox;
   private String colorButton="SelectedColor";

    //[SerializeField] private Texture2D colorTexture2D;
    [SerializeField] private Vector2 colorUV;
    
    private void Start() {
            PixelArtDrawingSystem.Instance.OnColorChanged += PixelArtDrawingSystem_OnColorChanged;
            GameBehavior.Instance.OnTookTurn += UpdateVisibility_Event;

            UpdateSelectedColor();
        }

    private void Awake() {
        
        Instance = this;
        //added
        smallButton.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetCursorSize(PixelArtDrawingSystem.CursorSize.Small);
        });

       mediumButton.onClick.AddListener(() => {
           PixelArtDrawingSystem.Instance.SetCursorSize(PixelArtDrawingSystem.CursorSize.Medium);
        });

        largeButton.onClick.AddListener(() => {
           PixelArtDrawingSystem.Instance.SetCursorSize(PixelArtDrawingSystem.CursorSize.Large);
        });
        
        selectedColorImage = transform.Find(colorButton).GetComponent<Image>();


     
       // CanvasTools.Instance.DisableButton("undo");
        CanvasTools.Instance.DisableButton("Pen");
        CanvasTools.Instance.DisableButton("Circle");

        
        // redo.onClick.AddListener(() => {
        //     PixelArtDrawingSystem.Instance.pushHistory();
        // });
        // undo.onClick.AddListener(() => {
        //     PixelArtDrawingSystem.Instance.pullHistory();
        // });

        
        Picker.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Picker" );
            EnableButton("Bucket");
            EnableButton("Pen");
            EnableButton("Eraser");
            DisableButton("Picker");
        });

        BucketFill.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Bucket" );
            DisableButton("Bucket");
            EnableButton("Pen");
            EnableButton("Eraser");
            EnableButton("Picker");
        });

        Pen.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Pen" );
            DisableButton("Pen");
            EnableButton("Bucket");
            EnableButton("Eraser");
            EnableButton("Picker");
        });

        Eraser.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Eraser" );
            DisableButton("Eraser");
            EnableButton("Bucket");
            EnableButton("Pen");
            EnableButton("Picker");
        });

        PenSquare.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetPenType("Square" );
            DisableButton("Square");
            EnableButton("Circle");

        });

        PenCircle.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetPenType("Circle" );
            DisableButton("Circle");
            EnableButton("Square");
        });
      

    }
    private void Update() {
        
        
        PixelArtDrawingSystem.Instance.SetPenSizeInt((int) penBox.value);
        
        
    }

    public void DisableButton (string button) {
        // if(button == "undo"){
        //     undo.interactable = false;
        // }
        // if(button == "redo"){
        //     redo.interactable = false;
        // }
        if(button == "Square"){
            PenSquare.interactable = false;
        }
        if(button == "Circle"){
            PenCircle.interactable = false;
        }
        if(button == "Eraser"){
            Eraser.interactable = false;
        }
        if(button == "Pen"){
            Pen.interactable = false;
        }
        if(button == "Bucket"){
            BucketFill.interactable = false;
        }
        if(button == "Picker"){
            Picker.interactable = false;
        }


       
    }
    public void EnableButton (string button) {
        // if(button == "undo"){
        //     undo.interactable = true;
        // } 
        // if(button == "redo"){
        //     redo.interactable = true;
        // }
        if(button == "Square"){
            PenSquare.interactable = true;
        }
        if(button == "Circle"){
            PenCircle.interactable = true;
        }
        if(button == "Eraser"){
            Eraser.interactable = true;
        }
        if(button == "Pen"){
            Pen.interactable = true;
        }
        if(button == "Bucket"){
            BucketFill.interactable = true;
        }
        if(button == "Picker"){
            Picker.interactable = true;
        }
      
    }



           private void UpdateVisibility_Event(object sender, EventArgs e) {
            UpdateVisibility();
        }

       
       
     private void UpdateVisibility() {
        
            if(PlayerList.Instance.getIsArtist()){
                EnableButton();
            }
            else{
                DisableButton();
            }
        }

        private void PixelArtDrawingSystem_OnColorChanged(object sender, System.EventArgs e) {
            UpdateSelectedColor();
        }

        private void UpdateSelectedColor() {
            Vector2 pixelCoordinates = PixelArtDrawingSystem.Instance.GetColorUV();
            pixelCoordinates.x *= colorsTexture.width;
            pixelCoordinates.y *= colorsTexture.height;
            selectedColorImage.color = colorsTexture.GetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y);
        }

         public void DisableButton () {
            selectedColorImage.enabled = false;
            smallButton.gameObject.SetActive(false);
            mediumButton.gameObject.SetActive(false);
            largeButton.gameObject.SetActive(false);
          
        }

        public void EnableButton () {
            selectedColorImage.enabled = true;
            smallButton.gameObject.SetActive(true);
            mediumButton.gameObject.SetActive(true);
            largeButton.gameObject.SetActive(true);
        }
    
}
