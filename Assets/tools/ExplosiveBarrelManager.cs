using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

class ExplosiveBarrelManager : MonoBehaviour
{
  public static List<ExplosiveBarrel> allTheBarrels = new List<ExplosiveBarrel>();

#if UNITY_EDITOR
  void OnDrawGizmos()
  {
    Handles.zTest = CompareFunction.LessEqual;

    foreach (ExplosiveBarrel barrel in allTheBarrels)
    {
      Vector3 managerPos = transform.position;
      Vector3 barrelPos = barrel.transform.position;
      float halfweight = (managerPos.y - barrelPos.y) * .5f;
      Vector3 offset = Vector3.up * halfweight;
      Handles.DrawBezier(
        managerPos,
        barrelPos,
        managerPos - offset,
        barrelPos + offset,
        barrel.color,
        EditorGUIUtility.whiteTexture,
        1f
      );
    }
    Handles.color = Color.white;
  }
#endif
}
