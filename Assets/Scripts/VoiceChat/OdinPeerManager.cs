using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using OdinNative.Odin;
using OdinNative.Odin.Room;
using OdinNative.Unity;
using OdinNative.Unity.Audio;
using UnityEngine;

public class OdinPeerManager : MonoBehaviour
{
    private bool muted = false;

    private void AttachOdinPlaybackToPlayer(PlayerObjectController player, Room room, ulong peerId, int mediaId)
    {
        PlaybackComponent playback = OdinHandler.Instance.AddPlaybackComponent(player.gameObject, room.Config.Name, peerId, mediaId);

        // Set the spatialBlend to 1 for full 3D audio. Set it to 0 if you want to have a steady volume independent of 3D position
        playback.PlaybackSource.spatialBlend = 1.0f; // set AudioSource to full 3D
    }

    public PlayerObjectController GetPlayerForOdinPeer(CustomUserDataJsonFormat userData)
    {
        if (userData.seed != null)
        {
            Debug.Log("Player has network Id: " + userData.seed);
            PlayerObjectController[] players = FindObjectsOfType<PlayerObjectController>();
            foreach (var player in players)
            {
                if (player.odinSeed == userData.seed)
                {
                    Debug.Log("Found PlayerScript with seed " + userData.seed);
                    if (player.isLocalPlayer)
                    {
                        Debug.Log("Is local player, no need to do anything");   
                    }
                    else
                    {
                        // We have matched the OdinPeer with our local player instance
                        return player;
                    }
                }   
            }
        }

        return null;
    }

    public void RemoveOdinPlaybackFromPlayer(PlayerObjectController player)
    {
        PlaybackComponent playback = player.GetComponent<PlaybackComponent>();
        Destroy(playback);

        AudioSource audioSource = player.GetComponent<AudioSource>();
        Destroy(audioSource);
    }

    public void OnMediaRemoved(object sender, MediaRemovedEventArgs eventArgs)
    {
        Room room = sender as Room;
        Debug.Log($"ODIN MEDIA REMOVED. Room: {room.Config.Name}, UserData: {eventArgs.Peer.UserData.ToString()}");
        
        CustomUserDataJsonFormat userData = CustomUserDataJsonFormat.FromUserData(eventArgs.Peer.UserData);
        PlayerObjectController player = GetPlayerForOdinPeer(userData);
        if (player)
        {
            RemoveOdinPlaybackFromPlayer(player);
        }
    }

    public void OnMediaAdded(object sender, MediaAddedEventArgs eventArgs)
    {
        Room room = sender as Room;
        Debug.Log($"ODIN MEDIA ADDED. Room: {room.Config.Name}, PeerId: {eventArgs.PeerId}, MediaId: {eventArgs.Media.Id}, UserData: {eventArgs.Peer.UserData.ToString()}");
        
        CustomUserDataJsonFormat userData = CustomUserDataJsonFormat.FromUserData(eventArgs.Peer.UserData);
        PlayerObjectController player = GetPlayerForOdinPeer(userData);
        if (player)
        {
            AttachOdinPlaybackToPlayer(player, room, eventArgs.PeerId, (int)eventArgs.Media.Id);
        }
    }

    public void ToggleMute()
    {
        muted = !muted;
        
        foreach (var room in OdinHandler.Instance.Rooms)
        {
            //OdinHandler.Instance.Microphone.MuteRoomMicrophone(room, muted);
        }
    }
}
