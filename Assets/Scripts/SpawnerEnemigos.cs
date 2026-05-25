using UnityEngine;

public class SpawnerEnemigos : MonoBehaviour
{
    public GameObject diabloPrefab;
    public float tiempoEntreSpawn = 3f;
    public float rangoSpawn = 8f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tiempoEntreSpawn)
        {
            timer = 0f;
            Spawnear();
        }
    }

    void Spawnear()
    {
        float x = Random.Range(-rangoSpawn, rangoSpawn);
        float y = Random.Range(-2f, 3f);
        Vector3 pos = new Vector3(x, y, 0);
        Instantiate(diabloPrefab, pos, Quaternion.identity);
    }
}