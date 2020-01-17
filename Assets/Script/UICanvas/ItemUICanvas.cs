﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUICanvas : MonoBehaviour
{
    private ItemUICanvasModel Model;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { OnClick(); });
    }

    public void SetModel(ItemUICanvasModel model)
    {
        Model = model;

        if (Model.Avatar != null)
            gameObject.transform.GetChild(0).GetComponent<RawImage>().texture = Model.Avatar.texture;

        if (Model.TextDescription != null)
            gameObject.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = Model.TextDescription;
    }

    public void OnClick()
    {
        if (Model.Delegate != null)
        {
            Model.Delegate.OnClick(Model);
        }
    }
}