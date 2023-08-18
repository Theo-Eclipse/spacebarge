using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlaySfx(string soundName) 
        {
        }

        public void PlayMusic(int index) 
        {

        }

        public void CreateLoopSoundInstance(string soundName) 
        {

        }
    }

    public class SoundEffectsPool
    {
        [Header("SFX Settings")]
        [SerializeField, Range(0, 1)] private float sfxVolume = 0.75f;
        [SerializeField] private List<BaseSound> soundClips = new List<BaseSound>();
        [SerializeField] private SFX soundWorldPrefab;
        [Header("Pool Settings")]
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

    public class MusicPlayer 
    {

    }
}
