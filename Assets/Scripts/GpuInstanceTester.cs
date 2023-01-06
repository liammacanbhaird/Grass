using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuInstanceTester : MonoBehaviour
{
  [SerializeField] int gridSize = 50;
  [SerializeField] GameObject go;

  private List<GameObject> go_List = new List<GameObject>();

  [ExecuteInEditMode]  
  private void Awake()
  {
    for (int i = 0; i < gridSize; i++)
    {
      for (int j = 0; j < gridSize; j++)
      {
        GameObject go_temp = Instantiate(go);
        go.transform.position = new Vector3(i, 0, j);
        go_temp.transform.SetParent(transform);
        go_List.Add(go_temp);
      }
    }
  }
  [ExecuteInEditMode]
  private void OnDisable()
  {
    go_List.Clear();
  }
}
