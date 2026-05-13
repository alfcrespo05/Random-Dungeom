using UnityEngine;

public class BancoEdificio : MonoBehaviour
{
    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckLateralSize = new Vector2(1f, 0.8f);
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PhysicsMaterial2D materialNormal;

    private bool transferenciaRealizada = false;
    private Collider2D col;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ComprobarHitLateral();
    }
    void ComprobarHitLateral()
    {
        if (transferenciaRealizada) return; // solo una vez

        Collider2D jugadorLateral = Physics2D.OverlapBox(transform.position, hitCheckLateralSize, 0f, playerLayer);
        if (jugadorLateral != null)
        {
           
            
                PlayerController pc = jugadorLateral.GetComponent<PlayerController>();
                if (pc != null)
                {
                    transferenciaRealizada = true;
                    Transferencia(pc);
                }
            
        }
    }
    public void Transferencia(PlayerController pc)
    {
        int llamada = UnityEngine.Random.Range(1, 3); // 1 o 2
        switch (llamada)
        {
            case 1:
                GameManager.Instance.vidas -= 2;
                pc.VelocidadBuff();
                break;
            case 2:
                GameManager.Instance.vidas += 2;
                pc.Lentitud();
                break;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, hitCheckLateralSize);
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }
}
