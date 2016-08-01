using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public float health = 150f;
    public GameObject laserPrefab;
    public float laserSpeed;
    public float shotsPerSecond = 0.5f;
    public int scoreValue = 150;
    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;
    private ParticleSystem explosion;
    //private Animator anim;

    void Start()
    {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
        //anim = GetComponent<Animator>();
        explosion = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        float probability = Time.deltaTime * shotsPerSecond;
        if(Random.value < probability)
        {
            Fire();
        }
    }
    void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -laserSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if(health <= 0)
            {
                Die();
            }
        }
    }
    void Die()
    {
        Destroy(gameObject);
        //anim.SetTrigger("ExplosionTrigger");
        explosion.Clear();
        explosion.Play();
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        scoreKeeper.Score(scoreValue);
    }
}
