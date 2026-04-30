using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    [SerializeField] private Color outlineColor = Color.black;
    [SerializeField] private float outlineSize = 0.05f;

    private SpriteRenderer sr;
    private GameObject[] outlineObjects = new GameObject[8];

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CrearOutline();
    }

    void CrearOutline()
    {
        Vector2[] direcciones = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1,1), new Vector2(1,-1), new Vector2(-1,1), new Vector2(-1,-1)
        };

        for (int i = 0; i < 8; i++)
        {
            GameObject outline = new GameObject("Outline_" + i);
            outline.transform.SetParent(transform);
            outline.transform.localPosition = (Vector3)(direcciones[i] * outlineSize);
            outline.transform.localScale = Vector3.one;

            SpriteRenderer outlineSR = outline.AddComponent<SpriteRenderer>();
            outlineSR.sprite = sr.sprite;
            outlineSR.color = outlineColor;
            outlineSR.sortingOrder = sr.sortingOrder - 1;

            outlineObjects[i] = outline;
        }
    }
}