using UnityEngine;
using System.Collections;



[ExecuteInEditMode]
public class GUI_Bar : MonoBehaviour 
{
    public float MaxValue = 15;
    public float Value = 10;

    public int Height = 16;
    public int SingleValueWidth = 4;

    public Vector2 ScreenPos;
    
    public HorizontalOrientation HorizontalOrientation = HorizontalOrientation.Left;
    public VerticalOrientation VerticalOrientation = VerticalOrientation.Top;

    public GUIStyle StyleBG, StyleValueFill;
    public GUIContent ContentBG, ContentFill;

    void OnGUI()
    {
        float width = SingleValueWidth * MaxValue;
        float minWidth = SingleValueWidth * Value;

        GUI.BeginGroup(GUIHelper.GetScreenRectByOrientation(ScreenPos, HorizontalOrientation, VerticalOrientation, width, Height));//, Style);

        GUI.Box(new Rect(0, 0, width, Height), ContentBG, StyleBG);

        if(HorizontalOrientation == global::HorizontalOrientation.Right)
            GUI.BeginGroup(new Rect(width-minWidth, 0, minWidth, Height));
        else
            GUI.BeginGroup(new Rect(0, 0, minWidth, Height));

        GUI.Box(new Rect(0, 0, minWidth, Height), ContentFill, StyleValueFill);
        
        GUI.EndGroup();
        GUI.EndGroup();
    }

    
}

 ////public GUITexture BG, Overlay, Fill;
 //   public Texture TexBG, TexOverlay, TexFill;
 //   public Color BorderColor = Color.black;
 //   public int Border = 2;
 //   public int Height = 24;
 //   public int SingleWidth = 6;

 //   public float Value { get { return nrValue; } set { nrValue = value; SetBars(); } }
 //   public float MaxValue { get { return maxValue; } set { maxValue = value; SetBars(); } }

 //   private float nrValue = 10f;
 //   private float maxValue = 15f;
 //   private Rect bgBar, valueBar;


 //   void Start()
 //   {
 //       SetBars();
 //   }

 //   private void SetBars()
 //   {
 //       if (nrValue > maxValue)
 //           nrValue = maxValue;
 //       float x = transform.position.x;
 //       float y = transform.position.y;
 //       bgBar = new Rect(x, y, SingleWidth * maxValue, Height);
 //       valueBar = new Rect(x, y, SingleWidth * nrValue, Height);
 //       Debug.Log(bgBar.ToString() + " " + valueBar.ToString());
 //   }

 //   void OnGUI()
 //   {
 //       GUI.DrawTexture(bgBar, TexBG, ScaleMode.StretchToFill);
 //       GUI.DrawTexture(valueBar, TexFill, ScaleMode.StretchToFill);
 //   }
