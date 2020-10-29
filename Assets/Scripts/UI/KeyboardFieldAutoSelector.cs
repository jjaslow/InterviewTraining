using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardFieldAutoSelector : MonoBehaviour
{
    [SerializeField]
    InputField myInputField;

    void OnEnable()
    {
        //Selection.activeGameObject = this.gameObject;
        myInputField.Select();
    }


}
