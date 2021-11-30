using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDisableCollider : InteractableScript
{
    bool _isStarted;

    public List<GameObject> _exludeRecursiveList;
    List<Collider> _excludeList;

    public override void MyStart()
    {
        _isStarted = false;
        _excludeList = new List<Collider>();
        foreach(GameObject  go in _exludeRecursiveList)
        {
            _excludeList.AddRange(go.GetComponentsInChildren<Collider>());
        }
    }

    public override void PlayerInteraction()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            base.PlayerInteraction();
            foreach (Collider collide in FindObjectsOfType<Collider>())
            {
                if(!_excludeList.Contains(collide) &&  !collide.isTrigger)
                {
                    collide.enabled = false;
                }
            }
        }
    }

    private void Update()
    {

    }

}
