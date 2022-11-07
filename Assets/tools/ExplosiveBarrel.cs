using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class ExplosiveBarrel : MonoBehaviour
{
  [Range(1f, 8f)]
  [SerializeField] float radius = 1f;

  [SerializeField] float damage = 10f;
  public Color color = Color.red;


  static readonly int shPropColor = Shader.PropertyToID("_Color");

  MaterialPropertyBlock mpb;
  MaterialPropertyBlock Mpb
  {
    get
    {
      if (mpb == null) mpb = new MaterialPropertyBlock();
      return mpb;
    }
  }

  private void OnDrawGizmosSelected()
  {
    Handles.color = color;
    Handles.DrawWireDisc(transform.position, transform.up, radius);
    Handles.color = Color.white;
  }

  private void OnValidate() => ApplyColor();

  private void OnEnable()
  {
    ApplyColor();
    ExplosiveBarrelManager.allTheBarrels.Add(this);
  }

  private void OnDisable() => ExplosiveBarrelManager.allTheBarrels.Remove(this);

  void ApplyColor()
  {
    MeshRenderer rend = GetComponent<MeshRenderer>();
    Mpb.SetColor(shPropColor, color);
    rend.SetPropertyBlock(Mpb);
  }

}
