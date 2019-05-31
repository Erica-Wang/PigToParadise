using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionClamp : MonoBehaviour
{
    [SerializeField]
    private Text DirectionLabel;
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);
        DirectionLabel.transform.position = pos;
    }
}
