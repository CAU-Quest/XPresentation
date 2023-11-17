using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class PreviewCube : MonoBehaviour
{
    public SnapInteractor snapInteractor;

    public TextMeshProUGUI textNumber;

    public void SetNumber(int number)
    {
        textNumber.SetText(number.ToString());
    }
}
