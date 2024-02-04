using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeColorOnForm : MonoBehaviour
{
    public Color humanColor;
    public Color demonColor;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Player.onFormChange += OnFormChange;
    }

    private void OnFormChange(Form form)
    {
        if (form == Form.Human)
        {
            spriteRenderer.color = humanColor;
        }
        else if (form == Form.Demon)
        {
            spriteRenderer.color = demonColor;
        }
    }
}
