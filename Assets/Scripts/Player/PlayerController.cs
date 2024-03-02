using Utils.Singleton;


namespace Player
{
    public class PlayerController : DontDestroyMonoBehaviour
    {
        public static PlayerState PlayerState { get; private set; }

        private PlayerStateObserver _observer;


        protected override void Awake()
        {
            base.Awake();

            _observer = new PlayerStateObserver(SetPlayerState);
            _observer.Subscribe();
        }

        private void OnDestroy()
        {
            _observer.Unsubscribe();
        }

        private void SetPlayerState(PlayerState playerState)
        {
            PlayerState = playerState;
        }
    }
}