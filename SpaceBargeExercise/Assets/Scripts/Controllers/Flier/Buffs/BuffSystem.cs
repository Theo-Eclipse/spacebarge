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
        public StackEffect stackEffect = StackEffect.DiscardAdded;
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
            Type buffType = buffData.buffEffect.GetType();
            if (buffData.buffEffect.duration == 0) 
                buffData.buffEffect.EnableEffect(flierInstance);// 0 duration - means permanent - activate the buff without adding it to effects list.
            else if (flierInstance.effects.ContainsKey(buffType))
                StackOverExistingEffect(flierInstance.effects[buffType]);
            else
            {
                flierInstance.effects.Add(buffType, this);
                buffData.buffEffect.EnableEffect(flierInstance);
                activeTime = buffData.buffEffect.duration;
            }

        }
        public void UpdateTimer(float deltaTime) 
        {
            if (activeTime == 0 || buffData.buffEffect.duration == 0)
                return;
            activeTime = activeTime - deltaTime >= 0 ? activeTime - deltaTime : 0;
            if (activeTime == 0)
                RemoveBuff();
        }
        public void RemoveBuff() 
        {
            Type buffType = buffData.buffEffect.GetType();
            buffData.buffEffect.DisableEffect(flierInstance);
            if(!flierInstance.removeEffects.Contains(buffType))
                flierInstance.removeEffects.Add(buffType);
            flierInstance = null;
        }
        private void StackOverExistingEffect(Buff newBuff)
        {
            switch (buffData.buffEffect.stackEffect)
            {
                case StackEffect.DiscardAdded:
                    return;
                case StackEffect.OverrideExisting:
                    newBuff.buffData = buffData;
                    newBuff.activeTime = buffData.buffEffect.duration;
                    buffData.buffEffect.EnableEffect(flierInstance);
                    return;
                case StackEffect.OverrideAndIncreaseDuration:
                    newBuff.buffData = buffData;
                    newBuff.activeTime += buffData.buffEffect.duration;
                    buffData.buffEffect.EnableEffect(flierInstance);
                    return;
                case StackEffect.IncreaseDuration:
                    newBuff.activeTime += buffData.buffEffect.duration;
                    return;
                case StackEffect.ResetDuration:
                    newBuff.activeTime = buffData.buffEffect.duration;
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
        DiscardAdded,
        ResetDuration,
        OverrideExisting,
        IncreaseDuration,
        OverrideAndIncreaseDuration
    }
}
