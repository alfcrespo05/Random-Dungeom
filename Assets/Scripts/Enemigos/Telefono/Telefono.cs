using UnityEngine;

public class Telefono : MonoBehaviour
{
    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckSize = new Vector2(1f, 0.2f);
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 hitCheckLateralSize = new Vector2(1f, 0.8f);
    public Vector3 posicionOriginal;
    [SerializeField] private float alturaSueloPajaro = 1.5f;
    [SerializeField] private GameObject prefabPajaro;
    


    private Collider2D col;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // no puede ser empujado
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void OnDrawGizmos()
    {
        // Hit check arriba (mata al telefono)
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitCheck.position, hitCheckSize);
        }

        // Hit check lateral (mata al jugador)
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, hitCheckLateralSize);


    }

    // Update is called once per frame
    void Update()
    {
        ComprobarHitLateral();
    
    }
    void ComprobarHitLateral()
    {
        Vector2 tamañoActual = hitCheckLateralSize;
        Collider2D jugadorLateral = Physics2D.OverlapBox(transform.position, tamañoActual, 0f, playerLayer);
        if (jugadorLateral != null)
        {
            if (jugadorLateral.transform.position.y < transform.position.y + 0.3f)
            {
                PlayerController pc = jugadorLateral.GetComponent<PlayerController>();
                if (pc != null && !pc.esInvulnerable) // comprueba primero
                {

                    Llamada();
                }
            }
        }
    }
   public void Llamada()
    {
        int llamada = UnityEngine.Random.Range(1, 2);
        switch (llamada)
        {
            case 1:
                posicionOriginal += Vector3.up * alturaSueloPajaro;
                GameObject Pajaro = Instantiate(prefabPajaro, posicionOriginal, Quaternion.identity);
                break;
            case 2:

                break;
        }
    }
   
    public void Muerte()
    {
        Destroy(gameObject);
    }
}
