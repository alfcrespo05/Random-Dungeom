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
    public bool esInvulnerable;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private Color colorNormal = Color.white;

    private Animator anim;
    private void Awake()
    {
        esInvulnerable = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        dobleSalto = false;
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Solo resetea cuando acaba de aterrizar
        if (isGrounded && !estabaEnSuelo)
            saltosRestantes = dobleSalto ? 2 : 1;
        estabaEnSuelo = isGrounded;

        anim.SetBool("Saltar", !isGrounded);

        if (Time.timeScale == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                saltosRestantes--;
                anim.SetTrigger("Saltar"); // trigger en vez de bool
                AudioManager.Instance.PlaySound(AudioManager.Instance.Salto);
            }
        }
        else return;
        
        Movimiento();
    }

    void Movimiento()
    {
        if (Time.timeScale == 1)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");

            anim.SetBool("Caminar", horizontal != 0);

            // Cambia la orientación según la dirección
            if (horizontal > 0)
                transform.localScale = new Vector3(0.5f, 0.5f, 1); // mira derecha
            else if (horizontal < 0)
                transform.localScale = new Vector3(-0.5f, 0.5f, 1); // mira izquierda

            if (horizontal != 0)
            {
                rb.AddForce(new Vector2(horizontal * velocidad, 0));
                rb.linearVelocity = new Vector2(
                    Mathf.Clamp(rb.linearVelocity.x, -velocidadMax, velocidadMax),
                    rb.linearVelocity.y
                );
            }
        }
        else return;
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
    public void Lentitud()
    {
        velocidad *= 0.75f;
        velocidadMax *= 0.75f;
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
    public void Subrayar()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1f, 0.84f, 0f, 0.49f);
    }
    public void NoSubrayar()
    {
     
        sr.color = colorNormal;
    }
    public void Muerte()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.muerteSound);
        GameManager.Instance.vidas = 0;
        Destroy(gameObject);
    }

   
}