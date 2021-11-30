using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAnimLauncher : InteractableScript
{
    public Animation _animComp;
    bool _isStarted;

    // Start is called before the first frame update
    public override void MyStart()
    {
        _isStarted = false;
    }

    public override void PlayerInteraction()
    {
        if(!_isStarted)
        {
            base.PlayerInteraction();
            _isStarted = true;
            _animComp.Play();
        }
    }
}
