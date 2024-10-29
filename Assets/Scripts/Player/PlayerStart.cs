using Mirror;
using UnityEngine;

public class PlayerStart : NetworkBehaviour
{
    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartClient();
    }
}
