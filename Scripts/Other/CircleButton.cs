﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleButton : Button, ICanvasRaycastFilter {

    [SerializeField]
    float buttonRadious = 50f;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
        return Vector2.Distance(sp, transform.position) < buttonRadious;
    }
}