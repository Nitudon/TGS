using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonCommons;
using UdonObservable.InputRx.GamePad;
using SystemParameter;

public class AudioManager : UdonBehaviourSingleton<AudioManager>{

    #region[Serialize Setting]
    [System.Serializable] private class BGMDictionary : SerializableDictionary<GameEnum.BGM, AudioClip, BGMPair> { }
    [System.Serializable] private class SEDictionary : SerializableDictionary<GameEnum.SE, AudioClip, SEPair> { }
    [System.Serializable] private class BGMPair : KeyAndValue<GameEnum.BGM, AudioClip> { }
    [System.Serializable] private class SEPair : KeyAndValue<GameEnum.SE, AudioClip> { }
    #endregion

    [SerializeField]
    private BGMDictionary BGMClips;

    [SerializeField]
    private SEDictionary SEClips;

    [SerializeField]
    private AudioSource _BGMSource;

    private Dictionary<GamePadObservable.Player,AudioSource> _playerSources;

    public AudioSource BGMSource
    {
        get
        {
            if(_BGMSource == null)
            {
                InstantLog.StringLogError("BGMSource is null");
                return null;
            }

            return _BGMSource;
        }
    }

    public Dictionary<GamePadObservable.Player,AudioSource> PlayerSources
    {
        get
        {
            if (_playerSources == null)
            {
                InstantLog.StringLogError("BGMSource is null");
                return null;
            }

            return _playerSources;
        }
    }

    public AudioSource GetPlayerSource(GamePadObservable.Player player)
    {
        AudioSource source;
        if (_playerSources == null || _playerSources.TryGetValue(player,out source) == false)
        {
            InstantLog.StringLogError("Invalid access to PlayerSource");
            return null;
        }

        return source;
    }

    public void SetBGMSource(AudioSource source)
    {
        _BGMSource = source;
    }

    public void SetPlayerSource(GamePadObservable.Player player,AudioSource source)
    {
        _playerSources.Add(player,source);
    }

    protected override void Init()
    {
        _playerSources = new Dictionary<GamePadObservable.Player, AudioSource>();
    }

    public void PlayBGM(GameEnum.BGM value)
    {
        AudioClip clip;

        if (_BGMSource == null)
        {
            InstantLog.StringLogError("BGMSource is null");
            return;
        } else if (BGMClips.TryGetValue(value,out clip) == false)
        {
            InstantLog.StringLogError(value.ToString() + " type clip is nothing");
            return;
        }
        else
        {
            _BGMSource.clip = clip;
            _BGMSource.Play();
        }
    }

    public void PlayPlayerSE(GamePadObservable.Player player,GameEnum.SE value)
    {
        AudioSource source;
        AudioClip clip;
        if (_playerSources == null || _playerSources.TryGetValue(player, out source) == false)
        {
            InstantLog.StringLogError("Invalid access to PlayerSource");
            return;
        }
        else if (SEClips.TryGetValue(value, out clip) == false)
        {
            InstantLog.StringLogError(value.ToString() + " type clip is nothing");
            return;
        }
        else
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void SetBGMVolume(float scale)
    {
        BGMSource.volume = scale;
    }

}
