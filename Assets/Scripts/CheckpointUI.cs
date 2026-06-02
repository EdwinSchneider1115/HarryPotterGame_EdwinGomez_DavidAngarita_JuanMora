using UnityEngine;
using TMPro;
using System.Collections;

public class CheckpointUI : MonoBehaviour
{
    public static CheckpointUI instancia;

    [Header("Panel")]
    public RectTransform panel;
    public TextMeshProUGUI texto;

    [Header("Animacion")]
    public float velocidadSlide = 900f;
    public float duracion = 3f;

    private float posicionOculta;
    private float posicionVisible;
    private Coroutine coroutineActual;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        posicionVisible = panel.anchoredPosition.y;
        posicionOculta = panel.rect.height + 200f;
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, posicionOculta);
    }

    public void Mostrar(string mensaje)
    {
        if (coroutineActual != null) StopCoroutine(coroutineActual);
        coroutineActual = StartCoroutine(MostrarTemporal(mensaje));
    }

    private IEnumerator MostrarTemporal(string mensaje)
    {
        texto.text = mensaje;

        // Entrar desde arriba
        float destino = posicionVisible;
        while (Mathf.Abs(panel.anchoredPosition.y - destino) > 1f)
        {
            float nuevaY = Mathf.MoveTowards(panel.anchoredPosition.y, destino, velocidadSlide * Time.deltaTime);
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, nuevaY);
            yield return null;
        }
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, destino);

        yield return new WaitForSeconds(duracion);

        // Salir hacia arriba
        destino = posicionOculta;
        while (Mathf.Abs(panel.anchoredPosition.y - destino) > 1f)
        {
            float nuevaY = Mathf.MoveTowards(panel.anchoredPosition.y, destino, velocidadSlide * Time.deltaTime);
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, nuevaY);
            yield return null;
        }
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, destino);
    }
}