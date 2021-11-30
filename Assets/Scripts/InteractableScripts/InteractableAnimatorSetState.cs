using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAnimatorSetState : InteractableScript
{
    public Animator _animatorComp;
    public string _stateNameToPlay;
    public string _paramName ="state";
    public int _paramValue;
    bool _isStarted;

    public enum  Mode
    {
        STATE,
        TRANSITION_SET_VALUE,
        TRANSITION_INC_VALUE
    }
    public Mode mode = Mode.STATE;


    // Start is called before the first frame update
    public override void MyStart()
    {
        _isStarted = false;
    }

    public override void PlayerInteraction()
    {
        if (!_isStarted)
        {
            base.PlayerInteraction();
            _isStarted = true;
            
            if( mode == Mode.STATE) _animatorComp.Play(_stateNameToPlay);
            else if(mode == Mode.TRANSITION_SET_VALUE)
            {
                _animatorComp.SetInteger(_paramName, _paramValue);
            }
            else if (mode == Mode.TRANSITION_INC_VALUE)
            {
                int prevVal = _animatorComp.GetInteger(_paramName);
                _animatorComp.SetInteger(_paramName, prevVal+1);
            }
        }
    }
}

