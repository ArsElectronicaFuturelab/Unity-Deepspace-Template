using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectBhv : MonoBehaviour {

    public UnityPharus.UnityPharusManager.Subject Subject;

    // Use this for initialization
    void Start () {
        transform.position = Subject.position;

    }

    // Update is called once per frame
    void Update () {
        transform.position = Subject.position;
        GetComponentInChildren<TMPro.TextMeshPro>().text = "sub" + Subject.id;
    }
}
