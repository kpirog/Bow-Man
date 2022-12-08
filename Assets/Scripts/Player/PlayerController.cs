using Elympics;
using Medicine;
using Projectiles;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInputProvider))]
    public class PlayerController : ElympicsMonoBehaviour, IInputHandler, IUpdatable, IInitializable
    {
        [SerializeField] private GameObject cameraRoot;
        [Inject] private PlayerMovementHandler MovementHandler { get; }
        [Inject] private PlayerShootingController ShootingController { get; }
        [Inject] public PlayerInputProvider InputProvider { get; }
        [Inject] public PlayerCollisionHandler CollisionHandler { get; }

        public ElympicsPlayer Local => PredictableFor;
        
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
            inputSerializer.Write(InputProvider.StandardArrow);
            inputSerializer.Write(InputProvider.IceArrow);
            inputSerializer.Write(InputProvider.InvertedArrow);
        }

        #region Useless code

        public void OnInputForBot(IInputWriter inputSerializer)
        {
        }

        #endregion

        public void ElympicsUpdate()
        {
            MovementHandler.LimitSpeed();
            HandleSlide();
            
            if (!ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
                return;

            inputReader.Read(out float horizontalAxis);
            inputReader.Read(out bool jump);
            inputReader.Read(out bool fire);
            inputReader.Read(out float mousePositionX);
            inputReader.Read(out float mousePositionY);
            inputReader.Read(out bool standardArrow);
            inputReader.Read(out bool iceArrow);
            inputReader.Read(out bool invertedArrow);

            HandleMovement(horizontalAxis);
            HandleDrag(horizontalAxis);
            HandleJump(jump);
            HandleShoot(fire, new Vector2(mousePositionX, mousePositionY));
            HandleSwitchArrow(standardArrow, iceArrow, invertedArrow);
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

        private void HandleSwitchArrow(bool standardArrow, bool iceArrow, bool invertedArrow)
        {
            if (standardArrow)
            {
                ShootingController.SetCurrentArrow(ArrowType.Push);
            }
            else if (iceArrow)
            {
                ShootingController.SetCurrentArrow(ArrowType.Ice);
            }
            else if (invertedArrow)
            {
                ShootingController.SetCurrentArrow(ArrowType.Inverted);
            }
        }

        private bool IsLocalPlayer()
        {
            return Elympics.Player == PredictableFor;
        }
    }
}