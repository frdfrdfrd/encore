using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchFX : MonoBehaviour
{
    public List<AnimationCurve> _curve;

    bool _glitching;
    float _timer;
    Volume volume;
    private Bloom thisBloom;
    int _currentID;

    private static GlitchFX mInstance;
    public static GlitchFX instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(GlitchFX)) as GlitchFX;
            }

            return mInstance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _glitching = false;
        volume = this.GetComponent<Volume>();

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.G) && !_glitching)
        //{
        //    Glitch(0);
        //}

        //if (Input.GetKeyDown(KeyCode.H) && !_glitching)
        //{
        //    Glitch(1);
        //}


        if (_glitching)
        {
            _timer += Time.deltaTime;

            VolumeProfile profile = volume.sharedProfile;
            volume.profile.TryGet(out thisBloom);
            thisBloom.intensity.value = _curve[_currentID].Evaluate(_timer);

            if (_timer >= _curve[_currentID].keys[_curve[_currentID].length-1].time)
            {
                _glitching = false;
            }

        }
    }

    public void Glitch(int id)
    {
        _glitching = true;
        _timer = 0;
        _currentID = id;
    }
}
