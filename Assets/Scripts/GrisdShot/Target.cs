using UnityEngine;

public class Target : MonoBehaviour
{
    public GridManager gridManager;

    public void OnHit()
    {
        gridManager.TargetHit(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnHit();
        }
    }
}

