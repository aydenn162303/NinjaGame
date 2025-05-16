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
        else
        {
            rb.AddForce(transform.up * 4.5f, ForceMode2D.Impulse);
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

        // Check if the arrow is close to the player (center, left, or right)
        if (player != null)
        {
            float xDistance = Mathf.Abs(transform.position.x - player.transform.position.x);

            // Define a safe zone around the player (center, left, right)
            float safeZoneWidth = 1.5f; // Adjust as needed for safety margin

            bool inSafeZone = xDistance <= safeZoneWidth;

            // If within the safe zone, not already touched the ground, and moving downward
            if (inSafeZone && !touchedGround && rb.linearVelocity.y < 0)
            {
                // Apply an upward force in the direction the arrow is currently pointing
                rb.linearVelocity = Vector2.zero; // Reset velocity

                // Add slight left/right velocity based on arrow's facing direction
                float direction = Mathf.Sign(transform.right.x);
                rb.linearVelocity = new Vector2(direction * 2f, 0f);
                rb.AddForce(transform.up * 8f, ForceMode2D.Impulse);
            }
        }
    }


    public void AddForce(bool left, float force)
    {
        rb = GetComponent<Rigidbody2D>();
        if (left)
        {
            rb.AddForce(-transform.right * force, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * force, ForceMode2D.Impulse);
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
