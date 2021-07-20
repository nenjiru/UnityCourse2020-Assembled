using UnityEngine;

public class UseWeapon : MonoBehaviour
{
    public Camera playerCamera;
    public Transform handPoint;
    public float distance = 100f;
    private Weapon weapon;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && weapon != null)
        {
            weapon.Shot();
        }

        if (Input.GetButtonDown("Cancel") && weapon != null)
        {
            weapon.transform.SetParent(null);
            weapon = null;
        }

        if (Input.GetButtonDown("Fire1") && weapon == null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = playerCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                hitObject(hit.collider);
            }
        }
    }

    private void hitObject(Collider hit)
    {
        if (hit.CompareTag("Weapon"))
        {
            hit.transform.SetParent(handPoint);
            hit.transform.localPosition = Vector3.zero;
            hit.transform.localRotation = Quaternion.identity;
            weapon = hit.GetComponent<Weapon>();
        }
    }
}
