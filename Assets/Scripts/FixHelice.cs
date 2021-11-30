using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHelice : MonoBehaviour
{
    void OnDisable()
    {
        //Debug.Log("script was disabled");
        this.transform.position = new Vector3(500, 500, 500);
    }

  
}
