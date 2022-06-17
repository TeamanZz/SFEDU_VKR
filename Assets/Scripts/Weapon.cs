using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public HeroController heroController;
    public float fireRate = 0.5f;
    public UnityEvent OnShoot;
    public Transform handTransform;

    private Vector3 originHand;
    private Vector3 targetHand;

    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        originHand = handTransform.localPosition;
        targetHand = originHand;
    }

    float t = 0;

    private void Update()
    {
        targetHand = Vector3.Lerp(targetHand, originHand, Time.deltaTime * 2f);

        handTransform.localPosition = targetHand;

        if (!view.IsMine)
            return;

        if (t > 0)
            t -= Time.deltaTime;
        
        if(Input.GetMouseButton(0) && t <= 0)
        {
            t = fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        targetHand.z -= .05f;

        view.RPC(nameof(Shoot_RPC), RpcTarget.All);

        PhotonNetwork.Instantiate("Bullet", firePoint.position, firePoint.rotation);
    }

    [PunRPC]
    private void Shoot_RPC()
    {
        OnShoot?.Invoke();  
    }
}