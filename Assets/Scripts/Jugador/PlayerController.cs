using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float velocidadMax = 8f;

    private bool dobleSalto = false;
    private int saltosRestantes = 0;
    private bool estabaEnSuelo = false;
    public bool esInvulnerable = false;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dobleSalto = false;
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Solo resetea cuando acaba de aterrizar
        if (isGrounded && !estabaEnSuelo)
            saltosRestantes = dobleSalto ? 2 : 1;

        estabaEnSuelo = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            saltosRestantes--;
        }

        Movimiento();
    }

    void Movimiento()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            rb.AddForce(new Vector2(horizontal * velocidad, 0));
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
    public void VelocidadBuff()
    {
        velocidad *= 1.25f;
        velocidadMax *= 1.25f; // no olvides actualizar también la velocidad máxima
    }
    public void InmovilidadBuff()
    {
        GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        manager.vidas -= 2;
    }
    public void DobleSaltoBuff()
    {
        velocidad *= 0.75f;
        velocidadMax *= 0.75f;
        dobleSalto = true;

    }
    public IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;
        float tiempo = 0f;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        while (tiempo < 1f)
        {
            sr.enabled = !sr.enabled; // parpadeo
            yield return new WaitForSeconds(0.15f);
            tiempo += 0.15f;
        }

        sr.enabled = true;
        esInvulnerable = false;
    }
    public void Muerte()
    {
        Destroy(gameObject);
    }
   
}