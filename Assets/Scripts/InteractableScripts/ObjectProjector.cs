using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProjector : InteractableScript
{
    // Start is called before the first frame update

    public Light _light;
    float _prevIntensity = 0;
    public AnimationCurve _intensityOn;
    bool _isOn;
    bool _isChangingState;
    float _timer;
    public float _duration = 5;


    public override void MyStart()
    {
        _isOn = _light.intensity != 0;
        _isChangingState = false;
        _prevIntensity = _light.intensity;
    }

    public override void PlayerInteraction()
    {
        if(!_isChangingState)
        {
            base.PlayerInteraction();
            _timer = 0;
            _isChangingState = true;
        }
    }

    public void Update()
    {
        if(_isChangingState)
        {
            _timer += Time.deltaTime;

            float trueDuration = _isOn ? 0.5f : _duration;
            float percent = _timer / trueDuration;
            if (!_isOn)
            {
                _light.intensity = _intensityOn.Evaluate(percent) * _prevIntensity;
            }
            if(percent >= 1)
            {
                if (_isOn) _light.intensity = 0;

                _isChangingState = false;
                _isOn = !_isOn;
            }
        }
    }

}
