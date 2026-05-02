using UnityEngine;

public class SierraController : MonoBehaviour
{
    [SerializeField] private float hitCheckRadius = 0.2f;
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;
    private SpriteRenderer sr;
    [SerializeField] private Color colorHielo = new Color(0.5f, 0.8f, 1f);
    [SerializeField] private Color colorNormal = Color.gray;
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;
    private Collider2D col;
    private bool helado = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        CambiarANormal();
    }

    void Update()
    {
        if (helado) return; // si está helada no hace daño

        Collider2D jugador = Physics2D.OverlapCircle(hitCheck.position, hitCheckRadius, playerLayer);
        if (jugador != null)
        {
            jugador.GetComponent<PlayerController>().Muerte();
            GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            manager.vidas -= 2;
            manager.SpawnJugador();
        }
    }

    void OnDrawGizmos()
    {
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitCheck.position, hitCheckRadius);
        }
    }

    public void CambiarAHielo()
    {
        helado = true;
        col.sharedMaterial = materialHielo;
        sr.color = colorHielo;
    }

    public void CambiarANormal()
    {
        helado = false;
        col.sharedMaterial = materialNormal;
        sr.color = colorNormal;
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }
}