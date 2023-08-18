using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class RandomSfx : BaseSound
    {
        [SerializeField] private List<AudioClip> audioClips = new();
        public override AudioClip GetAudio()
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }
    }
}
