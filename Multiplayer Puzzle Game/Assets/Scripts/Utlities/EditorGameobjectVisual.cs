using System;
using UnityEngine;

public class EditorGameobjectVisual : MonoBehaviour
{
    [SerializeField] private Color visualColor;
    [SerializeField] private float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = visualColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
