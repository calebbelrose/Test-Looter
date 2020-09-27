using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            InvenGridManager.Instance.transform.parent.gameObject.SetActive(!InvenGridManager.Instance.transform.parent.gameObject.activeSelf);
    }
}
