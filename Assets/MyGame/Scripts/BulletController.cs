using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Rigidbody2D rb;

    [SerializeField] private float bulletLifeTime;
    private float bulletTimeCount;

    [SerializeField] private float bulletDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Force);

    }

    // Update is called once per frame
    void Update()
    {

        if (bulletTimeCount >= bulletLifeTime)
        {
            Destroy(this.gameObject);
        }

        bulletTimeCount += Time.deltaTime;
    }

    [PunRPC]
    private void DestroyBullet() 
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PhotonView photonView = collision.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                Debug.Log("PlayerID: " + photonView.Owner.ActorNumber + " PlayerName: " + photonView.Owner.NickName);
                PlayerController playerController = collision.GetComponent<PlayerController>();
                playerController.TakeDamage(-bulletDamage, photonView.Owner);

                this.GetComponent<PhotonView>().RPC(nameof(DestroyBullet), RpcTarget.AllBufferedViaServer);
            }
        }
    }
}
