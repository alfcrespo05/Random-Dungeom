using UnityEngine;

public class SueloController : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D materialNormal;
    [SerializeField] private PhysicsMaterial2D materialHielo;
    [SerializeField] private Color colorNormal = Color.white;
    [SerializeField] private Color colorHielo = new Color(0.5f, 0.8f, 1f);

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);
    [SerializeField] private LayerMask playerLayer;

    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        // Crea el groundCheck automáticamente encima del bloque
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector2(0, 0.55f); // ajusta según el tamaño del bloque
            groundCheck = gc.transform;
        }
    }

    void Update()
    {
        if (groundCheck == null) return; // evita el error si no está asignado

        Collider2D jugador = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, playerLayer);
        if (jugador != null)
        {
            jugador.GetComponent<PlayerController>().SetGrounded(true);
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }

    public void CambiarAHielo()
    {
        col.sharedMaterial = materialHielo;
        sr.color = colorHielo;
    }

    public void CambiarANormal()
    {
        col.sharedMaterial = materialNormal;
        sr.color = colorNormal;
    }
}