using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class SFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        public System.Action<SFX> onEndReached;
        public bool EndReached => (audioSource.clip.length - audioSource.time) <= 0.1f;
        public void SetClipAndPlay(AudioClip clip, float defaultVolume = 0.75f) 
        {
            audioSource.clip = clip;
            audioSource.volume = defaultVolume;
            audioSource.time = 0;
            audioSource.Play();
        }

        private void Update()
        {
            if (audioSource.isPlaying && EndReached) 
            {
                audioSource.Stop();
                onEndReached?.Invoke(this);
            }
        }
    }
}
