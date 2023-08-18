using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        public SoundEffectsPool soundEffects;
        public BackgroundMusic backgroundMusic;
        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            backgroundMusic.Init();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void PlaySfx(string soundName) 
        {
            instance.soundEffects.PlaySfx(soundName);
        }

        public static void PlayMusic(int index) 
        {

        }

        public static void CreateLoopSoundInstance(string soundName) 
        {

        }
    }

    [System.Serializable]
    public class SoundEffectsPool
    {
        [Header("SFX Settings")]
        [SerializeField, Range(0, 1)] private float sfxVolume = 0.75f;
        [SerializeField] private List<BaseSound> soundClips = new List<BaseSound>();
        [SerializeField] private SFX soundWorldPrefab;
        [Space, Header("Pool Settings")]
        [SerializeField] private Transform poolContainer;

        private Queue<SFX> soundInstances = new();
        public AudioClip PlaySfx(string soundName) 
        {
            BaseSound mSound = soundClips.First((s) => s.name.ToLower().Equals(soundName.ToLower()));
            if (mSound != null)
                return mSound.GetAudio();
            Debug.LogError($"Sound Effects Pool <color=red><b>ERROR: No sound effect found, with name </b></color><color=yellow>\"{soundName}\"</color>!");
            return null;
        }

        private SFX CreateInstance() 
        {
            SFX newSfxInstance = Object.Instantiate(soundWorldPrefab.gameObject, poolContainer).GetComponent<SFX>();
            return newSfxInstance;
        }
        private SFX DequeueInstance()
        {
            SFX newSfxInstance = soundInstances.Dequeue();
            return newSfxInstance;
        }
        private void DequeueInstance(SFX sfx) 
        {
            sfx.gameObject.SetActive(false);
            soundInstances.Enqueue(sfx);
        }
    }

    public class LoopSounds
    {

    }

    [System.Serializable]
    public class BackgroundMusic 
    {
        [SerializeField] private List<AudioClip> musicList = new();
        public int currentMusicIndex = 0;
        [SerializeField] private MusicPlayer musicPlayer;
        public void Init() 
        {
            LoadMusicVolumeSettings();
            musicPlayer.onEndReached += PlayNext;
            musicPlayer.PlayNext(musicList[currentMusicIndex]);
        }
        private void PlayNext() 
        {
            currentMusicIndex = (currentMusicIndex + 1) % musicList.Count;
            musicPlayer.PlayNext(musicList[currentMusicIndex]);
        }
        private void SaveMusicVolumeSettings() 
        {
            //PlayerPrefs.SetFloat("MUSIC_VOLUME", musicVolume);
        }
        private void LoadMusicVolumeSettings() 
        {
            //musicVolume = PlayerPrefs.GetFloat("MUSIC_VOLUME", 0.75f);
        }
    }
}
