using UnityEngine;

public class Lava : MonoBehaviour
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            Rigidbody2D rbJugador = other.GetComponent<Rigidbody2D>();
            if (!pc.esInvulnerable)
            {
                GameManager.Instance.vidas -= 10;
                pc.StartCoroutine(pc.Invulnerabilidad());
            }
            return;
        }
    }
}
