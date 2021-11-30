using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacterTP : InteractableScript
{
    public Transform _destination;
    public float _durationTrans;
    bool _isStarted;
    bool _canUseMultipleTime = true;

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
            MainPlayerScript.instance.FreezeChar(true);
            StartCoroutine(TPAndUnfreeze()); 
        }
    }

    IEnumerator TPAndUnfreeze()
    {
        MainPlayerScript.instance.ShowPlayer(false);
        yield return new WaitForSeconds(_durationTrans);
        MainPlayerScript.instance.transform.position = _destination.position;
        MainPlayerScript.instance.FreezeChar(false);
        MainPlayerScript.instance.ShowPlayer(true);
        if(_canUseMultipleTime) _isStarted = false;
    }
}
