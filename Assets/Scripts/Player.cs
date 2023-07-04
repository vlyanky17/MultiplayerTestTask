using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] private PhotonView _view;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Shell _shell;


    public float Speed;
    public int score { get; private set; }

    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }
    public string playerName { get; private set; }
    public float hp { get; private set; }

    private Vector2 _movePosition;
    private FixedJoystick _joyStick;
    private GameManager _gameManager;


    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
        SetJoyStick();
    }

    public void SetName(string name)
    {
        playerName = name;
        Debug.Log(name);
    }

    private void Awake()
    {
        var minVector = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        var maxVector = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        minX = minVector.x + 0.2f;
        minY = minVector.y + 0.2f;
        maxX = maxVector.x - 0.2f;
        maxY = maxVector.y - 0.2f;
        score = 0;
    }

    public void SetHp(float Hp)
    {
        hp = Hp;
    }

    public void Fire()
    {
        object[] myCustomInitData = new object[2];
        myCustomInitData[0] = transform.rotation.y;
        myCustomInitData[1] = playerName;
        PhotonNetwork.Instantiate(_shell.name, transform.position, Quaternion.identity, 0, myCustomInitData);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        SetColor("#" + instantiationData[0].ToString());
    }

    private void SetColor(string hex)
    {
        Color outColor;
        if (ColorUtility.TryParseHtmlString(hex, out outColor))
        {
            _sprite.color = outColor;
        }

    }

    private void SetJoyStick()
    {
        _joyStick = _gameManager.JoyStick;
    }

    private void FixedUpdate()
    {
        if (_view.IsMine)
        {
            _movePosition = new Vector2(_rigidBody.position.x + _joyStick.Horizontal * Speed, _rigidBody.position.y + _joyStick.Vertical * Speed);
            if ((_movePosition.x > minX && _movePosition.x < maxX) && (_movePosition.y > minY && _movePosition.y < maxY))
            {
                _rigidBody.MovePosition(_movePosition);
                if (_joyStick.Horizontal < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (_joyStick.Horizontal > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.GetComponent<Coin>().Hide();
            score++;
            _gameManager.SetCoinCount(score);
        }
        if (collision.CompareTag("Shell"))
        {
            var shellCollision = collision.gameObject.GetComponent<Shell>();
            if (shellCollision.owner != playerName)
            {
                if (_view.IsMine)
                {
                    ReciveDamage();
                }
            }
        }
    }

    public void ReciveDamage()
    {
        hp = hp - _gameManager.Damage;
        _gameManager.UpdateHpBar(hp);
        Debug.Log("reciveDamage "+playerName);
    }
}
