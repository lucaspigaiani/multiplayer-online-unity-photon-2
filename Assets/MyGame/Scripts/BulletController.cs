using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Rigidbody2D rb;

    [SerializeField] private float bulletLifeTime;
    private float bulletTimeCount;

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
}
