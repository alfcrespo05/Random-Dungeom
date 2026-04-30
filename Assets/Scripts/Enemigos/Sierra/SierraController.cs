using UnityEngine;

public class SierraController : MonoBehaviour
{
    [SerializeField] private float hitCheckRadius = 0.2f;
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        Collider2D jugador = Physics2D.OverlapCircle(hitCheck.position, hitCheckRadius, playerLayer);

        if (jugador != null)
        {
            jugador.GetComponent<PlayerController>().Muerte();
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
    public void Muerte()
    {
        Destroy(gameObject);
    }
}
