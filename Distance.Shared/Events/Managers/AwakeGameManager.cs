namespace Events.Managers
{
    public class AwakeGameManager : StaticEvent<AwakeGameManager.Data>
    {
        public struct Data
        {
            public GameManager manager;

            public Data(GameManager gm)
            {
                manager = gm;
            }
        }
    }
}
