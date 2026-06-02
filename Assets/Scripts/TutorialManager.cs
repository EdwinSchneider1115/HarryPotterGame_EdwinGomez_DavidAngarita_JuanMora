using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instancia;

    [Header("Panel UI")]
    public RectTransform panelTutorial;
    public TextMeshProUGUI textoTitulo;
    public TextMeshProUGUI textoContenido;

    [Header("Animacion")]
    public float velocidadSlide = 3000f;

    private float posicionOculta;
    private float posicionVisible;
    private Coroutine coroutineActual;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        posicionOculta = -(Screen.width + panelTutorial.rect.width);
        posicionVisible = panelTutorial.anchoredPosition.x;
        panelTutorial.anchoredPosition = new Vector2(posicionOculta, panelTutorial.anchoredPosition.y);
    }

    public void MostrarMensaje(string titulo, string contenido)
    {
        if (coroutineActual != null) StopCoroutine(coroutineActual);
        coroutineActual = StartCoroutine(AnimarMensaje(titulo, contenido));
    }

    public void MostrarMensajeTemporal(string titulo, string contenido, float duracion = 2.5f)
    {
        if (coroutineActual != null) StopCoroutine(coroutineActual);
        coroutineActual = StartCoroutine(MensajeTemporal(titulo, contenido, duracion));
    }

    public void OcultarMensaje()
    {
        if (coroutineActual != null) StopCoroutine(coroutineActual);
        coroutineActual = StartCoroutine(Salir());
    }

    private IEnumerator MensajeTemporal(string titulo, string contenido, float duracion)
    {
        yield return StartCoroutine(AnimarMensaje(titulo, contenido));
        yield return new WaitForSeconds(duracion);
        yield return StartCoroutine(Salir());
        coroutineActual = null;
    }

    private IEnumerator AnimarMensaje(string titulo, string contenido)
    {
        if (panelTutorial.anchoredPosition.x > posicionOculta + 10f)
        {
            yield return StartCoroutine(Salir());
        }

        if (textoTitulo != null) textoTitulo.text = titulo;
        if (textoContenido != null) textoContenido.text = contenido;

        yield return StartCoroutine(Entrar());
    }

    private IEnumerator Entrar()
    {
        float destino = posicionVisible;

        while (Mathf.Abs(panelTutorial.anchoredPosition.x - destino) > 1f)
        {
            float nuevaX = Mathf.MoveTowards(panelTutorial.anchoredPosition.x, destino, velocidadSlide * Time.deltaTime);
            panelTutorial.anchoredPosition = new Vector2(nuevaX, panelTutorial.anchoredPosition.y);
            yield return null;
        }

        panelTutorial.anchoredPosition = new Vector2(destino, panelTutorial.anchoredPosition.y);

    }

    private IEnumerator Salir()
    {
        float destino = posicionOculta;

        while (Mathf.Abs(panelTutorial.anchoredPosition.x - destino) > 1f)
        {
            float nuevaX = Mathf.MoveTowards(panelTutorial.anchoredPosition.x, destino, velocidadSlide * Time.deltaTime);
            panelTutorial.anchoredPosition = new Vector2(nuevaX, panelTutorial.anchoredPosition.y);
            yield return null;
        }

        panelTutorial.anchoredPosition = new Vector2(destino, panelTutorial.anchoredPosition.y);
    }
}