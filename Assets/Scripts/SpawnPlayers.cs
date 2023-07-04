using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameManager _gameManager;

    public void SpawnPlayer()
    {
        Vector2 position = new Vector2(Random.RandomRange(_gameManager.minX, _gameManager.maxX), Random.RandomRange(_gameManager.minY, _gameManager.maxY));
        Color color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f));
        object[] myCustomInitData = new object[1];
        myCustomInitData[0] = color.ToHexString();
        var playerObject = PhotonNetwork.Instantiate(_player.name, position, Quaternion.identity, 0, myCustomInitData);
        var currentPlayer = playerObject.GetComponent<Player>();
        currentPlayer.SetGameManager(_gameManager);
        _gameManager.SetCurrentPlayer(currentPlayer);
    }
}
