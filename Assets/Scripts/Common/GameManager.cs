using System;
using Elympics;
using UnityEngine;
using Medicine;


namespace Common
{
    public class GameManager : ElympicsMonoBehaviour, IUpdatable, IInitializable
    {
        [SerializeField] private BaseTimer gameTimer;
        
        public static GameManager Instance;

        [Inject] private CountDown CountDown { get; }

        public GameState CurrentState
        {
            get => (GameState)_internalValue.Value;
            set => _internalValue.Value = (int)value;
        }

        public event Action<GameState> OnGameStateChanged;

        private readonly ElympicsInt _internalValue = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize()
        {
            CurrentState = GameState.WaitingForPlayers;
        }

        private void OnEnable()
        {
            _internalValue.ValueChanged += InternalValueOnValueChanged;
        }

        private void OnDisable()
        {
            _internalValue.ValueChanged -= InternalValueOnValueChanged;
        }

        private void InternalValueOnValueChanged(int lastValue, int newValue)
        {
            OnGameStateChanged?.Invoke(CurrentState);
        }

        public void ElympicsUpdate()
        {
            if (!Elympics.IsServer) return;

            switch (CurrentState)
            {
                default:
                case GameState.WaitingForPlayers:
                {
                    Debug.Log("Waiting for players");
                    break;
                }
                case GameState.Countdown:
                {
                    Debug.Log("CountDown");
                    CountDown.Count();

                    if (CountDown.Finished)
                    {
                        CurrentState = GameState.Playing;
                    }
                    
                    break;
                }
                case GameState.Playing:
                {
                    Debug.Log("Playing");
                    gameTimer.Count();

                    if (gameTimer.Finished)
                    {
                        CurrentState = GameState.Finished;
                    }
                    
                    break;
                }
                case GameState.Finished:
                {
                    Debug.Log("Finished");
                    break;
                }
            }
        }
    }

    public enum GameState
    {
        WaitingForPlayers,
        Countdown,
        Playing,
        Finished
    }
}