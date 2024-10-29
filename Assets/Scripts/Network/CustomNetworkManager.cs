using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController GamePlayerPrefab;
    
    public List<PlayerObjectController> GamePlayers =
        new List<PlayerObjectController>();

    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        
        Debug.Log("Hey <client>, you've been disconnected!");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            PlayerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);

            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
            /*GamePlayerInstance.PlayerSteamID =
                (ulong)SteamMatchmaking.GetLobbyMemberByIndex(
                    (CSteamID)SteamLobby.instance.CurrentLobbyID, GamePlayers.Count);*/

            // GamePlayerInstance.GetComponent<PlayerClass>().className = ClassGenerator.Instance.Currentclasses;

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);

            //ClassGenerator.Instance.GetCurrentList();

            //StartCoroutine(JoinMessage(GamePlayerInstance));
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
        
        if (newSceneName.StartsWith("Game"))
        {
            //AssignClassesToPlayers();
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        if (conn.identity != null)
        {
            // Perform any necessary cleanup here
            // For example, you can disconnect Steam if necessary
            // SteamAPI.Shutdown(); // Do not call this directly here
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        GamePlayers.Clear();
    }

    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinButton()
    {
        
    }

    public void JoinLobby()
    {
        NetworkManager.singleton.StartClient();
    }

    public void StartGame(string SceneGame)
    {
        ServerChangeScene("Game");
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
            ServerChangeScene("Lobby");
        }
        else
        {
            NetworkManager.singleton.StopClient();
            ServerChangeScene("Lobby");
        }
    }
}
