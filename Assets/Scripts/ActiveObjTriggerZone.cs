using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjTriggerZone : MonoBehaviour

{
    public GameObject _toActivate;
    private void OnTriggerEnter(Collider collision)
    {
        _toActivate.SetActive(true);
    }

    private void OnTriggerExit(Collider collision)
    {
        _toActivate.SetActive(false);
    }
}
