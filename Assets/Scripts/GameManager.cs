using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnPlayers _spawnPlayer;
    [SerializeField] private PhotonView _view;
    [SerializeField] private TextMeshProUGUI _coinsCount;
    [SerializeField] private Button _button;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private GameObject _winnerPopup;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public float minX, maxX, minY, maxY;
    public FixedJoystick JoyStick;
    public float MaxHp;
    public float Damage;

    private Player _player;
    private string[] values;
    private bool _notLastConnectedPlayer;
    private int _playersCount = 1;
    private int _alivePlayerCount = 1;

    private void Awake()
    {
        _spawnPlayer.SpawnPlayer();
        _view.RPC("NewPlayerArrived", RpcTarget.Others);
    }



    private void OnEnable()
    {
        _button.onClick.AddListener(FireClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void FireClick()
    {
        _player.Fire();
    }

    public void UpdateHpBar(float actualHp)
    {
        Debug.Log(actualHp+" "+ MaxHp);
        _hpBar.value = actualHp / MaxHp;
        if (actualHp<=0)
        {
            PhotonNetwork.Destroy(_player.gameObject);
            _view.RPC("PlayerDie", RpcTarget.All);
        }
    }

    public void SetCoinCount(int count)
    {
        _coinsCount.text = count.ToString() + " Coins";
    }

    public void SetCurrentPlayer(Player player)
    {
        _player = player;
        _player.SetName("player1");
        _player.SetHp(MaxHp);
    }

    [PunRPC]
    private void PlayerDie()
    {
        _alivePlayerCount--;
        if (_alivePlayerCount==1)
        {
            if (_player.hp>0)
            {
                _view.RPC("ShowWinner", RpcTarget.All,_player.playerName, _player.score.ToString());
            }
        }
    }
    [PunRPC]
    public void ShowWinner(string name,string score)
    {
        _winnerPopup.SetActive(true);
        _winnerText.text = name + " is winner";
        _scoreText.text = score + " score";
    }

    [PunRPC]
    private void NewPlayerArrived()
    {
        _notLastConnectedPlayer = true;
        _playersCount++;
        _alivePlayerCount++;
        _view.RPC("ReciveData", RpcTarget.Others, _playersCount, _alivePlayerCount);
        Debug.Log("NewPlayerArrived");
    }

    [PunRPC]
    private void ReciveData(int playersCount, int alivePlayerCount)
    {
        if (!_notLastConnectedPlayer)
        {
            _playersCount = playersCount;
            _alivePlayerCount = alivePlayerCount;
            _player.SetName("player"+ _playersCount);
        }
        Debug.Log("ReciveData");
    }
}
