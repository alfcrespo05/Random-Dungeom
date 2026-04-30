using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Vector2 hitCheckSize = new Vector2(1f, 0.2f);
    [SerializeField] private float hitCheckRadius = 0.2f;
    [SerializeField] private Transform hitCheck;
    [SerializeField] private Transform hitCheck2;
    [SerializeField] private LayerMask playerLayer;

    void Update()
    {
        bool playerEncima = Physics2D.OverlapBox(hitCheck.position, hitCheckSize, 0f, playerLayer);

        if (playerEncima)
        {
            Muerte();
        }

        Collider2D jugador = Physics2D.OverlapCircle(hitCheck2.position, hitCheckRadius, playerLayer);

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
            Gizmos.DrawWireCube(hitCheck.position, hitCheckSize);
        }
        if (hitCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hitCheck2.position, hitCheckRadius);
        }
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }
}

