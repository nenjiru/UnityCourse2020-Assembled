using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float shotPower = 1f;

    [ContextMenu("test shot")]
    public void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = muzzlePoint.position;

        Vector3 force = muzzlePoint.transform.forward;
        Rigidbody rigid = bullet.GetComponent<Rigidbody>();
        rigid.AddForce(force * shotPower, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        Vector3 pos = muzzlePoint.transform.position;
        Vector3 force = muzzlePoint.transform.forward * shotPower;
        Debug.DrawLine(pos, pos + force, Color.yellow);
    }
}
