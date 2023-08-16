using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.BuffSystem
{
    public class BuffBehaviour : MonoBehaviour
    {
        [SerializeField] private Buff data;
        private BasicFlier flireInstance;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger entered by: {other.gameObject.name}");
            flireInstance = other.GetComponentInParent<BasicFlier>();
            if (flireInstance)
                data.AddBuff(flireInstance);
            gameObject.SetActive(false);
        }
    }
}
