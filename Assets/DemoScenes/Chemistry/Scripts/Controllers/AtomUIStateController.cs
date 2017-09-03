using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class AtomUIStateController : MonoBehaviour
{
    #region Command implementation
    private class ChangeViewToClassic3DCommand : ICommand
    {
        private Atom _atom;

        public ChangeViewToClassic3DCommand(Atom atom)
        {
            _atom = atom;
        }

        public void Execute()
        {
            _atom.ChangeStateToClassic3D();
        }
    }

    private class ChangeViewToClassic2DCommand : ICommand
    {
        private Atom _atom;
        private Vector3? direction;

        public ChangeViewToClassic2DCommand(Atom atom, Vector3? direction = null)
        {
            _atom = atom;
            this.direction = direction;
        }

        public void Execute()
        {
            _atom.ChangeStateToClassic2D(direction);
        }
    }

    private class ChangeViewToRealCommand : ICommand
    {
        private Atom _atom;

        public ChangeViewToRealCommand(Atom atom)
        {
            _atom = atom;
        }

        public void Execute()
        {
            _atom.ChangeStateToReal();
        }
    }
    #endregion

    private ProjectorController _projector;

    [SerializeField]
    private AtomUIStateButton _classic3DButton;

    [SerializeField]
    private AtomUIStateButton _classic2DButton;

    [SerializeField]
    private AtomUIStateButton _realButton;

    private AtomUIStateButton _pressedButton;

    [SerializeField]
    private TextMeshPro _atomFormulaText;
    public TextMeshPro atomFormulaText { get { return _atomFormulaText; } }

    [SerializeField]
    private TextMeshPro _atomNameText;
    public TextMeshPro atomNameText { get { return _atomNameText; } }

    void Awake()
    {
        _projector = GetComponentInParent<ProjectorController>();
        // gameObject.SetActive(false);
    }

    void Update()
    {
       transform.LookAt(Camera.main.transform.position);
    }

    public void ChangeLocalViewTo3D(bool to3D)
    {
        if (to3D)
        {
            ChangeView(new ChangeViewToClassic3DCommand(_projector.GetProjectedAtom()), _classic3DButton);
            return;
        }

        Atom projAtom = _projector.GetProjectedAtom();
        Vector3 directionToCamera = Camera.main.transform.position - projAtom.transform.position;
        ChangeView(new ChangeViewToClassic2DCommand(_projector.GetProjectedAtom(), directionToCamera), _classic2DButton);
    }

    public void ChangeViewToClassic3D()
    {
        SV_Sharing.Instance.SendBool(true, "change_view_to_3d");

        ChangeView(new ChangeViewToClassic3DCommand(_projector.GetProjectedAtom()), _classic3DButton);
    }

    public void ChangeViewToClassic2D()
    {
        SV_Sharing.Instance.SendBool(false, "change_view_to_2d");

        Atom projAtom = _projector.GetProjectedAtom();
        Vector3 directionToCamera = Camera.main.transform.position - projAtom.transform.position;
        ChangeView(new ChangeViewToClassic2DCommand(_projector.GetProjectedAtom(), directionToCamera), _classic2DButton);
    }

    public void ChangeViewToReal()
    {
        ChangeView(new ChangeViewToRealCommand(_projector.GetProjectedAtom()), _realButton);
    }

    private void ChangeView(ICommand command, AtomUIStateButton newButton)
    {
        command.Execute();

        if(_pressedButton != null)
        {
            _pressedButton.Deselect();
        }

        _pressedButton = newButton;
        _pressedButton.Select();
    }
}
