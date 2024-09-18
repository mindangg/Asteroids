using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D player;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    public Bullet bulletPrefab;
    public ParticleSystem explosion;
    public float thrustPower = 1;
    public float turnSpeed = 0.1f;
    private bool thrust;
    private bool reverse;
    private float turnDirection;
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    private bool isHyper;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        thrust = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        reverse = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            turnDirection = 1;
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            turnDirection = -1;
        else
            turnDirection = 0;

        if(Input.GetMouseButtonDown(0))
            Shoot();

        if(Input.GetKeyDown(KeyCode.H))
            if(!isHyper)
                StartCoroutine("HyperSpace");

        //Screen Wrapping
        Vector2 newPos = transform.position;
        if(transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }
        if(transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        if(transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        if(transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }
        transform.position = newPos;
    }

    private void FixedUpdate()
    {
        if(thrust)
            GetComponent<Rigidbody2D>().AddForce(transform.up * thrustPower);
        if(reverse)
            GetComponent<Rigidbody2D>().AddForce(transform.up * -thrustPower);
        if(turnDirection != 0)
            GetComponent<Rigidbody2D>().AddTorque(turnDirection * turnSpeed);
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.SetTrajectory(transform.up);
    }

    private IEnumerator HyperSpace()
    {
        isHyper = true;
        spriteRenderer.enabled = false;
        explosion.transform.position = transform.position;
        explosion.Play();
        transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-5.4f, 5.4f), 0);

        yield return new WaitForSeconds(0.2f);
        explosion.transform.position = transform.position;
        explosion.Play();
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(3);
        isHyper = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroids")
        {
            player.velocity = Vector3.zero;
            player.angularVelocity = 0;

            gameObject.SetActive(false);
            gameManager.PlayerDied();
        }
    }
}
