using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVEye : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Slider outerEye;
    [SerializeField] private Slider middleEye;
    [SerializeField] private Slider innerEye;
    [Range(0f, 10f)]
    [SerializeField] private float playerMaxX;

    private void Update()
    {
        float playerCurrentX = player.position.x;
        outerEye.value = 0.5f + (playerCurrentX / (2 * playerMaxX));
        middleEye.value = 0.5f + (playerCurrentX / (2 * playerMaxX));
        innerEye.value = 0.5f + (playerCurrentX / (2 * playerMaxX));
    }
}
