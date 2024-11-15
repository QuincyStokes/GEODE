using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClampScreen : MonoBehaviour
{
    void Awake() {

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 0, Screen.width),
            Mathf.Clamp(transform.position.y, 0, Screen.height),
            0);
    }
}
