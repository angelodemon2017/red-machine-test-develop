using System;
using Connection;
using Events;
using Player.ActionHandlers;
using UnityEngine;

namespace Levels
{
    public class ClickObserver : MonoBehaviour
    {
        [SerializeField] private ColorConnectionManager colorConnectionManager;
        [Header("Field camera slider")]
        [SerializeField] private float Weight = 0f;
        [SerializeField] private float Height = 0f;
        private ClickHandler _clickHandler;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 0, 1, 0.3f);
            Gizmos.DrawCube(Vector3.zero, new Vector3(Weight, Height, 0));
            Gizmos.color = new Color(0, 1, 0, 0.4f);
            Gizmos.DrawCube(Vector3.zero, new Vector3(Weight + 1, Height + 1, 0));
        }

        private void OnValidate()
        {
            if (Weight < 0)
            {
                Weight = 0;
            }
            if (Height < 0)
            {
                Height = 0;
            }
        }

        private void Awake()
        {
            _clickHandler = ClickHandler.Instance;
            
            _clickHandler.PointerDownEvent += OnPointerDown;
            _clickHandler.PointerUpEvent += OnPointerUp;
        }

        private void Start()
        {
            CameraSlide.Instance.InitBorders(Weight, Height);
        }

        private void OnDestroy()
        {
            _clickHandler.PointerDownEvent -= OnPointerDown;
            _clickHandler.PointerUpEvent -= OnPointerUp;
        }
        
        private void OnPointerDown(Vector3 position)
        {
            colorConnectionManager.TryGetColorNodeInPosition(position, out var node);
            
            if (node != null)
                EventsController.Fire(new EventModels.Game.NodeTapped());
            else
                EventsController.Fire(new EventModels.Game.FreeTapped());
        }
        
        private void OnPointerUp(Vector3 position)
        {
            EventsController.Fire(new EventModels.Game.PlayerFingerRemoved());
        }
    }
}