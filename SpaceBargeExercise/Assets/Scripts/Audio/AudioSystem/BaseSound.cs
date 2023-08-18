using UnityEngine;

namespace AudioSystem
{
    public abstract class BaseSound
    {
        public string name = "My Sound";
        public abstract AudioClip GetAudio();
    }
}
