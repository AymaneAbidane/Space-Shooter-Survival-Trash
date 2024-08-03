using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField, Range(0.01f, 0.5f)] public float speed;

    private Material mat;
    private float distance;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        StartCoroutine(COR_ActivateParallax());
    }

    private void OnDisable()
    {
        StopCoroutine(COR_ActivateParallax());
    }

    private IEnumerator COR_ActivateParallax()
    {
        while (true)
        {
            distance += Time.deltaTime * speed;
            mat.SetTextureOffset("_MainTex", Vector2.up * distance);
            yield return null;
        }
    }

}
