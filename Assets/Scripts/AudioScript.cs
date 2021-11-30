using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string _key;
    [FMODUnity.EventRef]
    public string path;

    public bool _onlyAllowOneInstance = false;
    public bool _playOnStart = false;
    public bool _IsEventParameter = true;
    [HideInInspector] public FMOD.Studio.EventInstance _Sound;

    AudioSource _audioSource;
    bool _isPlayedOnce;

    public AudioScript _specialAudioScriptToChangeParam;

    private void Start()
    {
        _isPlayedOnce = false;
        if (_IsEventParameter)
        {

        }
        else
        {
            if (_playOnStart)
            {
                StartCoroutine(PlaySoundCoroutine());
            }
        }

        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayOnce(float parameterValue = 1)
    {
#if UNITY_EDITOR
        Debug.Log("play "+ _key);
#endif
        if (path != null && path != "")
        {
            if (_IsEventParameter)
            {
                if (_specialAudioScriptToChangeParam != null)
                {
                    //FMOD.RESULT res = _specialAudioScriptToChangeParam._Sound.setParameterByName(_key, parameterValue);
                    FMOD.RESULT res = FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Time", parameterValue);
                    //Debug.Log("change param " + _key + " of " + _specialAudioScriptToChangeParam._key + " to " + parameterValue + " -> "+ res.ToString());
                }
                else if (AudioManager.instance._currentMusicFile) AudioManager.instance._currentMusicFile._Sound.setParameterByName(_key, parameterValue);

            }
            else
            {
                if (path == null) return;

                if(!_onlyAllowOneInstance || (_onlyAllowOneInstance && !_isPlayedOnce))  _Sound = FMODUnity.RuntimeManager.CreateInstance(path);


                if(_onlyAllowOneInstance)
                {
                    //FMOD.Studio.EventDescription description ;
                    //_Sound.getDescription(out description);
                    //int countParam;
                    //description.getParameterDescriptionCount(out countParam);
                    //Debug.Log(countParam +" PARAMETERS");
                    //for (int i = 0; i < countParam; i++)
                    //{
                    //    FMOD.Studio.PARAMETER_DESCRIPTION param;
                    //    description.getParameterDescriptionByIndex(i, out param);
                    //    Debug.Log(param.name + " from " + param.minimum + " to " + param.maximum + " index is "+ param.id.data1.ToString() +"/" + param.id.data2.ToString()+ " / type=" + param.type.ToString());
                    //}

                    //FMOD.RESULT res2 = FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Time", Random.Range(0, 100));// _Sound.setParameterByName("Time", Random.Range(0,100));
                    //Debug.Log("changer param before play "+ res2.ToString());
                }

                _Sound.start();
                _isPlayedOnce = true;

                //float val;
                //FMOD.RESULT res = _Sound.getParameterByName("Time",out val);
                //Debug.Log("play with param "+ val + " -> "+ res.ToString());

                if (!_onlyAllowOneInstance) _Sound.release();
            }
        }
        else
        {
           if(_audioSource) _audioSource.Play();
        }
    }

    IEnumerator PlaySoundCoroutine()
    {
       // yield return new WaitUntil(() => IsSceneNotInit() );

        yield return new WaitUntil(() => AudioManager.instance._HaveAllBanksLoaded == true);
        AudioManager.instance._currentMusicFile = this;
        PlayOnce();
    }

    public void Stop()
    {
        //Debug.Log("Stop");
        if (path != null && path != "")
        {
            _Sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            if (_audioSource) _audioSource.Stop();
        }
    }



    //bool IsSceneNotInit()
    //{
    //    return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex > 0;
    //}
}
