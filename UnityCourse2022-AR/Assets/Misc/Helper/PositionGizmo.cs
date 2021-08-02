using UnityEngine;
using UnityEditor;

namespace UnityCourse2022
{
    /// <summary>
    /// ワールド座標をシーンビューに表示
    /// </summary>
    public class PositionGizmo : MonoBehaviour
    {
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            var pos = transform.position;
            Handles.Label(pos + Vector3.up * 1f, gameObject.name + " " + pos.ToString());
        }
#endif
    }
}