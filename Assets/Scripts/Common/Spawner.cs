using System.Collections.Generic;
using Elympics;
using UnityEngine;

namespace Common
{
    public class Spawner : ElympicsMonoBehaviour, IUpdatable
    {
        [SerializeField] private Vector2[] spawnPositions;
        [SerializeField] private int requiredPlayersCount;

        public static readonly Queue<ElympicsPlayer> PlayersToSpawn = new();

        private bool _canSpawn;

        public void ElympicsUpdate()
        {
            if (!_canSpawn)
            {
                _canSpawn = PlayersToSpawn.Count == requiredPlayersCount;
            }
            else
            {
                if (PlayersToSpawn.Count == 0)
                {
                    _canSpawn = false;
                    return;
                }
                
                SpawnPlayer();
            }
        }

        private void SpawnPlayer()
        {
            var player = ElympicsInstantiate("Prefabs/Player", PlayersToSpawn.Dequeue());
            player.transform.position = spawnPositions[PlayersToSpawn.Count];
        }
    }
}