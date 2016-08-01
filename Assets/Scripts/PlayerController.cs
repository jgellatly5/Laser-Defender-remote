using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed = 15.0f;
    public float padding = 1f;
    public GameObject laserPrefab;
    public float laserSpeed;
    public float firingRate = 0.2f;
    public float health = 250f;

    public AudioClip fireSound;

    float xmin;
    float xmax;
	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
    }
	void Fire()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject laser = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, laserSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //move to left
            //transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // move to right
            //transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        // restrict the player to the gamespace
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
           CancelInvoke("Fire");
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    { 
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            Debug.Log("Player hit by missile");
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0)
            {
                Die();
            }
        }
    }
    void Die()
    {
        Destroy(gameObject);
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
    }
}
