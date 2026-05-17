using UnityEngine;

public class Telefono : MonoBehaviour
{
    [Header("Hit Check")]
    [SerializeField] private Vector2 hitCheckLateralSize = new Vector2(1f, 0.8f);
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float alturaSueloPajaro = 1.5f;
    [SerializeField] private GameObject prefabPajaro;
    [SerializeField] private PhysicsMaterial2D materialNormal;
    private AudioSource audioTelefono;

    private bool llamadaRealizada = false;
    private Collider2D col;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        col = GetComponent<Collider2D>();
        audioTelefono = gameObject.AddComponent<AudioSource>();
        audioTelefono.clip = AudioManager.Instance.Telefono;
        audioTelefono.loop = true;
        audioTelefono.volume = 0.1f;
        if (Time.timeScale == 1) audioTelefono.Play();

    }

    void Update()
    {
        ComprobarHitLateral();
        if (Time.timeScale == 1 && !audioTelefono.isPlaying) audioTelefono.Play();
        if (Time.timeScale == 0 && audioTelefono.isPlaying) audioTelefono.Pause();
    }

    void ComprobarHitLateral()
    {
        if (llamadaRealizada) return; // solo una vez

        Collider2D jugadorLateral = Physics2D.OverlapBox(transform.position, hitCheckLateralSize, 0f, playerLayer);
        if (jugadorLateral != null)
        {
           
            
                PlayerController pc = jugadorLateral.GetComponent<PlayerController>();
                if (pc != null)
                {
                    llamadaRealizada = true;
                    Llamada();
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
                Muerte();
                break;
            case 2:
                pajaro.SetObjetivo(TipoObjetivo.Enemigo);
                Muerte();
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
        AudioManager.Instance.musicaSource.Stop();
        Destroy(gameObject);
    }
}