using System;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconTemplate : MonoBehaviour
{
    [SerializeField]
    Image iconImage;

    public void SetIconImage(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

}
