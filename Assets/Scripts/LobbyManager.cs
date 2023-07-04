using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _createLobbyInput;
    [SerializeField] private TMP_InputField _connectLobbyInput;
    [SerializeField] private Button _createLobbyButton;
    [SerializeField] private Button _connectLobbyButton;

    private void Awake()
    {
        _createLobbyButton.onClick.AddListener(CreateLobby);
        _connectLobbyButton.onClick.AddListener(ConnectToLobby);
    }

    private void CreateLobby()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(_createLobbyInput.text, roomOptions);
    }

    public void ConnectToLobby()
    {
        PhotonNetwork.JoinRoom(_connectLobbyInput.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("fail " + returnCode + " " + message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
        _createLobbyButton.onClick.RemoveAllListeners();
        _connectLobbyButton.onClick.RemoveAllListeners();
    }
}
