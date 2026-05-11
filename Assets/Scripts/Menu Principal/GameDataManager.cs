using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public float mejorTiempo = float.MaxValue;

    void Awake()
    {
        // Singleton con DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CargarDatos();
        }
        else
        {
            Destroy(gameObject); // evita duplicados al reiniciar
        }
    }

    void CargarDatos()
    {
        mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo", float.MaxValue);
    }

    public void IntentarGuardarMejorTiempo(float tiempo)
    {
        if (tiempo < mejorTiempo)
        {
            mejorTiempo = tiempo;
            PlayerPrefs.SetFloat("MejorTiempo", mejorTiempo);
            PlayerPrefs.Save();
        }
    }

    public string GetMejorTiempoFormato()
    {
        if (mejorTiempo == float.MaxValue) return "--:--";
        int min = Mathf.FloorToInt(mejorTiempo / 60f);
        int sec = Mathf.FloorToInt(mejorTiempo % 60f);
        return string.Format("{0:00}:{1:00}", min, sec);
    }
}