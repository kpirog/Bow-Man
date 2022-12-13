using Elympics;
using UnityEngine;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        public void JoinAsServer()
        {
            ElympicsLobbyClient.Instance.StartHalfRemoteServer();
        }

        public void JoinAsClient()
        {
            ElympicsLobbyClient.Instance.PlayHalfRemote(0);
        }

        public void PlayOnline()
        {
            ElympicsLobbyClient.Instance.PlayOnline(null, null, "TestQueue");
        }
    }
}
