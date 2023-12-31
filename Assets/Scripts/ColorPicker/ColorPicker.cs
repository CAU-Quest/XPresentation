﻿using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;
public class ColorPicker : MonoBehaviour, ISelectedObjectModifierInitializer
{
    /// <summary>
    /// Event that gets called by the ColorPicker
    /// </summary>
    /// <param name="c">received Color</param>
    public delegate void ColorEvent(Color c);

    private static ColorPicker instance;

    //onColorChanged event
    private static ColorEvent onCC;
    //onColorSelected event
    private static ColorEvent onCS;

    //Color before editing
    private static Color32 originalColor;
    //current Color
    private static Color32 modifiedColor;
    private static HSV modifiedHsv = new HSV();

    //useAlpha bool
    private static bool useA;

    private bool interact;

    // these can only work with the prefab and its children
    public RectTransform pivot;
    public RawImage chosenColorImage;
    public RectTransform positionIndicator;
    public Slider mainComponent;
    public Slider rComponent;
    public Slider gComponent;
    public Slider bComponent;
    public Slider aComponent;
    //public InputField hexaComponent;
    public RawImage colorComponent;

    [SerializeField] private RawImage rAImage, rBImage, gAImage, gBImage, bAImage, bBImage, aImage;

    private bool _isSelected;
    
    private void Awake()
    {
        instance = this;
        rComponent.onValueChanged.AddListener(SetR);
        gComponent.onValueChanged.AddListener(SetG);
        bComponent.onValueChanged.AddListener(SetB);
        aComponent.onValueChanged.AddListener(SetA);
    }
    
    public void InitializeProperty(PresentationObject selectedObject)
    {
        Create(selectedObject.Material.color, "", null, null);
    }

    public void FinalizeProperty() { }

    /// <summary>
    /// Creates a new Colorpicker
    /// </summary>
    /// <param name="original">Color before editing</param>
    /// <param name="message">Display message</param>
    /// <param name="onColorChanged">Event that gets called when the color gets modified</param>
    /// <param name="onColorSelected">Event that gets called when one of the buttons done or cancel get pressed</param>
    /// <param name="useAlpha">When set to false the colors used don't have an alpha channel</param>
    /// <returns>
    /// False if the instance is already running
    /// </returns>
    public static bool Create(Color original, string message, ColorEvent onColorChanged, ColorEvent onColorSelected, bool useAlpha = true)
    {   
        if(instance is null)
        {
            Debug.LogError("No Colorpicker prefab active on 'Start' in scene");
            return false;
        }

        originalColor = original;
        modifiedColor = original;
        onCC = onColorChanged;
        onCS = onColorSelected;
        useA = useAlpha;
        instance.gameObject.SetActive(true);
        //instance.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
        instance.aComponent.gameObject.SetActive(useAlpha);
        instance.RecalculateMenu(true);
        //instance.hexaComponent.placeholder.GetComponent<Text>().text = "RRGGBB" + (useAlpha ? "AA" : "");
        return true;

    }

    //called when color is modified, to update other UI components
    private void RecalculateMenu(bool recalculateHSV)
    {
        interact = false;
        if(recalculateHSV)
        {
            modifiedHsv = new HSV(modifiedColor);
        }
        else
        {
            modifiedColor = modifiedHsv.ToColor();
        }
        rComponent.value = modifiedColor.r;
        //rComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.r.ToString();
        gComponent.value = modifiedColor.g;
        //gComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.g.ToString();
        bComponent.value = modifiedColor.b;
        //bComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.b.ToString();
        if (useA)
        {
            aComponent.value = modifiedColor.a;
            //aComponent.transform.GetChild(3).GetComponent<InputField>().text = modifiedColor.a.ToString();
        }
        mainComponent.value = (float)modifiedHsv.H;
        rAImage.color = new Color32(255, modifiedColor.g, modifiedColor.b, 255);
        rBImage.color = new Color32(0, modifiedColor.g, modifiedColor.b, 255);
        gAImage.color = new Color32(modifiedColor.r, 255, modifiedColor.b, 255);
        gBImage.color = new Color32(modifiedColor.r, 0, modifiedColor.b, 255);
        bAImage.color = new Color32(modifiedColor.r, modifiedColor.g, 255, 255);
        bBImage.color = new Color32(modifiedColor.r, modifiedColor.g, 0, 255);
        aImage.color = new Color32(modifiedColor.r, modifiedColor.g, modifiedColor.b, 255);
        
        chosenColorImage.color = new HSV(modifiedHsv.H, 1d, 1d).ToColor();
        positionIndicator.anchorMin = new Vector2((float)modifiedHsv.S, (float)modifiedHsv.V);
        positionIndicator.anchorMax = positionIndicator.anchorMin;
        //hexaComponent.text = useA ? ColorUtility.ToHtmlStringRGBA(modifiedColor) : ColorUtility.ToHtmlStringRGB(modifiedColor);
        colorComponent.color = modifiedColor;
        onCC?.Invoke(modifiedColor);
        interact = true;
    }


    public void OnSelect()
    {
        _isSelected = true;
    }

    public void OnUnselect()
    {
        _isSelected = false;
    }

    private void Update()
    {
        if(!_isSelected) return;

        SetColorAtHSVBox();
    }

    public void SetColorAtHSVBox()
    {
        var activeRayCursorPos = (XRUIManager.Instance.leftRayInteractor.IsSelecting)
            ? XRUIManager.Instance.leftRayInteractor.End
            : XRUIManager.Instance.rightRayInteractor.End;
        
        var activeRayCursorLocalPos = pivot.InverseTransformPoint(activeRayCursorPos);
        var hsvPoint = new Vector2(Mathf.Clamp(activeRayCursorLocalPos.x, -80f, 80f), 
                                    Mathf.Clamp(activeRayCursorLocalPos.y, -80f, 80f));
        modifiedHsv.S = (hsvPoint.x + 80f)/ 160f;
        modifiedHsv.V = (hsvPoint.y + 80f )/ 160f;
        positionIndicator.localPosition = hsvPoint;

        RecalculateMenu(false);
    }

    //gets main Slider value
    public void SetMain(float value)
    {
        if (interact)
        {
            modifiedHsv.H = value;
            RecalculateMenu(false);
        }
    }

    //gets r Slider value
    public void SetR(float value)
    {
        if (interact)
        {
            modifiedColor.r = (byte)value;
            RecalculateMenu(true);
        }
    }
    //gets r InputField value
    public void SetR(string value)
    {
        if(interact)
        {
            modifiedColor.r = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(true);
        }
    }
    //gets g Slider value
    public void SetG(float value)
    {
        if(interact)
        {
            modifiedColor.g = (byte)value;
            RecalculateMenu(true);
        }
    }
    //gets g InputField value
    public void SetG(string value)
    {
        if (interact)
        {
            modifiedColor.g = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(true);
        }
    }
    //gets b Slider value
    public void SetB(float value)
    {
        if (interact)
        {
            modifiedColor.b = (byte)value;
            RecalculateMenu(true);
        }
    }
    //gets b InputField value
    public void SetB(string value)
    {
        if (interact)
        {
            modifiedColor.b = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(true);
        }
    }
    //gets a Slider value
    public void SetA(float value)
    {
        if (interact)
        {
            modifiedHsv.A = (byte)value;
            RecalculateMenu(false);
        }
    }
    //gets a InputField value
    public void SetA(string value)
    {
        if (interact)
        {
            modifiedHsv.A = (byte)Mathf.Clamp(int.Parse(value), 0, 255);
            RecalculateMenu(false);
        }
    }
    //gets hexa InputField value
    public void SetHexa(string value)
    {
        if (interact)
        {
            if (ColorUtility.TryParseHtmlString("#" + value, out Color c))
            {
                if (!useA) c.a = 1;
                modifiedColor = c;
                RecalculateMenu(true);
            }
            else
            {
                //hexaComponent.text = useA ? ColorUtility.ToHtmlStringRGBA(modifiedColor) : ColorUtility.ToHtmlStringRGB(modifiedColor);
            }
        }
    }

    //HSV helper class
    private sealed class HSV
    {
        public double H = 0, S = 1, V = 1;
        public byte A = 255;
        public HSV () { }
        public HSV (double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }
        public HSV (Color color)
        {
            float max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            float min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));

            float hue = (float)H;
            if (min != max)
            {
                if (max == color.r)
                {
                    hue = (color.g - color.b) / (max - min);

                }
                else if (max == color.g)
                {
                    hue = 2f + (color.b - color.r) / (max - min);

                }
                else
                {
                    hue = 4f + (color.r - color.g) / (max - min);
                }

                hue *= 60;
                if (hue < 0) hue += 360;
            }

            H = hue;
            S = (max == 0) ? 0 : 1d - ((double)min / max);
            V = max;
            A = (byte)(color.a * 255);
        }
        public Color32 ToColor()
        {
            int hi = Convert.ToInt32(Math.Floor(H / 60)) % 6;
            double f = H / 60 - Math.Floor(H / 60);

            double value = V * 255;
            byte v = (byte)Convert.ToInt32(value);
            byte p = (byte)Convert.ToInt32(value * (1 - S));
            byte q = (byte)Convert.ToInt32(value * (1 - f * S));
            byte t = (byte)Convert.ToInt32(value * (1 - (1 - f) * S));

            switch(hi)
            {
                case 0:
                    return new Color32(v, t, p, A);
                case 1:
                    return new Color32(q, v, p, A);
                case 2:
                    return new Color32(p, v, t, A);
                case 3:
                    return new Color32(p, q, v, A);
                case 4:
                    return new Color32(t, p, v, A);
                case 5:
                    return new Color32(v, p, q, A);
                default:
                    return new Color32();
            }
        }
    }
}