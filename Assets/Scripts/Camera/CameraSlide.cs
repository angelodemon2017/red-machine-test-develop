using Player;
using Player.ActionHandlers;
using UnityEngine;
using Utils.Singleton;

public class CameraSlide : DontDestroyMonoBehaviourSingleton<CameraSlide>
{
    public float sensitivity = 100.0f;
    public Vector3 defaulthPosition;

    private Vector2 lastTouchPosition;

    private ClickHandler _clickHandler;
    public float _halfWeight;
    public float _halfHeight;

    protected override void Init()
    {
        base.Init();
        defaulthPosition = transform.position;
        _clickHandler = ClickHandler.Instance;
        _clickHandler.DragStartEvent += OnDragStart;
        _clickHandler.PressedPoint += OnSlide;
    }
    
    public void InitBorders(float weight, float height)
    {
        _halfWeight = weight / 2;
        _halfHeight = height / 2;
        transform.position = defaulthPosition;
    }

    private void OnDragStart(Vector3 startPosition)
    {
        lastTouchPosition = startPosition;
    }

    private void OnSlide(Vector3 newPosition)
    {
        if (PlayerController.PlayerState != PlayerState.Scrolling)
            return;

        Vector2 deltaPosition = (Vector2)newPosition - lastTouchPosition;

        var newX = -deltaPosition.x * sensitivity * Time.deltaTime + transform.position.x;
        var newY = -deltaPosition.y * sensitivity * Time.deltaTime + transform.position.y;

        if (newX > _halfWeight)
            newX = _halfWeight;
        if (newX < -_halfWeight)
            newX = -_halfWeight;
        if (newY > _halfHeight)
            newY = _halfHeight;
        if (newY < -_halfHeight)
            newY = -_halfHeight;

        transform.position = new Vector3(newX, newY, transform.position.z);

        lastTouchPosition = newPosition;
    }

    private void OnDestroy()
    {
        _clickHandler.DragStartEvent -= OnDragStart;
        _clickHandler.PressedPoint -= OnSlide;
    }
}