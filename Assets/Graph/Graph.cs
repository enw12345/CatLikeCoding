using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;

    void Awake()
    {
        int i = 0;
        while (i < 10)
        {
            i = i + 1;
            Transform point = Instantiate(pointPrefab);
            point.localPosition = Vector3.right * i;
        }
    }
}
