using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTools : MonoBehaviour
{
    public static CanvasTools Instance { get; private set; }


    [SerializeField] private Texture2D colorsTexture;


    [SerializeField] private Image selectedColorImage;
    [SerializeField] private Image  toolBoxImage;
   
 
    [SerializeField] private Button BucketFill;
    [SerializeField] private Button Pen;
    [SerializeField] private Button Eraser;
    [SerializeField] private Button PenSquare;
    [SerializeField] private Button PenCircle;
    

    [SerializeField] private Slider penBox;


    
    private void Start() {
            PixelArtDrawingSystem.Instance.OnColorChanged += PixelArtDrawingSystem_OnColorChanged;
            GameBehavior.Instance.OnTookTurn += UpdateVisibility_Event;

            UpdateSelectedColor();
        }

    private void Awake() {
        
        Instance = this;

        penBox.minValue = 1;
        penBox.maxValue = 9;

        penBox.value = 4;

      

        BucketFill.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Bucket" );
            DisableButton("Bucket");
            EnableButton("Pen");
            EnableButton("Eraser");
           
        });

        Pen.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Pen" );
            DisableButton("Pen");
            EnableButton("Bucket");
            EnableButton("Eraser");
         
        });

        Eraser.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Eraser" );
            DisableButton("Eraser");
            EnableButton("Bucket");
            EnableButton("Pen");
            
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

       
      
        DisableButton();
    }
    private void Update() {
        
        
        PixelArtDrawingSystem.Instance.SetPenSizeInt((int) penBox.value);
        
        
    }

    public void DisableButton (string button) {
       
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
      


       
    }
    public void EnableButton (string button) {
     
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
    private void EnableButton(){
        selectedColorImage.enabled = true;
        BucketFill.gameObject.SetActive(true);
        Pen.gameObject.SetActive(true);
        Eraser.gameObject.SetActive(true);
        PenSquare.gameObject.SetActive(true);
        PenCircle.gameObject.SetActive(true);
        toolBoxImage.enabled = true;

        penBox.gameObject.SetActive(true);
    }
    private void DisableButton(){
        selectedColorImage.enabled = false;
        BucketFill.gameObject.SetActive(false);
        Pen.gameObject.SetActive(false);
        Eraser.gameObject.SetActive(false);
        PenSquare.gameObject.SetActive(false);
        PenCircle.gameObject.SetActive(false);
        penBox.gameObject.SetActive(false);
        toolBoxImage.enabled = false;
    }
    
    
}
