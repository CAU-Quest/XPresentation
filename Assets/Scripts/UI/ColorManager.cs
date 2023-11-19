using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{
    public static Color Default = new Color(235f / 255f, 235f / 255f, 255f / 255f, 1f);
    public static Color Hover = new Color(149f / 255f, 148f / 255f, 219f / 255f, 1f);
    public static Color Select = new Color(107f / 255f, 104f / 255f, 255f / 255f, 1f);

    public static Color SliderDefault = Hover;
    public static Color SliderHover = Select;
    public static Color SliderSelect = new Color(53f / 255f, 52f / 255f, 130f / 255f, 1f);
    
    public static Color ToggleUnselected = new Color(189f / 255f, 189f / 255f, 219f / 255f, 1f);
    public static Color ToggleSelected = Select;
    public static Color ToggleUnselectedHover = Hover;
    public static Color ToggleSelectedHover =  new Color(80f / 255f, 77f / 255f, 202f / 255f, 1f);
    public static Color ToggleSelect = SliderSelect;

    public static Color BoundBoxEdgeDefault = new Color(170f / 255f, 169f / 255f, 221f / 255f, 1f);
    public static Color BoundBoxEdgeHover = new Color(138f / 255f, 136f / 255f, 231f / 255f, 1f);
    public static Color BoundBoxEdgeSelect = Select;
    public static Color BoundBoxVertexDefault = new Color(98f / 255f, 97f / 255f, 157f / 255f, 1f);
    public static Color BoundBoxVertexHover = new Color(117f / 255f, 115f / 255f, 221f / 255f, 1f);
    public static Color BoundBoxVertexSelect = Select;
}
