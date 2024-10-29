using Mirror;

public class PlayerLeave : NetworkBehaviour
{
    public void LeaveGame()
    {
        if (isServer)
        {
            CustomNetworkManager.singleton.StopHost();
            return;
        }
        CustomNetworkManager.singleton.StopClient();
    }
}
