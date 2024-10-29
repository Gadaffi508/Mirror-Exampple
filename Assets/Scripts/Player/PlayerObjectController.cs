using System;
using Mirror;
using UnityEngine;

public class PlayerObjectController : NetworkBehaviour
{
    public GameObject house;

    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;

    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string PlayerName;

    [SyncVar(hook = nameof(PlayerReadyUpdate))]
    public bool Ready;
    
    [SyncVar]
    public string odinSeed;
    
    public AudioSource microphone;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }

            return manager = NetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayername("Yusuf");
        gameObject.name = "LocalGamePlayer";
    }

    public override void OnStartLocalPlayer()
    {
        CustomUserDataJsonFormat userData = new CustomUserDataJsonFormat(name, "online");
        userData.seed = netId.ToString();

        odinSeed = netId.ToString();
        OdinHandler.Instance.JoinRoom("DuckDastic", userData.ToUserData());
        
        microphone = GetComponent<AudioSource>();
        if (microphone != null)
        {
            microphone.spatialBlend = 0f; // 2D ses için
            microphone.priority = 128;    // Orta öncelik
        }
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        PlayerNameShow();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayerNameShow()
    {
        if (isLocalPlayer && isClient && NetworkClient.ready)
        {
            CmdPlayerNameShow();
        }
    }

    public void CanStartGame(string SceneGame)
    {
        if (isLocalPlayer)
        {
            CmdCanStartGame(SceneGame);
        }
    }

    public void ChangeReady()
    {
        if (isLocalPlayer)
        {
            CMdSetPlayerReady();
        }
    }
    
    [Command]
    public void CmdCanStartGame(string SceneGame)
    {
        manager.StartGame(SceneGame);
    }

    private void PlayerNameUpdate(string OldValue, string newValue)
    {
        if (isServer)
        {
            this.PlayerName = newValue;
        }

        if (isClient)
        {
            //LobbyController.instance.UpdatePlayerList();
        }
    }

    private void PlayerReadyUpdate(bool oldValue, bool newValue)
    {
        if (isServer)
        {
            this.Ready = newValue;
        }

        if (isClient)
        {
            //LobbyController.instance.UpdatePlayerList();
        }
    }
    
    [Command]
    private void CMdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }

    [Command]
    private void CmdPlayerNameShow()
    {
        RpcUpdateUI(this.PlayerName);
    }

    [Command]
    private void CmdSetPlayername(string PlayerName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    [Command]
    public void CmdUpdateClass(string value)
    {
        //RpcUpdateClass(value);
    }

    [ClientRpc]
    private void RpcUpdateUI(string playerName)
    {
        //NameText.text = playerName;
    }
}