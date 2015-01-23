using UnityEngine;
using System.Collections;

public enum HorizontalOrientation { Left, Middle, Right };
public enum VerticalOrientation { Top, Middle, Bottom };

public class GUIHelper
{

    public static Rect GetScreenRectByOrientation(Vector2 scrPos, HorizontalOrientation hor, VerticalOrientation ver, float width, float height)
    {
        float x = Screen.width * scrPos.x;
        float y = Screen.height * scrPos.y;

        float xMin = 0;
        float xMax = 0;
        float yMin = 0;
        float yMax = 0;

        switch (hor)
        {
            case global::HorizontalOrientation.Left:
                xMin = x;
                xMax = x + width;
                break;
            case global::HorizontalOrientation.Middle:
                xMin = x - 0.5f * width;
                xMax = x + 0.5f * width;
                break;
            case global::HorizontalOrientation.Right:
                xMin = x - width;
                xMax = x;
                break;
        }

        switch (ver)
        {
            case global::VerticalOrientation.Top:
                yMin = y;
                yMax = y + height;
                break;
            case global::VerticalOrientation.Middle:
                yMin = y - 0.5f * height;
                yMax = y + 0.5f * height;
                break;
            case global::VerticalOrientation.Bottom:
                yMin = y - height;
                yMax = y;
                break;
        }
        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }
}
