using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5f;
    //public float padding = 1f;
    public float spawnDelay = 0.5f;

    private float xmin;
    private float xmax;
    private bool movingRight = true;

    // Use this for initialization
    void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x; //+ padding;
        xmax = rightmost.x; //- padding;
        SpawnUntilFull();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
	
	// Update is called once per frame
	void Update () {
        /*transform.position += Vector3.left * speed * Time.deltaTime;
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);*/
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);
        if (leftEdgeOfFormation < xmin)
        {
            movingRight = true;
        }
        else if (rightEdgeOfFormation > xmax)
        {
            movingRight = false;
        }
        if (AllMembersDead())
        {
            Debug.Log("Empty Formation");
            SpawnUntilFull();
        }
    }
    bool AllMembersDead()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }
    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }


    void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }
    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }
}
