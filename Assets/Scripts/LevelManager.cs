using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourPunCallbacks
{
    public static LevelManager Instance;
    private void Awake()
    {
        PhotonNetwork.AutosyncScene = true;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    public void Restart()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LeaveRoom(false);

        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}