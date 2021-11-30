using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRenderer : MonoBehaviour
{
    public Material _matPlaceHolder;
    public bool _isRecursive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToPlaceHolder()
    {
        // TODO
        if (_matPlaceHolder != null)
        {
            if(!_isRecursive) this.GetComponent<Renderer>().material = _matPlaceHolder;
            else
            {
                Renderer[] rendList = GetComponentsInChildren<Renderer>();
                foreach(Renderer rend in rendList) rend.material = _matPlaceHolder;
            }
        }
    }

    public void SetNoRendererAtAll()
    {
        //TODO
    }
}
