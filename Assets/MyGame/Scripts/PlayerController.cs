using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Unity.VisualScripting;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;
    private Rigidbody2D body2D;
    private PhotonView photonView;

    [Header("Health")]
    private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Image healthBar;

    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletPhoton;
    [SerializeField] private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

        HealthManager(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PlayerMove();
        PlayerTurn();
        Shooting();


    }

    private void HealthManager(float value) 
    {
        currentHealth += value;
        healthBar.fillAmount = currentHealth/maxHealth;
    }

    private void Shooting() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Shoot");
            // Instantiate(bullet, spawnPoint.transform.position, spawnPoint.rotation);
            //photonView.RPC(nameof(NetworkShoot), RpcTarget.All);
            PhotonNetwork.Instantiate(bullet.name, spawnPoint.transform.position, spawnPoint.rotation);
        }
        /*if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Shoot");
            PhotonNetwork.Instantiate(bulletPhoton.name, spawnPoint.transform.position, spawnPoint.rotation);
        }*/
    }

    [PunRPC]
    private void NetworkShoot() 
    {
        Instantiate(bullet, spawnPoint.transform.position, spawnPoint.rotation);
    }

    public void TakeDamage(float value, Player player) 
    {
        photonView.RPC(nameof(NetworkTakeDamage), RpcTarget.AllBuffered, value, player);
    }

    [PunRPC]
    private void NetworkTakeDamage(float value, Player player) 
    {
        HealthManager(value);


        player.AddScore(10);

        player.CustomProperties.TryGetValue("score", out object tempScorePlayer);

        int sum = (int)tempScorePlayer;
        sum += 10;

        Hashtable playerTempHash = new()
        {
            { "score", sum }
        };

        player.SetCustomProperties(playerTempHash);


        if (currentHealth <= 0 && photonView.IsMine)
        {
            photonView.RPC(nameof(GameOver), RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void GameOver() 
    {
        if (photonView.Owner.IsMasterClient) 
        {
            Debug.Log("GameOver");
        }

        foreach (var item in PhotonNetwork.PlayerList)
        {
            item.CustomProperties.TryGetValue("score", out object tempScorePlayer);

            Debug.Log("player name: " + item.NickName + " | Score: " +  tempScorePlayer.ToString() + " | Photon score: " + item.GetScore());
        }

    }



    private void PlayerMove() 
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        body2D.velocity = new Vector2 (x, y) * playerSpeed;
    }

    private void PlayerTurn() 
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float x = mousePosition.x - transform.position.x;
        float y = mousePosition.y - transform.position.y;

        Vector2 direction = new Vector2(x, y);

        transform.up = direction;
    }
}
