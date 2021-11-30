using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public GameObject _interactCanvas;
    public int _storyConditionID = -1;
    public bool _increaseStoryProgress = false;

    public List<GameObject> _alsoToActivate;
    public List<GameObject> _alsoToDesactivate;

    [HideInInspector] public bool _disableBecauseMultiComposant = false;

    public bool _specialReplaceTriggerByCameraVisibility = false;
    bool _specialVisible;

    public int _changeMetaverseToStep = -1;

    public void Awake()
    {
        _disableBecauseMultiComposant = false;
        _specialVisible = false;
    }

    public void Start()
    {
        if(_interactCanvas) _interactCanvas.SetActive(false);
        MyStart();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_specialReplaceTriggerByCameraVisibility) return;
        if (_disableBecauseMultiComposant) return;
        if (_storyConditionID != -1 && _storyConditionID != GameManager.instance._currentQuestID) return;
        if(collision.transform.parent != null && collision.transform.parent.gameObject.GetComponent< MainPlayerScript>()) MainPlayerScript.instance.AddObjectInRange(this);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (_specialReplaceTriggerByCameraVisibility) return;
        if (_disableBecauseMultiComposant) return;
        if (collision.transform.parent != null && collision.transform.parent.gameObject.GetComponent<MainPlayerScript>()) MainPlayerScript.instance.RemoveObjectInRange(this);
    }

    public virtual void MyStart()
    {
    }

    public virtual void MyUpdate()
    {
        if (!_specialReplaceTriggerByCameraVisibility) return;

        if (_disableBecauseMultiComposant) return;
        if (_storyConditionID != -1 && _storyConditionID != GameManager.instance._currentQuestID) return;

      //  Debug.Log(IsVisibleInScreen());

        if (!_specialVisible && IsVisibleInScreen())
        {
            MainPlayerScript.instance.AddObjectInRange(this);
            _specialVisible = true;
        }
        else if (_specialVisible && !IsVisibleInScreen())
        {
            MainPlayerScript.instance.RemoveObjectInRange(this);
            _specialVisible = false;
        }
    }

    bool IsVisibleInScreen()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return ViewportIsOk(viewportPosition);
    }

    bool ViewportIsOk(Vector3 pos)
    {
        return pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1 && pos.z > 0;
    }

    private void Update()
    {
        MyUpdate();
    }

    public virtual void PlayerInteraction()
    {
        if (_increaseStoryProgress) GameManager.instance.IncreaseCurrentObj();

        foreach(GameObject go in _alsoToActivate)
        {
            go.SetActive(true);
        }

        foreach (GameObject go in _alsoToDesactivate)
        {
            go.SetActive(false);
        }

        if(_changeMetaverseToStep != -1)
        {
            Metaverse.instance.ChangeStep(_changeMetaverseToStep);
        }
    }

}
