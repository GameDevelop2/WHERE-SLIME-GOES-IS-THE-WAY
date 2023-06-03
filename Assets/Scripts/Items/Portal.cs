using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isBluePortal;
    [HideInInspector] public bool isTeleportUnabled;
    public static Portal bluePortal = null, orangePortal = null;


    void OnEnable()
    {
        isTeleportUnabled = false;
        if (isBluePortal)
        {
            if (bluePortal)
                Destroy(bluePortal.gameObject);
            bluePortal = this;
        }
        else
        {
            if (orangePortal)
                Destroy(orangePortal.gameObject);
            orangePortal = this;
        }
    }

    void OnDisable()
    {
        if (isBluePortal)
            bluePortal = null;
        else
            orangePortal = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTeleportUnabled)
            return;

        if (isBluePortal)
        {
            collision.transform.position = orangePortal.transform.position;
            orangePortal.isTeleportUnabled = true;
        }
        else
        {
            collision.transform.position = bluePortal.transform.position;
            bluePortal.isTeleportUnabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTeleportUnabled = false;
    }
}
