using UnityEngine;

public interface IVideoChatClient
{
    void Join(string channel);
    void Leave();
    void LoadEngine(string appId);
    void UnloadEngine();
    void EnableVideo(bool enable);
    event System.Action OnJoinedSuccess;
}