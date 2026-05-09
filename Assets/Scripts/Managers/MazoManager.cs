using UnityEngine;
using System.Collections.Generic;

public class MazoManager : MonoBehaviour
{
    [Header("Prefabs de cartas")]
    [SerializeField] private GameObject[] prefabsCartas; // arrastra aquí todos los prefabs
    [SerializeField] private Transform posicionMazo; // punto donde aparece la primera carta
    [SerializeField] private float separacionVertical = 1f; // distancia entre cartas

    private List<GameObject> mazo = new List<GameObject>();

    void Start()
    {
        GenerarMazo();
    }

    void GenerarMazo()
    {
        // Copia y mezcla aleatoriamente
        List<GameObject> cartasAleatorias = new List<GameObject>(prefabsCartas);
        for (int i = cartasAleatorias.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = cartasAleatorias[i];
            cartasAleatorias[i] = cartasAleatorias[j];
            cartasAleatorias[j] = temp;
        }

        // Instancia las cartas en columna
        for (int i = 0; i < cartasAleatorias.Count; i++)
        {
            Vector3 pos = posicionMazo.position + Vector3.up * i * separacionVertical;
            GameObject carta = Instantiate(cartasAleatorias[i], pos, Quaternion.identity);
            mazo.Add(carta);

            SpriteRenderer sr = carta.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = cartasAleatorias.Count - i;

            // Desactiva todas menos la primera
            if (i != 0)
                carta.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void CartaUsada()
    {
        // Elimina la primera carta del mazo
        if (mazo.Count > 0)
        {
            mazo.RemoveAt(0);

            // Activa la siguiente
            if (mazo.Count > 0)
                mazo[0].GetComponent<Collider2D>().enabled = true;
        }
    }
}
