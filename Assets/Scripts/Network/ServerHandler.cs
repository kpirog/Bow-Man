using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Elympics;
using UnityEngine;

namespace Network
{
    public class ServerHandler : ElympicsMonoBehaviour, IServerHandler
    {
        private static readonly TimeSpan StartGameTimeout = TimeSpan.FromSeconds(30);

        private int _playersNumber;
        private DateTime _waitToStartFinishTime;
        private bool _gameStarted;

        private readonly HashSet<ElympicsPlayer> _playersConnected = new HashSet<ElympicsPlayer>();

        private void Awake()
        {
            Application.runInBackground = true;
        }

        public void OnServerInit(InitialMatchPlayerDatas initialMatchPlayerDatas)
        {
            if (!IsEnabledAndActive)
                return;

            _playersNumber = initialMatchPlayerDatas.Count;
            var humansPlayers = initialMatchPlayerDatas.Count(x => !x.IsBot);
            Debug.Log(
                $"Game initialized with {humansPlayers} human players and {initialMatchPlayerDatas.Count - humansPlayers} bots");

            StartCoroutine(WaitForGameStartOrEnd());
        }

        private IEnumerator WaitForGameStartOrEnd()
        {
            _waitToStartFinishTime = DateTime.Now + StartGameTimeout;

            while (DateTime.Now < _waitToStartFinishTime)
            {
                if (_gameStarted)
                    yield break;

                Debug.Log("Waiting for game to start");
                yield return new WaitForSeconds(5);
            }

            Debug.Log("Forcing game end because game didn't start");
            Elympics.EndGame();
        }

        public void OnPlayerDisconnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
                return;

            Debug.Log($"Player {player} disconnected");
            Debug.Log("Game ended!");
            Elympics.EndGame();
        }

        public void OnPlayerConnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
                return;

            Debug.Log($"Player {player} connected");

            _playersConnected.Add(player);
            Spawner.PlayersToSpawn.Enqueue(player);

            if (_playersConnected.Count != _playersNumber || _gameStarted)
                return;

            _gameStarted = true;
            Debug.Log("Game started!");
        }
        private void Start()
        {
            
        }
    }
}