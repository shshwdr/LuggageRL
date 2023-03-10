using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance musicEventInstance;
    LevelTheme currentLevelTheme = LevelTheme.MAIN_MENU;
    
    override protected void Awake()
    {
        base.Awake();


        if (Instance != this)
        {
            //we already destroyed in the Base Class, we just don't want to do anything else. 
            return;
        }

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        //unparent itself from Managers to make DontDestroyOnLoad work properly
        gameObject.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        InitializeMusic();
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            PlayNextMusic();
        }
    }


    private void InitializeMusic()
    {
        if (currentLevelTheme != LevelTheme.MAIN_MENU)
        {
            //something else has already initialized the sound. Nothing to do anymore. 
            return;
        }
        musicEventInstance = CreateInstance(FMODEvents.Instance.music_biome_1);
        FMOD.RESULT result = musicEventInstance.start();
        //Debug.Log("Music Start Result: " + result.ToString());
    }   
    public void PlayNextMusic()
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicEventInstance.release();
        EventReference newReference = FMODEvents.Instance.getNextMusicReference();
        //Debug.Log("Loading new music: " + newReference.ToString());
        musicEventInstance = CreateInstance(newReference);
        FMOD.RESULT result = musicEventInstance.start();
    }
    public enum LevelTheme{
        FOREST = 0,
        CITY = 1,
        LAB = 2,
        MAIN_MENU = 1000
    }

    public void SwitchMusic(LevelTheme levelTheme)
    {
        if (levelTheme == currentLevelTheme)
        {
            return;
        }
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicEventInstance.release();



        EventReference newReference = FMODEvents.Instance.getNextMusicReference();
        
        Debug.Log("Loading new music: " + newReference.ToString());
        musicEventInstance = CreateInstance(newReference);
        currentLevelTheme = levelTheme;
        FMOD.RESULT result = musicEventInstance.start();
    }

    public void SetMusicArea(BiomeArea area)
    {
        musicEventInstance.setParameterByName("area", (float)area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        if (eventInstances != null)
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
        if (eventEmitters != null)
        {
            // stop all of the event emitters, because if we don't they may hang around in other scenes
            foreach (StudioEventEmitter emitter in eventEmitters)
            {
                emitter.Stop();
            }
        }
    }

    override protected void OnDestroy()
    {
        CleanUp();
        //base.OnDestroy();
    }
}
