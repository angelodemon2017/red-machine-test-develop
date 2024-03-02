using Player;
using Player.ActionHandlers;
using UnityEngine;

public class CameraSlide : MonoBehaviour
{
    public float sensitivity = 100.0f;

    private Vector2 lastTouchPosition;

    private ClickHandler _clickHandler;

    private void Awake()
    {
        _clickHandler = ClickHandler.Instance;
        _clickHandler.DragStartEvent += OnDragStart;
        _clickHandler.PressedPoint += OnSlide;
    }

    private void OnDragStart(Vector3 startPosition)
    {
        lastTouchPosition = startPosition;
        if (PlayerController.PlayerState == PlayerState.None)
        {

        }
    }

    private void OnSlide(Vector3 newPosition)
    {
        if (PlayerController.PlayerState == PlayerState.Connecting)
            return;

        Vector2 deltaPosition = (Vector2)newPosition - lastTouchPosition; 
        transform.Translate(-deltaPosition.x * sensitivity * Time.deltaTime,
            -deltaPosition.y * sensitivity * Time.deltaTime, 0);
        lastTouchPosition = newPosition;
    }

    private void OnDestroy()
    {
        _clickHandler.DragStartEvent -= OnDragStart;
        _clickHandler.PressedPoint -= OnSlide;
    }
}