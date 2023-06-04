using UnityEngine;

public class LOPortal : MonoBehaviour
{
    public bool isBluePortal;
    [HideInInspector] public bool isTeleportUnabled;
    public static LOPortal blueLOPortal = null, orangeLOPortal = null;
    void OnEnable()
    {
        isTeleportUnabled = false;
        if (isBluePortal)
        {
            if (blueLOPortal)
                Destroy(blueLOPortal.gameObject);
            blueLOPortal = this;
        }
        else
        {
            if (orangeLOPortal)
                Destroy(orangeLOPortal.gameObject);
            orangeLOPortal = this;
        }
    }

    void OnDisable()
    {
        if (isBluePortal)
            blueLOPortal = null;
        else
            orangeLOPortal = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTeleportUnabled)
            return;

        if (isBluePortal)
        {
            if (orangeLOPortal)
            {
                collision.transform.position = orangeLOPortal.transform.position;
                orangeLOPortal.isTeleportUnabled = true;
            }
        }
        else
        {
            if (blueLOPortal)
            {
                collision.transform.position = blueLOPortal.transform.position;
                blueLOPortal.isTeleportUnabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTeleportUnabled = false;
    }
}
