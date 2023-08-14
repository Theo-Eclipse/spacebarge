using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.Modules {
    public class WeaponModule : MonoBehaviour
    {
        [SerializeField] private ModuleInfo info;
        public WeaponStats sDefault = new WeaponStats
        {
            damage = 20,
            shootInterval = 0.2f
        };
        public WeaponStats sFinnal = new WeaponStats
        {
            damage = 20,
            shootInterval = 0.2f
        };
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
