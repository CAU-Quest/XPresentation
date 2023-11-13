using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(x => OpenKeyboard());
    }

    public void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        Vector3 direction = XRUIManager.Instance.positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = XRUIManager.Instance.positionSource.position + direction * XRUIManager.Instance.distance 
                                                                              + Vector3.up * XRUIManager.Instance.verticalOffset;
        
        NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
        
        SetCarteColorAlpha(1);
        NonNativeKeyboard.Instance.OnClosed += Instance_OnClosed;
    }

    private void Instance_OnClosed(object sender, System.EventArgs e)
    {
        SetCarteColorAlpha(0);
        NonNativeKeyboard.Instance.OnClosed -= Instance_OnClosed;
    }
    
    public void SetCarteColorAlpha(float value)
    {
        inputField.customCaretColor = true;
        Color caretColor = inputField.caretColor;
        caretColor.a = value;
        inputField.caretColor = caretColor;
    }
}
