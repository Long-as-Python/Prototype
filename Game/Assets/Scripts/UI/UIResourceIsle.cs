﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceIsle : UIController
{
    private ResourcesIsle _isle;
    [SerializeField] private RectTransform _buttonPrefab;
    [SerializeField] private RectTransform _contentList;
    [SerializeField] private ResourceInfo _resourceinfo;
    private List<ResourceButton> _buttons = new List<ResourceButton>();
    [SerializeField] private Scrollbar _scroll;

    private int _buttonNum;
    private int _level;
    private int _maxLevel;

    private void Awake()
    {
        _buttonNum = 0;

    }

    public override void UpdateAll()
    {
        _isle = UIManager._instance.LastActiveIsle as ResourcesIsle;
        if (_isle == null)
            Debug.LogError("Isle type error");

        _isle.OnRefresh += UpdateByIndex;

        if (_buttons != null)
            _buttons.ForEach(b => Destroy(b.gameObject));
        _buttons.Clear();

        UpdateStaticInfo();
        UpdateDynamicInfo();
        UpdateResourceInfo();

        StartCoroutine(UpdateLayout());
    }

    private IEnumerator UpdateLayout()
    {
        yield return new WaitForFixedUpdate();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform as RectTransform);
        _scroll.value = 1;

    }

    private void UpdateStaticInfo()
    {
        _level = _isle.Level;
        _maxLevel = _isle.Logic.Info.Length;

        for(int i = 0; i < _maxLevel; i++)
        {
            var buttonObj = Instantiate(_buttonPrefab.gameObject);
            buttonObj.transform.SetParent(_contentList, false);
            var button = buttonObj.GetComponent<ResourceButton>();
            _buttons.Add(button);
            if(i >= _level)
            {
                button.Button.interactable = false;
                button.ClearCount();
            }
            _buttons[i].ChangeName(_isle.Logic.Info[i].Item.Name);
        }
    }

    private void UpdateByIndex(int i)
    {
        int count = _isle.Items.Container[i].Amount;
        int maxCount = _isle.Logic.Info[i].MaxAmount;
        _buttons[i].ChangeCount(count, maxCount);
        _buttons[i].ChangeProgress(_isle.RefreshedItems[_isle.Logic.Info[i].Item]);
    }

    //rewrite 
    private void UpdateDynamicInfo()
    {
        for (int i = 0; i < _level; i++)
        {
            int count = _isle.Items.Container[i].Amount;
            int maxCount = _isle.Logic.Info[i].MaxAmount;
            _buttons[i].ChangeCount(count, maxCount);
            _buttons[i].ChangeProgress(_isle.RefreshedItems[_isle.Logic.Info[i].Item]);
        }

        //rewrite
        _resourceinfo.ChangeCount(_isle.Items.Container[_buttonNum].Amount, _isle.Logic.Info[_buttonNum].MaxAmount);
    }

    private void UpdateResourceInfo()
    {
        Item item = _isle.Logic.Info[_buttonNum].Item;
        _resourceinfo.ChangeInfo(item.Name, item.Description, item.Icon);
        _resourceinfo.ChangeCount(_isle.Items.Container[_buttonNum].Amount, _isle.Logic.Info[_buttonNum].MaxAmount);
    }

    private void OnDisable()
    {
        _isle.OnRefresh -= UpdateByIndex;
    }
}
