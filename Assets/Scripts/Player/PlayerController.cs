using System;
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
        [Inject] private PlayerShootingController ShootingController { get; }
        

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
            inputSerializer.Write(InputProvider.Fire);
            inputSerializer.Write(InputProvider.MousePositionX);
            inputSerializer.Write(InputProvider.MousePositionY);
        }

        #region Useless code

        public void OnInputForBot(IInputWriter inputSerializer)
        {
        }

        #endregion

        public void ElympicsUpdate()
        {
            HandleSlide();
            
            if (!ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
                return;

            inputReader.Read(out float horizontalAxis);
            inputReader.Read(out bool jump);
            inputReader.Read(out bool fire);
            inputReader.Read(out float mousePositionX);
            inputReader.Read(out float mousePositionY);

            HandleMovement(horizontalAxis);
            HandleDrag(horizontalAxis);
            HandleJump(jump);
            HandleShoot(fire, new Vector2(mousePositionX, mousePositionY));
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

        private void HandleShoot(bool fire, Vector2 mousePosition)
        {
            ShootingController.ProcessShoot(fire, mousePosition);
        }

        public bool IsLocalPlayer()
        {
            return Elympics.Player == PredictableFor;
        }
    }
}