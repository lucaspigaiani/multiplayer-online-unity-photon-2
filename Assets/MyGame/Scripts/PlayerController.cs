using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5f;
    private Rigidbody2D body2D;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
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
