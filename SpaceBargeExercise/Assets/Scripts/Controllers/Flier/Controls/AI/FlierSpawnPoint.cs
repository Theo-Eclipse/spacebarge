using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flier.Controls
{
    public class FlierSpawnPoint : MonoBehaviour, ILevelControl
    {
        [SerializeField] private BasicFlier aiInstance;
        public float respawnTimer = 0;

        private float nextRespawnTime = 0;
        private void Start()
        {
            aiInstance.onFlierDestroyed.AddListener(OnAiDestroyed);
        }

        private void Update()
        {
            if (respawnTimer == 0 || nextRespawnTime == 0)
                return;
            TimerUpdate();
        }
        public void SpawnAi()
        {
            aiInstance.transform.position = transform.position;
            aiInstance.Respawn();
        }
        public void OnLevelLoaded()
        {
            SpawnAi();
        }
        private void OnAiDestroyed() 
        {
            if (respawnTimer == 0)
                return;
            nextRespawnTime = Time.time + respawnTimer;
        }
        private void TimerUpdate() 
        {
            if (Time.time >= nextRespawnTime) 
            {
                SpawnAi();
                nextRespawnTime = 0;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1.5f);
        }


    }
}