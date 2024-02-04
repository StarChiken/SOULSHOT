using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!Player.Instance.GetIsHolding())
        {
            if (col.gameObject.tag == "Pistol Pick-Up")
            {
                Player.Instance.SetIsHolding(true);

                SetFormAndDestroyCollidedObject(col);
            }
            else if (col.gameObject.tag == "Throwable Pick-Up")
            {
                ThrowablePickUp pickUpScript = col.transform.parent.GetComponent<ThrowablePickUp>();
                Player.Instance.SetIsHoldingThrowable(true, pickUpScript.GetInHandPrefab(), pickUpScript.GetInHandSprite());

                SetFormAndDestroyCollidedObject(col);
            }
        }
    }

    private static void SetFormAndDestroyCollidedObject(Collider2D col)
    {
        Player.Instance.SetPlayerForm(Form.Human);

        Destroy(col.transform.parent.gameObject);
    }
}
