using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public void StartGame()
    {
        NetworkManager.singleton.StartHost();
    }
}
