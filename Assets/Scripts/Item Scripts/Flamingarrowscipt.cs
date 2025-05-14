using UnityEngine;

public class Flamingarrowscipt : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool touchedGround = false;
    private ParticleSystem particleSystem;
    private GameObject player;
    public bool straightdown = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(transform.up * 6.2f, ForceMode2D.Impulse);

        if (!straightdown)
        {
            if (player != null && player.GetComponent<PlayerStateMachine>().playerLastDirectionWasLeft)
            {
                rb.AddForce(-transform.right * 7.5f, ForceMode2D.Impulse);
                transform.position = player.transform.position - new Vector3(1, 0, 0);
            }
            else
            {
                rb.AddForce(transform.right * 7.5f, ForceMode2D.Impulse);
                transform.position = player.transform.position + new Vector3(1, 0, 0);
            }
        }
    }

    void Update()
    {
        // Check if the arrow is moving
        if (rb.linearVelocity.magnitude > 0)
        {
            // Rotate the arrow based on its velocity
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !touchedGround)
        {
            touchedGround = true;
            rb.isKinematic = true; // Make the arrow kinematic to stop it from moving
            rb.bodyType = RigidbodyType2D.Static;
            rb.linearVelocity = Vector2.zero; // Stop the arrow
            
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
            Destroy(GetComponent<Collider2D>());
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            Destroy(gameObject, 0.7f); // Destroy the arrow after 2 seconds
        }
    }


}
