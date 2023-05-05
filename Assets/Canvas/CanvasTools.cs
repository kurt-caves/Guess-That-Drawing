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


    [SerializeField] private Image selectedColorImage;
    [SerializeField] private Image selectedColor1Image;
    private String colorButton="SelectedColor";
 
    [SerializeField] private Button BucketFill;
    [SerializeField] private Button Pen;
    [SerializeField] private Button Eraser;
    [SerializeField] private Button PenSquare;
    [SerializeField] private Button PenCircle;
    [SerializeField] private Button Picker;
    [SerializeField] private Button SelectedColor;
    [SerializeField] private Button SelectedColor1;

    [SerializeField] private Slider penBox;

    
    
    
    private void Start() {
            PixelArtDrawingSystem.Instance.OnColorChanged += PixelArtDrawingSystem_OnColorChanged;
            GameBehavior.Instance.OnTookTurn += UpdateVisibility_Event;

            UpdateSelectedColor();
        }

    private void Awake() {
        
        Instance = this;

        SelectedColor.onClick.AddListener(() => {
            colorButton="SelectedColor";
            PixelArtDrawingSystem.Instance.ChangeColorUV(0);

        });
        SelectedColor1.onClick.AddListener(() => {
            colorButton="SelectedColor1";
            PixelArtDrawingSystem.Instance.ChangeColorUV(1);
        });

        if(colorButton=="SelectedColor"){
            if(transform.Find(colorButton).GetComponent<Image>()!=null)
             selectedColorImage = transform.Find(colorButton).GetComponent<Image>();
        }
        else if(colorButton=="SelectedColor1"){
            if(transform.Find(colorButton).GetComponent<Image>()!=null)
             selectedColor1Image = transform.Find(colorButton).GetComponent<Image>();
        }

     
        CanvasTools.Instance.DisableButton("Pen");
        CanvasTools.Instance.DisableButton("Circle");

        

        

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
            if(colorButton=="SelectedColor"){
                    selectedColorImage.color = colorsTexture.GetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y);
                    
            }
            else if(colorButton=="SelectedColor1"){
                selectedColor1Image.color = colorsTexture.GetPixel((int)pixelCoordinates.x, (int)pixelCoordinates.y);
                
            }
        }
    private void EnableButton(){
        BucketFill.gameObject.SetActive(true);
        Pen.gameObject.SetActive(true);
        Eraser.gameObject.SetActive(true);
        PenSquare.gameObject.SetActive(true);
        PenCircle.gameObject.SetActive(true);
        Picker.gameObject.SetActive(true);
        SelectedColor.gameObject.SetActive(true);
        SelectedColor1.gameObject.SetActive(true);
        penBox.gameObject.SetActive(true);
    }
    private void DisableButton(){
        BucketFill.gameObject.SetActive(false);
        Pen.gameObject.SetActive(false);
        Eraser.gameObject.SetActive(false);
        PenSquare.gameObject.SetActive(false);
        PenCircle.gameObject.SetActive(false);
        Picker.gameObject.SetActive(false);
        SelectedColor.gameObject.SetActive(false);
        SelectedColor1.gameObject.SetActive(false);
        penBox.gameObject.SetActive(false);
    }
    
    
}
