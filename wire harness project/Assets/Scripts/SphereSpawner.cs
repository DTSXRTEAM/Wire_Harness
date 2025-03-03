using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab;
    public DynamicSplineUpdater splineUpdater;
    public int sphereCount = 5;

    void Start()
    {
        for (int i = 0; i < sphereCount; i++)
        {
            Vector3 randomPosition = new Vector3(i * 0.1f, Random.Range(0, 2), Random.Range(-2, 2));
            GameObject newSphere = Instantiate(spherePrefab, randomPosition, Quaternion.identity);
            splineUpdater.AddSphere(newSphere.transform);
        }
    }
}