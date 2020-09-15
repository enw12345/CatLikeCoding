using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _ballSpawners;

    [SerializeField]
    private GameObject _ball;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _ballSpawners.Length; i++)
        {
            //Vector3 rotation = new Vector3(0, 90, 0);
            //Instantiate(_ball, _ballSpawners[i].transform.position, Quaternion.Euler(rotation));
            Instantiate(_ball, _ballSpawners[i].transform.position, Quaternion.identity);
        }
    }
}
