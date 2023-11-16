using Oculus.Interaction;
using UnityEngine;

public class SlideListSnapPoseDelegateRoundedBoxVisual : MonoBehaviour
{
    [SerializeField]
    private SlideListSnapPoseDelegate _listSnapPoseDelegate;

    [SerializeField]
    private RoundedBoxProperties _properties;

    [SerializeField]
    private SnapInteractable _snapInteractable;

    [SerializeField]
    private float _minSize = 0f;

    [SerializeField]
    private ProgressCurve _curve;

    private float _targetWidth = 0;
    private float _startWidth = 0;

    protected virtual void LateUpdate()
    {
        float targetWidth = Mathf.Max(_listSnapPoseDelegate.Size, _minSize);
        if (targetWidth != _targetWidth)
        {
            _targetWidth = targetWidth;
            _curve.Start();
            _startWidth = _properties.Width;
        }

        _properties.Width = Mathf.Lerp(_startWidth, _targetWidth, _curve.Progress());
        _properties.BorderColor =
            _snapInteractable.Interactors.Count != _snapInteractable.SelectingInteractors.Count
                ? new Color(1, 1, 1, 1)
                : new Color(1, 1, 1, 0.5f);
    }
}