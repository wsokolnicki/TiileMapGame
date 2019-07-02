using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHolder : MonoBehaviour
{
    [SerializeField] Text textHolderText;

    private void Update()
    {
        Vector2 textPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        textHolderText.transform.position = textPosition;
    }
}
