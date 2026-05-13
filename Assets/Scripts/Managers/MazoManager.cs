using UnityEngine;
using System.Collections.Generic;

public class MazoManager : MonoBehaviour
{
    [Header("Prefabs de cartas")]
    [SerializeField] private GameObject[] prefabsCartas; // todas las cartas menos polisémicas
    [SerializeField] private GameObject[] prefabsPolisemicas; // las dos polisémicas 
    [SerializeField] private Transform posicionMazo; // punto donde aparece la primera carta
    [SerializeField] private float separacionVertical = 1f; // distancia entre cartas

    private List<GameObject> mazo = new List<GameObject>();

    void Start()
    {
        GenerarMazo();
    }

    void GenerarMazo()
    {
        List<GameObject> cartasAleatorias = new List<GameObject>(prefabsCartas);

        // Elige una polisemica aleatoria para el principio
        int indicePolisemica = Random.Range(0, prefabsPolisemicas.Length);
        GameObject polisemica = prefabsPolisemicas[indicePolisemica];



        // Añade la otra polisemica al mazo normal
        for (int i = 0; i < prefabsPolisemicas.Length; i++)
        {
            if (i != indicePolisemica)
                cartasAleatorias.Add(prefabsPolisemicas[i]);
        }

        // Mezcla las cartas normales + la polisemica restante
        for (int i = cartasAleatorias.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = cartasAleatorias[i];
            cartasAleatorias[i] = cartasAleatorias[j];
            cartasAleatorias[j] = temp;
        }

        // Pone la polisemica elegida al principio
        cartasAleatorias.Insert(0, polisemica);

        foreach (GameObject carta in cartasAleatorias)
            Debug.Log("Carta en mazo: " + carta.name);

        // Instancia las cartas
        for (int i = 0; i < cartasAleatorias.Count; i++)
        {
            Vector3 pos = posicionMazo.position + Vector3.up * i * separacionVertical;
            GameObject carta = Instantiate(cartasAleatorias[i], pos, Quaternion.identity);
            mazo.Add(carta);

            SpriteRenderer sr = carta.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = cartasAleatorias.Count - i;

            if (i != 0)
                carta.GetComponent<Collider2D>().enabled = false;

            //  Aquí, marca la primera carta como no tireable a papelera
            if (i == 0)
            {
                Llama lc = carta.GetComponent<Llama>();
                if (lc != null) lc.esPrimera = true;

                Banco bc = carta.GetComponent<Banco>();
                if (bc != null) bc.esPrimera = true;
            }
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
