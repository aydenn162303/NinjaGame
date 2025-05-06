using UnityEngine;

public class Flamingarrowscipt : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool touchedGround = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * 5f, ForceMode2D.Impulse); // Adjust the force as needed
        rb.AddForce(transform.right * 8f, ForceMode2D.Impulse); // Adjust the force as needed
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
            rb.linearVelocity = Vector2.zero; // Stop the arrow
            rb.isKinematic = true; // Make the arrow kinematic to stop it from moving
            transform.rotation = Quaternion.identity; // Reset the rotation
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2f); // Destroy the arrow after 2 seconds
        }
    }


}
