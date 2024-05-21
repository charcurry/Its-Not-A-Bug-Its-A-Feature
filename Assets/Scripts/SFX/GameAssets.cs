using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    public bool isInitialized = false;

    private void Start()
    {
        if (GameObject.Find("UX_Main") != null)
            uxVariables = GameObject.Find("UX_Main").GetComponent<Shared_UXVariables>();
        if (!isInitialized)
        {
            i.InitializeSoundSettings();
        }
    }

    // This is a singleton class that holds all the game assets.
    private static GameAssets _i;

    // This is a property that returns the singleton instance of the GameAssets class.
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    // This dictionary is used to store the sound settings for each sound.
    public Dictionary<SoundManager.Sound, SoundSettings> soundSettingsDictionary = new Dictionary<SoundManager.Sound, SoundSettings>();

    public UISlider masterVolumeSlider = null;
    // This array is used to store all the sound audio clips.
    public SoundAudioClip[] soundAudioClipArray;

    private Shared_UXVariables uxVariables;

    // This is the class that holds the sound and the audio clip.
    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    // This is the class that holds the sound settings.
    [Serializable]
    public class SoundSettings
    {
        public float maxDistance = 100f;
        public float dopplerLevel = 0f;
        public AudioRolloffMode audioRolloffMode = AudioRolloffMode.Logarithmic;
        public bool isLooped = false;
        public bool isMoving = false;
        public bool destroyAfterFinished = false;
        public UISlider volume;
    }

    // This method is used to initialize the sound settings for each sound.
    // If nothing is stated it uses the default settings.
    public void InitializeSoundSettings()
    {

        masterVolumeSlider = uxVariables.masterVolumeObject.GetComponent<UISlider>();

        soundSettingsDictionary.Add(SoundManager.Sound.Moving_Platform, new SoundSettings
            { maxDistance = 25f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = true, isMoving = false, destroyAfterFinished = false, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Player_Jump, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Jump_Landing, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Player_Move, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Correct_Sound, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Incorrect_Sound, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Puzzle_Solved, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Explosion, new SoundSettings 
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Air_Vent, new SoundSettings 
            { maxDistance = 17.5f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Linear, isLooped = true, isMoving = false, destroyAfterFinished = false, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Door_Open_1, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Door_Open_2, new SoundSettings
            { maxDistance = 200f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Box_Collision, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Key_Collision, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Unlocking_Lock, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Nail_Gun, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Timer_Beep, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Glass_Shattering, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Breaker, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Barrel_Collision, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Nail_Gun_Hit, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        soundSettingsDictionary.Add(SoundManager.Sound.Key_Card_Swipe, new SoundSettings
            { maxDistance = 100f, dopplerLevel = 0, audioRolloffMode = AudioRolloffMode.Logarithmic, isLooped = false, isMoving = false, destroyAfterFinished = true, volume = uxVariables.sfxVolumeObject.GetComponent<UISlider>() });
        isInitialized = true;
    }
}