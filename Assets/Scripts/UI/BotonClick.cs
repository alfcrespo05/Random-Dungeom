using UnityEngine;
using UnityEngine.UI;

public class BotonClick : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            AudioManager.Instance.PlayClick());
    }
}
