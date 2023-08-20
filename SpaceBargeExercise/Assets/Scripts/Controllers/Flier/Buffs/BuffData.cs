using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.BuffSystem
{
    [System.Serializable, CreateAssetMenu(fileName = "SpeedBuff", menuName = "Scriptable Objects/Flier/Buffs/SpeedBuff", order = 1)]
    public class BuffData : ScriptableObject
    {
        public BuffInfo buffInfo;
        public SpeedEffect buffEffect;
    }
}
