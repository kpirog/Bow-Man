using Elympics;
using Medicine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInputProvider))]
    public class PlayerController : ElympicsMonoBehaviour, IInputHandler, IUpdatable, IInitializable
    {
        [SerializeField] private GameObject cameraRoot;
        [Inject] private PlayerMovementHandler MovementHandler { get; }
        [Inject] private PlayerInputProvider InputProvider { get; }
        
        public void Initialize()
        {
            if (IsLocalPlayer())
            {
                gameObject.name = "Local Player";
            }

            cameraRoot.SetActive(IsLocalPlayer());
        }
        
        public void OnInputForClient(IInputWriter inputSerializer)
        {
            if (!IsLocalPlayer())
                return;
            
            inputSerializer.Write(InputProvider.HorizontalAxis);
            inputSerializer.Write(InputProvider.Jump);
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
            inputReader.Read(out bool jump);
            
            HandleMovement(horizontalAxis);
            HandleDrag(horizontalAxis);
            HandleJump(jump);
            HandleSlide();
        }

        private void HandleMovement(float axis)
        {
            MovementHandler.Move(axis);
        }

        private void HandleDrag(float axis)
        {
            MovementHandler.SetDrag(axis);
        }

        private void HandleJump(bool input)
        {
            MovementHandler.ProcessJump(input);
        }

        private void HandleSlide()
        {
            MovementHandler.Slide();
        }

        private bool IsLocalPlayer()
        {
            return Elympics.Player == PredictableFor;
        }
    }
}