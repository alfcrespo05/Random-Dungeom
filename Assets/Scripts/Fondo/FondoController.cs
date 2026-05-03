using UnityEngine;

public class FondoController : MonoBehaviour
{
    [SerializeField] private float fuerzaViento = 5f;
    [SerializeField] private float intervaloCambio = 10f;
    [SerializeField] private LayerMask objetivosLayer; // Player + Enemigos que se mueven

    private bool aireActivo = false;
    private float direccion = 1f; // 1 = derecha, -1 = izquierda
    private float timer = 0f;

    void Update()
    {
        if (!aireActivo) return;

        // Cambia dirección cada 10 segundos
        timer += Time.deltaTime;
        if (timer >= intervaloCambio)
        {
            timer = 0f;
            direccion *= -1f;
        }

        // Aplica fuerza al jugador
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador != null)
        {
            Rigidbody2D rbJugador = jugador.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
                rbJugador.AddForce(new Vector2(direccion * fuerzaViento, 0));
        }

        // Aplica fuerza a los slimes (solo si se mueven)
        SlimeController[] slimes = FindObjectsByType<SlimeController>(FindObjectsSortMode.None);
        foreach (SlimeController slime in slimes)
        {
            Rigidbody2D rbSlime = slime.GetComponent<Rigidbody2D>();
            if (rbSlime != null)
                rbSlime.AddForce(new Vector2(direccion * fuerzaViento, 0));
        }
    }

    public void CambiarAAire()
    {
        aireActivo = true;
        timer = 0f;
        direccion = 1f;
    }
}