using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

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
            Instantiate(bullet, spawnPoint.transform.position, spawnPoint.rotation);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Shoot");
            PhotonNetwork.Instantiate(bulletPhoton.name, spawnPoint.transform.position, spawnPoint.rotation);
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
