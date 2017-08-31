using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class PeriodicTable : MonoBehaviour
{
    #region Command implementation
    private interface ICommand
    {
        void Execute(TableElement element);
    }

    private class ShowAtomCommand : ICommand
    {
        public void Execute(TableElement element)
        {
            throw new NotImplementedException();
        }
    }

    private class HideAtomCommand : ICommand
    {
        public void Execute(TableElement element)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    private TableElement _selectedElement;
    public TableElement SelectedElement { get { return _selectedElement; } }

    [SerializeField]
    private ProjectorController _projector;

    public GameObject projectorPrefab;

    public MeshRenderer shape;

    public MeshRenderer periodShape;

    public MeshRenderer groupShape;

    public MeshRenderer elementShape;

    private List<TableElement>[] groups;

    public void SelectElement(TableElement element)
    {
        if (_selectedElement != null)
        {
            _selectedElement.CanselSelect();
        }

        _selectedElement = element;

        _projector.CreateAtomProjection(_selectedElement);
    }

    public void ShowCommand()
    {
        projectorPrefab.SetActive(true);
    }

    public void DeselectElement(TableElement element)
    {
        if(_selectedElement != null)
        {
            _selectedElement.SimpleDeselect();
        }

        _selectedElement = null;
        _projector.DestroyAtomProjection();
    }

    public void HideCommand()
    {
        projectorPrefab.SetActive(false);
    }

    void Awake()
    {
        groups = new List<TableElement>[8];

        for(int i = 0; i < 8; i++)
        {
            groups[i] = new List<TableElement>();
        }

        foreach(Transform period in transform)
        {
            if(period.GetComponent<TablePeriod>())
            {
                foreach(Transform child in period)
                {
                    TableElement tableElement = child.GetComponent<TableElement>();
                    TableGroup tableGroup = child.GetComponent<TableGroup>();
                    if(tableElement != null)
                    {
                        AtomInformation atomInfo = DataManager.Instance.GetAtominfo(tableElement.atomName);
                        groups[atomInfo.type].Add(tableElement);
                    }

                    if(tableGroup != null)
                    {
                        foreach(Transform groupElement in tableGroup.transform)
                        {
                            TableElement groupTableElement = groupElement.GetComponent<TableElement>();

                            if(groupTableElement != null)
                            {
                                AtomInformation atomInfo = DataManager.Instance.GetAtominfo(groupTableElement.atomName);
                                groups[atomInfo.type].Add(groupTableElement);
                            }
                        }
                    }
                }
            }
        }
    }

    public void HighLightGroup(int groupIndex, float duration, int loopsCount, bool clear = false)
    {
        foreach(TableElement element in groups[groupIndex])
        {
            element.GetComponent<MeshRenderer>().material.DOColor(Color.white, duration).SetLoops(loopsCount, LoopType.Yoyo).Play();
        }

        if (clear)
        {
            foreach (TableElement element in groups[groupIndex])
            {
                element.GetComponent<MeshRenderer>().material.DOComplete();
            }
        }
    }

    public TableElement GetRandomTableElementInGroup(int groupIndex)
    {
        return groups[groupIndex][UnityEngine.Random.Range(0, groups[groupIndex].Count)];
    }
}
