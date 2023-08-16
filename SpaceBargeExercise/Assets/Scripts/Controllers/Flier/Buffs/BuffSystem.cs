using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.BuffSystem
{
    //
    // 
    //
    [Serializable]
    public class BuffInfo 
    {
        [Header("Buff Info")]
        public string buffName = "BlankBuff";
        [TextArea] public string description = "Blank Buff - Gives you whole a lot of nothing.";
        public Sprite buffIcon;
    }
    [Serializable]
    public abstract class BuffEffect<T>
    {
        [Header("Effect")]
        public float duration;
        public StackEffect stackEffect = StackEffect.DiscardExisting;
        public abstract void EnableEffect(T flier);
        public abstract void DisableEffect(T flier);
    }

    [Serializable]
    public class Buff 
    {
        public BuffData buffData;
        private BasicFlier flierInstance;
        public float activeTime = 0;
        public void AddBuff(BasicFlier flierInstance)
        {
            this.flierInstance = flierInstance;
            Type buffType = buffData.buffEffectInstance.GetType();
            if (buffData.buffEffectInstance.duration == 0) 
                buffData.buffEffectInstance.EnableEffect(flierInstance);// 0 duration - means permanent - activate the buff without adding it to effects list.
            else if (flierInstance.effects.ContainsKey(buffType))
                StackOverExistingEffect(flierInstance.effects[buffType]);
            else
            {
                flierInstance.effects.Add(buffType, this);
                buffData.buffEffectInstance.EnableEffect(flierInstance);
                activeTime = buffData.buffEffectInstance.duration;
            }

        }
        public void UpdateTimer(float deltaTime) 
        {
            if (activeTime == 0 || buffData.buffEffectInstance.duration == 0)
                return;
            activeTime = activeTime - deltaTime >= 0 ? activeTime - deltaTime : 0;
            if (activeTime == 0)
                RemoveBuff();
        }
        public void RemoveBuff() 
        {
            Type buffType = buffData.buffEffectInstance.GetType();
            buffData.buffEffectInstance.DisableEffect(flierInstance);
            if(!flierInstance.removeEffects.Contains(buffType))
                flierInstance.removeEffects.Add(buffType);
            flierInstance = null;
        }
        private void StackOverExistingEffect(Buff otherBuff)
        {
            switch (buffData.buffEffectInstance.stackEffect)
            {
                case StackEffect.DiscardExisting:
                    return;
                case StackEffect.OverrideExisting:
                    otherBuff.buffData = buffData;
                    otherBuff.activeTime = buffData.buffEffectInstance.duration;
                    buffData.buffEffectInstance.EnableEffect(flierInstance);
                    return;
                case StackEffect.OverrideAndIncreaseDuration:
                    otherBuff.buffData = buffData;
                    otherBuff.activeTime += buffData.buffEffectInstance.duration;
                    buffData.buffEffectInstance.EnableEffect(flierInstance);
                    return;
                case StackEffect.IncreaseDuration:
                    otherBuff.activeTime += buffData.buffEffectInstance.duration;
                    return;
            }
        }
    }
    public enum ValueEffectType 
    {
        Addition,
        Multiplication,
        Constant
    }
    public enum StackEffect
    {
        DiscardExisting,
        OverrideExisting,
        IncreaseDuration,
        OverrideAndIncreaseDuration
    }
}
