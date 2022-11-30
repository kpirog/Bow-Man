using Cinemachine;
using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInputProvider))]
    public class PlayerController : ElympicsMonoBehaviour, IInputHandler, IUpdatable, IInitializable
    {
        [Inject] private PlayerMovementHandler MovementHandler { get; }
        [Inject] private PlayerInputProvider InputProvider { get; }
        
        [Inject.FromChildren] private CinemachineVirtualCamera VirtualCamera { get; }
        
        public void Initialize()
        {
            if (!IsLocalPlayer())
            {
                VirtualCamera.gameObject.SetActive(false);
            }
        }
        
        public void OnInputForClient(IInputWriter inputSerializer)
        {
            if (!IsLocalPlayer())
                return;
            
            inputSerializer.Write(InputProvider.HorizontalAxis);
        }

        #region Useless code

        public void OnInputForBot(IInputWriter inputSerializer)
        {
        }

        #endregion

        public void ElympicsUpdate()
        {
            if (!ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
                return;

            inputReader.Read(out float horizontalAxis);

            HandleMovement(horizontalAxis);
            HandleDrag(horizontalAxis);
        }

        private void HandleMovement(float axis)
        {
            MovementHandler.Move(axis);
        }

        private void HandleDrag(float axis)
        {
            MovementHandler.SetDrag(axis);
        }

        private bool IsLocalPlayer()
        {
            return Elympics.Player == PredictableFor;
        }
    }
}