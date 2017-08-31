using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;

public class TableElement : MonoBehaviour, IInteractive
{
    private AudioSource audioSource;

    private PeriodicTable _periodicTable;

    [SerializeField]
    private string _atomName;
    public string atomName { get { return _atomName; } }

    [SerializeField]
    private List<ActionType> _allowedActions = new List<ActionType>();

    private Color _selfColor = Color.black;
    private Color _highlightColor = Color.gray;
    private Color _selectColor = Color.white;

    [SerializeField]
    private TextMeshPro _elementText;

    private bool _isSelected = false;
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
    }

    void Awake()
    {        
        _atomName = name;
        _periodicTable = GetComponentInParent<PeriodicTable>();
        _elementText = GetComponentInChildren<TextMeshPro>();
  
        _isSelected = false;
    }

    void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource = AudioManager.Instance.AudioSourceSettings(audioSource);
    }

    public List<ActionType> GetAllowedActions() { return _allowedActions; }

    public void OnGazeEnter()
    {
        if (PlayerManager.Instance.Strategy != InputStrategyFacade.Strategies.Default ||
            _isSelected)
            return;

        _elementText.color = _highlightColor;

        SV_Sharing.Instance.SendInt(GetComponent<IDHolder>().ID, "highlight_element");
    }

    public void OnGazeLeave()
    {
        if (PlayerManager.Instance.Strategy != InputStrategyFacade.Strategies.Default ||
            _isSelected)
            return;

        Debug.Log("Gaze left at " + _atomName);
        CanselHighlighting();

        SV_Sharing.Instance.SendInt(GetComponent<IDHolder>().ID, "dehighlight_element");
    }

    public void HighlightElement()
    {
        if (_isSelected)
            return;

        _elementText.color = _highlightColor;
    }

    public void CanselHighlighting()
    {
        if (_isSelected)
            return;

        _elementText.color = _selfColor;
    }

    public void OnGestureTap()
    {
        Debug.Log("!Tapped the element " + _atomName);

        if (PlayerManager.Instance.Strategy != InputStrategyFacade.Strategies.Default)
            return;

        audioSource.Play();

        if (!_isSelected)
        {
            Select();
        }
        else
        {
            Deselect();
        }

        SV_Sharing.Instance.SendInt(GetComponent<IDHolder>().ID, "select_element");
    }

    public void StopDrag() { }

    public bool TryToDrag() { return false; }

    public void Select()
    {
        _isSelected = true;
        _elementText.color = _selectColor;

        _periodicTable.SelectElement(this);
    }

    public void Deselect()
    {
        _isSelected = false;
        _elementText.color = _highlightColor;

        _periodicTable.DeselectElement(this);
    }

    public void CanselSelect()
    {
        
        Deselect();
        CanselHighlighting();
    }

    public void SimpleDeselect()
    {
        _isSelected = false;
        _elementText.color = _selfColor;
    }

    public bool TryToResize() { return false; }
}
