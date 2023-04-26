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
    
    

    [SerializeField] private TMPro.TMP_InputField penBox;
    
    private void Awake() {
        penBox.text="15";
        Instance = this;

      

        BucketFill.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Bucket" );
        });

        Pen.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Pen" );
        });

        Eraser.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetToolType("Eraser" );
        });

        PenSquare.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetPenType("Square" );
        });

        PenCircle.onClick.AddListener(() => {
            PixelArtDrawingSystem.Instance.SetPenType("Circle" );
        });

    }
    private void Update() {
        if(penBox.text != null && penBox.text != "")
        {
            PixelArtDrawingSystem.Instance.SetPenSizeInt(int.Parse(penBox.text));
        }
        
    }
    
}
