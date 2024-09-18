using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public Sprite[] sprites;
    private GameManager gameManager;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    public float size = 1f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    private float speed = 5;
    private float maxLifeTime = 30;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        transform.eulerAngles = new Vector3(0, 0, Random.value * 360);
        transform.localScale = Vector3.one * size;

        _rigidbody2D.mass = size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody2D.AddForce(direction * speed);

        Destroy(gameObject, maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if(size / 2 >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            gameManager.AsteroidsDestroy(this);
            Destroy(gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle / 2;

        Asteroids half = Instantiate(this, position, transform.rotation);
        half.size = this.size / 2;
        half.SetTrajectory(Random.insideUnitCircle.normalized * speed);    
    }
}
