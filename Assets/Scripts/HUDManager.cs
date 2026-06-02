using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public PlayerStats playerStats;

    [Header("Barra de vida")]
    public Image barraVida;

    [Header("Contadores PowerUps")]
    public TextMeshProUGUI contadorVelocidad;
    public TextMeshProUGUI contadorInvisibilidad;
    public TextMeshProUGUI contadorVida;

    [Header("Cajitas PowerUps")]
    public Image cajitaVelocidad;
    public Image cajitaInvisibilidad;
    public Image cajitaVida;

    [Header("Colores cajitas")]
    public Color colorActivo = Color.white;
    public Color colorInactivo = new Color(1f, 1f, 1f, 0.3f);

    [Header("Temporizador")]
    public TextMeshProUGUI textoTiempo;
    private float tiempoTranscurrido = 0f;

    void Update()
    {
        if (playerStats == null) return;
        ActualizarVida();
        ActualizarPowerUps();
        ActualizarTiempo();
    }

    void ActualizarVida()
    {
        if (barraVida != null)
        {
            float porcentaje = (float)playerStats.GetVidas() / playerStats.GetVidasMaximas();
            barraVida.fillAmount = porcentaje;

            if (porcentaje > 0.6f) barraVida.color = Color.green;
            else if (porcentaje > 0.3f) barraVida.color = Color.yellow;
            else barraVida.color = Color.red;
        }
    }

    void ActualizarPowerUps()
    {
        ActualizarCajita(cajitaVelocidad, contadorVelocidad, playerStats.GetCantVelocidad());
        ActualizarCajita(cajitaInvisibilidad, contadorInvisibilidad, playerStats.GetCantInvisibilidad());
        ActualizarCajita(cajitaVida, contadorVida, playerStats.GetCantVida());
    }

    void ActualizarCajita(Image cajita, TextMeshProUGUI contador, int cantidad)
    {
        if (contador != null)
            contador.text = cantidad.ToString();

        if (cajita != null)
            cajita.color = cantidad > 0 ? colorActivo : colorInactivo;
    }

    void ActualizarTiempo()
    {
        tiempoTranscurrido += Time.deltaTime;
        int minutos = (int)(tiempoTranscurrido / 60f);
        int segundos = (int)(tiempoTranscurrido % 60f);
        if (textoTiempo != null)
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}