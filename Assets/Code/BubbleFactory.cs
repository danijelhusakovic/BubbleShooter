using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFactory : MonoBehaviour
{
    public static BubbleFactory Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject Create()
    {
        GameObject result = null;

        result = ObjectPooler.instance.SpawnFromPool("Bubble", transform.position, Quaternion.identity);

        return result;
    }
}
