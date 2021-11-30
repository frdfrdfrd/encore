using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDisableAllAnim : InteractableScript
{
    bool _isStarted;

    public Collider _alsoSetSpecificColliderToTrigger;
    public GameObject _alsoDisableGameObject;

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
            foreach(ObjectAnimator oa in FindObjectsOfType<ObjectAnimator>())
            {
                oa.SetAnimToFreeze();
            }
            if (_alsoSetSpecificColliderToTrigger) _alsoSetSpecificColliderToTrigger.isTrigger = true;
            if(_alsoDisableGameObject) _alsoDisableGameObject.SetActive(false);
        }
    }

    private void Update()
    {
        
    }

}
