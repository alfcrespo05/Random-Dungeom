using UnityEngine;

public class LlamaAnimal : MonoBehaviour
{
    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckSize = new Vector2(1f, 0.2f);
    [SerializeField] private Transform hitCheck;
    [SerializeField] private LayerMask playerLayer;

    [Header("Modo Torreta")]
    [SerializeField] private GameObject prefabProyectilLlama;
    [SerializeField] private float intervaloDisparo = 2f;
    private float timerDisparo = 0f;

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
        // Hit check arriba (mata a la llama)
        if (hitCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitCheck.position, hitCheckSize);
        }

        //// Hit check lateral (mata al jugador)
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawWireCube(transform.position, hitCheckLateralSize);

        
    }

    // Update is called once per frame
    void Update()
    {
        //ComprobarHitLateral();
        ModoTorreta();
    }
    void ComprobarHit()
    {
        if (hitCheck == null) return;
        Collider2D jugadorArriba = Physics2D.OverlapBox(hitCheck.position, hitCheckSize, 0f, playerLayer);
        if (jugadorArriba != null)
        {
            // El jugador pisó al slime muere el slime
            Muerte();
        }
    }
    //void ComprobarHitLateral()
    //{
       
    //    Collider2D jugadorLateral = Physics2D.OverlapBox(transform.position, tamañoActual, 0f, playerLayer);
    //    if (jugadorLateral != null)
    //    {
    //        if ( jugadorLateral.transform.position.y < transform.position.y + 0.3f)
    //        {
    //            PlayerController pc = jugadorLateral.GetComponent<PlayerController>();
    //            if (pc != null && !pc.esInvulnerable) // comprueba primero
    //            {
    //                GameManager.Instance.vidas -= 1; // luego resta vidas
    //                pc.StartCoroutine(pc.Invulnerabilidad());
    //            }
    //        }
    //    }
    //}
    void ModoTorreta()
    {
        // Sigue comprobando si el jugador lo pisa
        ComprobarHit();

        timerDisparo += Time.deltaTime;
        if (timerDisparo >= intervaloDisparo)
        {
            timerDisparo = 0f;
            Disparar();
        }
    }
    void Disparar()
    {
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador == null || prefabProyectilLlama == null) return;

        Vector2 dir = (jugador.transform.position - transform.position).normalized;
        AudioManager.Instance.PlaySound(AudioManager.Instance.enemigoDisparo);
        GameObject proyectil = Instantiate(prefabProyectilLlama, transform.position, Quaternion.identity);
        proyectil.GetComponent<ProyectilLlama>().SetDireccion(dir);
    }
    public void Muerte()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.muerteEnemigo);
        Destroy(gameObject);
    }
}
