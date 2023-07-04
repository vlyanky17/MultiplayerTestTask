using Photon.Pun;
using UnityEngine;

public class Shell : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private PhotonView _view;

    public float Speed;

    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }
    public string owner { get; private set; }

    private bool _onRun;
    private int _sideCoef;


    private void Awake()
    {
        var minVector = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        var maxVector = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        minX = minVector.x + 0.2f;
        minY = minVector.y + 0.2f;
        maxX = maxVector.x - 0.2f;
        maxY = maxVector.y - 0.2f;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        if (instantiationData[0].ToString() == "-1")
        {
            _sideCoef = -1;
        }
        else _sideCoef = 1;
        owner = instantiationData[1].ToString();
        _onRun = true;
    }

    private void FixedUpdate()
    {
        if (_onRun)
        {
            Vector2 _movePosition = new Vector2(transform.position.x + Speed * _sideCoef, transform.position.y);
            if ((_movePosition.x > minX && _movePosition.x < maxX) && (_movePosition.y > minY && _movePosition.y < maxY))
            {
                _rigidBody.MovePosition(_movePosition);
            }
            else PhotonNetwork.Destroy(gameObject);
        }
    }
}

