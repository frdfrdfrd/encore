using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleInteractableContainer : InteractableScript
{
    bool _isStarted;
    public bool _canUseMultipleTime = false;
    List<InteractableScript> _multiScript;

    public override void MyStart()
    {
        _isStarted = false;
        _multiScript = new List<InteractableScript>();
        _multiScript.AddRange(gameObject.GetComponents<InteractableScript>());
        if(_multiScript.Contains(this))
        {
            _multiScript.Remove(this);
        }

        foreach(InteractableScript iscript in _multiScript)
        {
            iscript._disableBecauseMultiComposant = true;
            if (this._interactCanvas == null && iscript._interactCanvas != null) _interactCanvas = iscript._interactCanvas;
        }

        foreach (InteractableScript iscript in _multiScript)
        {
            if (iscript._interactCanvas != null) iscript._interactCanvas = null;
        }
    }

    public override void PlayerInteraction()
    {
        if (!_isStarted)
        {
            base.PlayerInteraction();
            if(!_canUseMultipleTime) _isStarted = true;
            MainPlayerScript.instance.RemoveObjectInRange(this); // remove this interaction

            foreach (InteractableScript iscript in _multiScript)
            {
                if (_canUseMultipleTime) iscript.MyStart(); // todo : check for bugs

                iscript.PlayerInteraction();
            }
            if(!_canUseMultipleTime && this.GetComponent<AudioTriggerZone>() )
            {
                AudioTriggerZone trig = this.GetComponent<AudioTriggerZone>();
                if(trig.audioKeyToStart == "player_can_interract")
                {
                    trig._isActivated = false;
                }
            }
        }
    }

}
