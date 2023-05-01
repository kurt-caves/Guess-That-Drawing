using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTools : MonoBehaviour
{
    public static CanvasTools Instance { get; private set; }

    [SerializeField] private Button BucketFill;
    [SerializeField] private Button Pen;
    [SerializeField] private Button Eraser;
    [SerializeField] private Button PenSquare;
    [SerializeField] private Button PenCircle;
    [SerializeField] private Button undo;
    [SerializeField] private Button redo;
    [SerializeField] private Button Picker;

    [SerializeField] private TMPro.TMP_InputField penBox;

    [SerializeField] private Texture2D colorTexture2D;
    [SerializeField] private Vector2 colorUV;
    
    private void Start() {
        PixelArtDrawingSystem.Instance.OnColorChanged += PixelArtDrawingSystem_OnColorChanged;

        UpdateSelectedColor();
    }
    private void Awake() {
        penBox.text="15";
        Instance = this;
        CanvasTools.Instance.DisableButton("undo");
        CanvasTools.Instance.DisableButton("Pen");
        CanvasTools.Instance.DisableButton("Circle");

        
        // redo.onClick.AddListener(() => {
        //     PixelArtDrawingSystem.Instance.pushHistory();
        // });
        // undo.onClick.AddListener(() => {
        //     PixelArtDrawingSystem.Instance.pullHistory();
        // });

        // Picker.onClick.AddListener(() => {
        //     PixelArtDrawingSystem.Instance.SetToolType("Picker" );
        //     EnableButton("Bucket");
        //     EnableButton("Pen");
        //     EnableButton("Eraser");
        //     DisableButton("Picker");
        // });

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
        if(penBox.text != null && penBox.text != "")
        {
            PixelArtDrawingSystem.Instance.SetPenSizeInt(int.Parse(penBox.text));
        }
        
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
        // if(button == "Picker"){
        //     Picker.interactable = false;
        // }
       
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
        // if(button == "Picker"){
        //     Picker.interactable = true;
        // }
       
    }

    private void UpdateSelectedColor() {
        Vector2 pixelCoordinate = PixelArtDrawingSystem.Instance.GetColorUV();
        pixelCoordinate.x *= colorTexture2D.width;
        pixelCoordinate.y *= colorTexture2D.height;
        PixelArtDrawingSystem.Instance.changeColorUV(pixelCoordinate);
        //PixelArtDrawingSystem.Instance.SetColorUV(pixelCoordinate);
    }

    private void PixelArtDrawingSystem_OnColorChanged(object sender, System.EventArgs e) {
        UpdateSelectedColor();
    }
    
}
