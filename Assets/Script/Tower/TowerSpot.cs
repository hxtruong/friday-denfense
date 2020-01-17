﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;

public class TowerSpot : MyObject, ItemUICanvasDelegate
{
    public Tower CurrentTower;
    public ShowItemsUICanvas ShowItemsUICanvas;

    public List<GameObject> AvailableTowers = new List<GameObject>();

    protected TowerFactory Factory = new TowerFactory();
    protected ItemUICanvasModel[] itemUICanvasModels;
    Ray ray;
    RaycastHit hit;

    public override void SetUpInStart()
    {
        base.SetUpInStart();
        ShowItemsUICanvas.gameObject.SetActive(false);

        Tower[] towers = new Tower[AvailableTowers.Count];
        for (int i = 0; i < AvailableTowers.Count; i++)
        {
            towers[i] = AvailableTowers[i].gameObject.GetComponent<Tower>();
            GoldCoin g1 = new GoldCoin(towers[i].GoldToBuy);
            GoldCoin g2 = new GoldCoin(towers[i].GoldToSell);

            towers[i].Price = new Price(g1);
            towers[i].CostToBuild = new Price(g2);
        }

        Factory.prototypes = towers;
        Debug.Log("Tower plot: " + DateTime.Now + Random.Range(-10.0f, 10.0f));
    }

    public override void UpdatePerFrame()
    {

        base.UpdatePerFrame();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 9;

        if (Input.GetKey(KeyCode.Mouse0) && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
//            Debug.Log("UpdatePerFrame: " + transform.position);

            if (!ShowItemsUICanvas.gameObject.activeSelf)
            {
                itemUICanvasModels = Factory.ShowTowerInUICanvas(this);
                ShowItemsUICanvas.ShowItems(itemUICanvasModels);
            }
        }
    }

//
//    private void OnMouseDown()
//    {
//        Debug.Log("Fuck you: " + DateTime.Now);
//        if (Input.GetKey(KeyCode.Mouse0) && !ShowItemsUICanvas.gameObject.activeSelf)
//        {
//            itemUICanvasModels = Factory.ShowTowerInUICanvas(this);
//            ShowItemsUICanvas.ShowItems(itemUICanvasModels);
//        }
//    }

    public void OnClick(ItemUICanvasModel model)
    {
        if (Factory.CanBuild((Tower) model.Object))
        {
            if (CurrentTower != null)
            {
                CurrentTower.transform.parent = null;
                Destroy(CurrentTower.gameObject);
            }
            Debug.Log("OnClick: " + gameObject.transform.position);
            CurrentTower = Factory.Build((Tower) model.Object, gameObject.transform);
            

            ShowItemsUICanvas.OnClose();
        }
    }
}