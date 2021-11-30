using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacterAnim : InteractableScript
{
    public Animation _animComp;
    bool _isStarted;
    public string animClipName;
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
            _animComp.Play(animClipName);
            StartCoroutine(UnfreezeChar());
        }
    }

    IEnumerator UnfreezeChar()
    {
        yield return new WaitForSeconds(_animComp.clip.length);
        MainPlayerScript.instance.FreezeChar(false);
        if (_canUseMultipleTime) _isStarted = false;
    }
}
