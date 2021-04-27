using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    public bool CanAttack { get; protected set; }

    public abstract void Attack();

    protected void Update() {
        LookAtMouse();
    }

    protected virtual void LookAtMouse() {
        if (Player.Instance == null || Player.Instance.Died) {
            return;
        }

        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
