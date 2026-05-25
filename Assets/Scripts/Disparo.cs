using UnityEngine;

public class Disparo : MonoBehaviour
{
    public GameObject tokenPrefab;
    public float velocidadToken = 10f;
    public Transform puntoDisparo;

    private Animator anim;
    private float tiempoDisparo = 0.3f;
    private float timerDisparo;

    void Start()
    {
        anim = GetComponent<Animator>();
        timerDisparo = 0f;
    }

    void Update()
    {
        if (timerDisparo > 0)
        {
            timerDisparo -= Time.deltaTime;
            if (timerDisparo <= 0 && anim != null)
                anim.SetBool("disparando", false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
            Disparar(Vector2.right * (transform.localScale.x > 0 ? 1 : -1));

        if (Input.GetKeyDown(KeyCode.X))
            Disparar(Vector2.up);
    }

    void Disparar(Vector2 direccion)
    {
        if (tokenPrefab == null || puntoDisparo == null) return;

        if (anim != null)
            anim.SetBool("disparando", true);

        timerDisparo = tiempoDisparo;

        GameObject token = Instantiate(tokenPrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = token.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direccion * velocidadToken;
        Destroy(token, 3f);
    }
}