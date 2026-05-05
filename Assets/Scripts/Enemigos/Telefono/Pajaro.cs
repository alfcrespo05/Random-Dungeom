using UnityEngine;

public class Pajaro : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CaerJugador();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<LlamaAnimal>() != null) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            // Empuje siempre, daño solo si no es invulnerable
            Rigidbody2D rbJugador = other.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
              

            if (!pc.esInvulnerable)
            {
                GameManager.Instance.vidas -= 2;
                pc.StartCoroutine(pc.Invulnerabilidad());
            }

            Destroy(gameObject);
            return;
        }
        SlimeController slc = other.GetComponent<SlimeController>();
        if (slc != null)
        {
            // Empuje siempre, daño solo si no es invulnerable
            Rigidbody2D rbslime = other.GetComponent<Rigidbody2D>();
            if (rbslime != null) slc.Muerte();

            Destroy(gameObject);
            return;
        }
        

    }
   
    void CaerJugador()
    {
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador == null) return;

        Vector2 dir = jugador.transform.position;
   
    }
    void CaerEnemigo()
    {
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador == null) return;

        Vector2 dir = jugador.transform.position;

    }
    public void Muerte()
    {
        Destroy(gameObject);
    }
}
