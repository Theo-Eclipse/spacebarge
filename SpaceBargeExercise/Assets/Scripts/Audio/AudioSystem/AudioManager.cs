using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    [SerializeField] private AudioMixer musicMixer;

    public SoundEffectsPool soundEffects;

    public LoopSounds loopSounds;

    public BackgroundMusic backgroundMusic;
    private float volume = 0;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadMusicVolumeSettings();
        backgroundMusic.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySfx(string soundName, Vector3 position)
    {
        instance.soundEffects.PlaySfx(soundName, position);
    }

    public static LoopSfx CreateLoopSoundInstance(string soundName, Transform parent)
    {
        return instance.loopSounds.CreateInstance(soundName, parent);
    }
    private void SaveMusicVolumeSettings()
    {
        musicMixer.GetFloat("MusicVolume", out volume);
        PlayerPrefs.SetFloat("MUSIC_VOLUME", volume);
    }
    private void LoadMusicVolumeSettings()
    {
        musicMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MUSIC_VOLUME", 0.75f));
    }
}

[System.Serializable]
public class SoundEffectsPool
{
    [Header("SFX Settings")]
    [SerializeField] private SFX soundPrefab;
    [SerializeField] private List<BaseSound> soundClips = new List<BaseSound>();
    [Space, Header("Pool Settings")]
    [SerializeField] private Transform poolContainer;

    private Queue<SFX> soundsPool = new();
    public BaseSound GetSfxClip(string soundName)
    {
        BaseSound mSound = soundClips.First((s) => s.name.ToLower().Equals(soundName.ToLower()));
        if (mSound != null)
            return mSound;
        Debug.LogError($"Sound Effects Pool <color=red><b>ERROR: No sound effect found, with name </b></color><color=yellow>\"{soundName}\"</color>!");
        return null;
    }
    public void PlaySfx(string name, Vector3 position)
    {
        BaseSound clip = GetSfxClip(name);
        if (!clip.audioClip) return;
        SFX newSfxInstance = soundsPool.Count > 0 ? DequeueInstance() : CreateInstance();
        newSfxInstance.transform.position = position;
        newSfxInstance.SetClipAndPlay(clip);
    }

    private SFX CreateInstance()
    {
        SFX newSfxInstance = Object.Instantiate(soundPrefab.gameObject, poolContainer).GetComponent<SFX>();
        newSfxInstance.onEndReached += DequeueInstance;
        return newSfxInstance;
    }
    private SFX DequeueInstance()
    {
        SFX newSfxInstance = soundsPool.Dequeue();
        newSfxInstance.gameObject.SetActive(true);
        return newSfxInstance;
    }
    private void DequeueInstance(SFX sfx)
    {
        sfx.gameObject.SetActive(false);
        soundsPool.Enqueue(sfx);
    }
}

[System.Serializable]
public class LoopSounds
{
    [Header("SFX Settings")]
    [SerializeField] private LoopSfx soundPrefab;
    [SerializeField] private List<BaseSound> soundClips = new List<BaseSound>();

    public BaseSound GetSfxClip(string soundName)
    {
        BaseSound mSound = soundClips.First((s) => s.name.ToLower().Equals(soundName.ToLower()));
        if (mSound != null)
            return mSound;
        Debug.LogError($"Sound Effects Pool <color=red><b>ERROR: No sound effect found, with name </b></color><color=yellow>\"{soundName}\"</color>!");
        return null;
    }
    public LoopSfx CreateInstance(string souondName, Transform parent)
    {
        BaseSound clip = GetSfxClip(souondName);
        if (!clip.audioClip) return null;
        LoopSfx newSfxInstance = Object.Instantiate(soundPrefab.gameObject, parent).GetComponent<LoopSfx>();
        newSfxInstance.transform.localPosition = Vector3.zero;
        newSfxInstance.SetAudioClipAndVolume(clip);
        newSfxInstance.targetVolume = 0;
        return newSfxInstance;
    }
}

[System.Serializable]
public class BackgroundMusic
{
    [SerializeField] private List<AudioClip> musicList = new();
    public int currentMusicIndex = 0;
    [SerializeField] private MusicPlayer musicPlayer;
    private float volume;
    public void Init()
    {
        musicPlayer.onEndReached += PlayNext;
        musicPlayer.PlayNext(musicList[currentMusicIndex]);
    }
    private void PlayNext()
    {
        currentMusicIndex = (currentMusicIndex + 1) % musicList.Count;
        musicPlayer.PlayNext(musicList[currentMusicIndex]);
    }
}
