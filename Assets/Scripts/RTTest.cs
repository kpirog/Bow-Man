using Elympics;
using MatchTcpClients.Synchronizer;
using UnityEngine;

public class RTTest : MonoBehaviour, IClientHandler
{
    public static int RTTime { get; private set; }

    public void OnStandaloneClientInit(InitialMatchPlayerData data)
    {
    }

    public void OnClientsOnServerInit(InitialMatchPlayerDatas data)
    {
    }

    public void OnConnected(TimeSynchronizationData data)
    {
    }

    public void OnConnectingFailed()
    {
    }

    public void OnDisconnectedByServer()
    {
    }

    public void OnDisconnectedByClient()
    {
    }

    public void OnSynchronized(TimeSynchronizationData data)
    {
        RTTime = data.RoundTripDelay.Milliseconds;
    }

    public void OnAuthenticated(string userId)
    {
    }

    public void OnAuthenticatedFailed(string errorMessage)
    {
    }

    public void OnMatchJoined(string matchId)
    {
    }

    public void OnMatchJoinedFailed(string errorMessage)
    {
    }

    public void OnMatchEnded(string matchId)
    {
    }
}