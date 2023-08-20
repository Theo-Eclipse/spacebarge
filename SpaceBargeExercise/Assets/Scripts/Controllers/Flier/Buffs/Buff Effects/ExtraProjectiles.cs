using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flier.Modules;

namespace Flier.BuffSystem
{
    public class ExtraProjectiles : BuffEffect<BasicFlier>
    {
        [Space, SerializeField] private int value;
        [SerializeField] private ValueEffectType effectType;
        private List<WeaponModule> weapons = new();
        public override void EnableEffect(BasicFlier flier)
        {
            if(weapons.Count == 0) flier.GetComponentsInChildren(false, weapons);
            foreach(WeaponModule weapon in weapons)
                switch (effectType)
                {
                    case ValueEffectType.Addition:
                        weapon.sFinnal.projectileCount += value;
                        return;
                    case ValueEffectType.Multiplication:
                        weapon.sFinnal.projectileCount *= value;
                        return;
                    case ValueEffectType.Constant:
                        weapon.sFinnal.projectileCount = value;
                        return;
                }
        }
        public override void DisableEffect(BasicFlier flier)
        {
            if (weapons.Count == 0) flier.GetComponentsInChildren(false, weapons);
            foreach (WeaponModule weapon in weapons)
                weapon.sFinnal.projectileCount = weapon.sDefault.projectileCount;
        }
    }
}
