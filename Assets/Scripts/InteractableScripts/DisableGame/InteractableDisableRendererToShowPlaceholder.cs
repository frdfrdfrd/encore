using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDisableRendererToShowPlaceholder : InteractableScript
{
    bool _isStarted;

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
            foreach (ObjectRenderer oa in FindObjectsOfType<ObjectRenderer>())
            {
                oa.SetToPlaceHolder();
            }
        }
    }

    private void Update()
    {

    }

}
