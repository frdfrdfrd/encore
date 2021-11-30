using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager mInstance;
    public static AudioManager instance
    {
        get
        {
            //if (mInstance == null)
            //{
            //    mInstance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            //}

            return mInstance;
        }
    }

    [FMODUnity.BankRef]
    public List<string> _Banks;

    [HideInInspector] public bool _AudioResumed = false;
    [HideInInspector] public bool _HaveAllBanksLoaded = false;

    AudioScript[] _AudioScriptList;

    [HideInInspector] public AudioScript _currentMusicFile;
    [HideInInspector] public AudioScript _previousMusicFile;

    public float _waitFadeBeforeFullStop = 4;

    public bool _playMainMusicOnStart = true;

    public string _keyMainMenu = "main_menu";
    public string _keyIntroGame = "representation";

    private void Awake()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            mInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        _AudioScriptList = FindObjectsOfType<AudioScript>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        foreach (string b in _Banks)
        {
            FMODUnity.RuntimeManager.LoadBank(b, true);
        }
    }

    private void Update()
    {
        bool allBanksLoaderCheck = true;

        if (!_HaveAllBanksLoaded)
        {
            foreach (string b in _Banks)
            {
                if (!FMODUnity.RuntimeManager.HasBankLoaded(b))
                {
                    allBanksLoaderCheck = false;
                    break;
                }
            }
            _HaveAllBanksLoaded = allBanksLoaderCheck;
        }

        if (allBanksLoaderCheck && !_AudioResumed)
        {
            if (Input.anyKeyDown)
            {
                FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
                FMODUnity.RuntimeManager.CoreSystem.mixerResume();
                _AudioResumed = true;
            }
        }
    }
    public void PlaySound(string audioKey,float parameterVal =1)
    {
        AudioScript audio = GetAudioScriptByKey(audioKey);
        if(audio != null) audio.PlayOnce(parameterVal);
    }

    public void StopSound(string audioKey)
    {
        AudioScript audio = GetAudioScriptByKey(audioKey);
        if (audio != null) audio.Stop();
    }

    AudioScript GetAudioScriptByKey(string audioKey)
    {
        foreach (AudioScript audio in _AudioScriptList)
        {
            if (audio._key.ToLower() == audioKey.ToLower())
            {
                return audio;

            }
        }
        return null;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int gameSceneIndex = scene.buildIndex - 1;

        bool playMusic = !PlayerPrefs.HasKey("jam24_disable") || (PlayerPrefs.HasKey("jam24_disable") && PlayerPrefs.GetInt("jam24_disable") == 0);
        if (_playMainMusicOnStart && playMusic) // going to main menu
        {
            if(gameSceneIndex == 0) ChangeMainMusic(_keyMainMenu);
            else if (gameSceneIndex == 1) ChangeMainMusic(_keyIntroGame);
        }
    }

    public void ChangeMainMusic(string newKey)
    {
        AudioScript newAudio = GetAudioScriptByKey(newKey);

        //Debug.Log("ChangeMainMusic to" + newKey + " / " + (newAudio != null) + " / " + (_currentMusicFile != null) + " / prev = " +(_currentMusicFile == null ? "null" : _currentMusicFile._key));

        // do not change the main music if we are already playing it !
        if (newAudio == null || (_currentMusicFile != null && newKey.ToLower() == _currentMusicFile._key.ToLower())) return;

        // change in Coroutine
        StartCoroutine(ChangeMainMusicRoutine(newAudio));
    }

    IEnumerator ChangeMainMusicRoutine(AudioScript newAudio)
    {
        yield return new WaitUntil(() => _HaveAllBanksLoaded == true);
       // Debug.Log("swap " + (_currentMusicFile ==null ? "NULL" :_currentMusicFile._key) + " to "+ newAudio._key);
        _previousMusicFile = _currentMusicFile;
        _currentMusicFile = newAudio;
      //  if(_previousMusicFile != null) _previousMusicFile._Sound.setParameterByName("fade", 0);
      //  _currentMusicFile._Sound.setParameterByName("fade", 1);
        _currentMusicFile.PlayOnce();
        yield return new WaitForSeconds(_waitFadeBeforeFullStop);
       // if (_previousMusicFile != null) Debug.Log("STOP " + _previousMusicFile._key);
        if (_previousMusicFile != null) _previousMusicFile._Sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
