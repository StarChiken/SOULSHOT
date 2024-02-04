using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private void Start()
    {
        Player.Instance.SetIsGrounded(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Ground" && !Player.Instance.GetIsGrounded())
        {
            Player.Instance.SetIsGrounded(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Ground")
        {
            Player.Instance.SetIsGrounded(false);
        }
    }
}
