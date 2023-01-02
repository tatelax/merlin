using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class MonoBehaviourBenchmark : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int numCubes;
    [SerializeField] private float radius;
    [SerializeField] private float lerpSpeed;
    
    private List<Transform> spawnedCubes;
    private float lerpTime;
    
    private void Start()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        
        spawnedCubes = new List<Transform>();
        
        for (int i = 0; i < numCubes; i++)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = Random.insideUnitSphere * radius;
            spawnedCubes.Add(newObject.transform);
        }
        
        watch.Stop();
        Debug.Log($"Instantiate Time: {watch.ElapsedMilliseconds.ToString()}ms");
    }

    private void Update()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        
        for (var i = 0; i < spawnedCubes.Count; i++)
        {
            spawnedCubes[i].position = Vector3.Lerp(spawnedCubes[i].position, Vector3.zero, Time.deltaTime * lerpSpeed);
        }

        watch.Stop();

        lerpTime = watch.ElapsedMilliseconds;
    }

    private void OnGUI()
    {
        GUILayout.Label($"Lerp Time: {lerpTime.ToString()}ms");
    }
}
