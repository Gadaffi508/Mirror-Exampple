using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVote : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnChangedVote))]
    [SerializeField] private bool isVote = false;

    [SerializeField] private Button isVoteChangedButton = null;

    public void SetVote()
    {
        if (isLocalPlayer)
        {
            if (isServer)
            {
                VoteChanged();
                return;
            }  
            CmdSetVote();
        }
    }

    [Command]
    void CmdSetVote()
    {
        VoteChanged();
        ClientRpcVoteChanged();
    }

    [Server]
    void VoteChanged()
    {
        isVote = !isVote;
    }

    [ClientRpc]
    void ClientRpcVoteChanged()
    {
        isVote = !isVote;
    }

    void OnChangedVote(bool isValue, bool newValue)
    {
        if (newValue == true)
        {
            Debug.Log("Value : " + newValue);
        }
        else
        {
            Debug.Log("Value : " + newValue);
        }
    }
}
