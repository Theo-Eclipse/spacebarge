using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    [System.Serializable]
    public class SoundFx : BaseSound
    {
        [SerializeField] private AudioClip audioClip;
        public override AudioClip GetAudio()
        {
            return audioClip;
        }
    }
}
