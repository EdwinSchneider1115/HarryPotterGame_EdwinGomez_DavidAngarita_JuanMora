using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidadMouse = 200f;
    public float fuerzaSalto = 7f;

    private Rigidbody rb;
    private int saltosRestantes = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        transform.Rotate(0f, mouseX, 0f);

        float x = Input.GetAxis("Vertical");
        float z = -Input.GetAxis("Horizontal");

        Vector3 direccion = transform.forward * z + transform.right * x;
        direccion.Normalize();

        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            saltosRestantes--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            saltosRestantes = 2;
        }
    }
}