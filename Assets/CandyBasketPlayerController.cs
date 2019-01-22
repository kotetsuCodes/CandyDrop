using UnityEngine;

public class CandyBasketPlayerController : MonoBehaviour
{
    readonly float maxSpeed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var horizontalAxis = Input.GetAxis("Horizontal");

        // move right

        if (horizontalAxis > 0.0)
        {
            transform.position += Vector3.right * Time.deltaTime * maxSpeed;
        }
        else if (horizontalAxis < 0.0)
        {
            transform.position += Vector3.left * Time.deltaTime * maxSpeed;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CandyDrop")
        {
            var candyDropScript = collision.gameObject.GetComponent<CandyDrop>();
            var points = LevelManager.instance.CandyDropProblem.GetPoints(candyDropScript.CandyDropAnswer);
            LevelManager.instance.AddPoints(points);
            Destroy(collision.gameObject);
        }
    }
}
