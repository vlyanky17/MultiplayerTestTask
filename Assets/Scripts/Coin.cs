using Photon.Pun;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public void Hide()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
