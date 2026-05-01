using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float velocidadMax = 8f;
    private bool isGrounded = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(10f, 10f);

    private Rigidbody2D rb;

   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
    }

    public void SetGrounded(bool value)
    {
        isGrounded = value;
    }

    void Update()
    {
        // Solo detecta la parte de arriba del bloque gracias al GroundCheck
        // que está en los pies del jugador
        bool isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        Movimiento();
    }

    void Movimiento()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            // Aplica fuerza en lugar de velocidad directa
            rb.AddForce(new Vector2(horizontal * velocidad, 0));

            // Limita la velocidad máxima
            rb.linearVelocity = new Vector2(
                Mathf.Clamp(rb.linearVelocity.x, -velocidadMax, velocidadMax),
                rb.linearVelocity.y
            );
        }
    }
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
  
    public void Muerte()
    {
        Destroy(gameObject);
     
    }

}