using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> _playerPrefabs = new List<GameObject>();
    public Transform spawnPosition;

    private void Start()
    {
        int players = PhotonNetwork.CurrentRoom.PlayerCount;

        var player = PhotonNetwork.Instantiate(_playerPrefabs[players - 1].name, spawnPosition.position, Quaternion.identity);
    }
}