using UnityEngine;

public class Telefono : MonoBehaviour
{
    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckLateralSize = new Vector2(1f, 0.8f);
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float alturaSueloPajaro = 1.5f;
    [SerializeField] private GameObject prefabPajaro;

    private bool llamadaRealizada = false;
    private Collider2D col;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        ComprobarHitLateral();
    }

    void ComprobarHitLateral()
    {
        if (llamadaRealizada) return; // solo una vez

        Collider2D jugadorLateral = Physics2D.OverlapBox(transform.position, hitCheckLateralSize, 0f, playerLayer);
        if (jugadorLateral != null)
        {
            if (jugadorLateral.transform.position.y < transform.position.y + 0.3f)
            {
                PlayerController pc = jugadorLateral.GetComponent<PlayerController>();
                if (pc != null)
                {
                    llamadaRealizada = true;
                    Llamada();
                }
            }
        }
    }

    public void Llamada()
    {
        int llamada = UnityEngine.Random.Range(1, 3); // 1 o 2
        Vector3 spawnPos = transform.position + Vector3.up * alturaSueloPajaro;
        GameObject pajaroObj = Instantiate(prefabPajaro, spawnPos, Quaternion.identity);
        Pajaro pajaro = pajaroObj.GetComponent<Pajaro>();

        switch (llamada)
        {
            case 1:
                pajaro.SetObjetivo(TipoObjetivo.Jugador);
                break;
            case 2:
                pajaro.SetObjetivo(TipoObjetivo.Enemigo);
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