using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class Atom : MonoBehaviour
{
    #region State implementation
    private interface IAtomState
    {
        void ChangeStateToNone();
        void ChangeStateToClassic3D();
        void ChangeStateToClassic2D(Vector3? direction = null);
        void ChangeStateToReal();
    }

    private class AtomNoneState : IAtomState
    {
        Atom _atom;
        Sequence seq;

        public AtomNoneState(Atom atom)
        {
            _atom = atom;
        }

        public void ChangeStateToClassic2D(Vector3? direction = null)
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;
            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));
            seq.AppendCallback(() =>
            {
                _atom._classicModel.SetActive(true);

                foreach(Orbit orbit in _atom._orbitManager.orbits)
                {
                    if(direction != null)
                    {
                        orbit.transform.rotation = Quaternion.LookRotation(direction.Value);
                    }
                    else
                    {
                        //orbit.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
                        orbit.transform.localRotation = Quaternion.identity;
                    }

                    orbit.orbitLine.rectTransform.gameObject.SetActive(true);
                }

                _atom._state = new AtomClassic3DState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }

        public void ChangeStateToClassic3D()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;
            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));
            seq.AppendCallback(() =>
            {
                _atom._classicModel.SetActive(true);

                foreach(Orbit orbit in _atom._orbitManager.orbits)
                {
                    orbit.orbitLine.rectTransform.gameObject.SetActive(true);
                }

                _atom._state = new AtomClassic3DState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }

        public void ChangeStateToNone() { }

        public void ChangeStateToReal()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;
            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));
            seq.AppendCallback(() =>
            {
                _atom._realModel.SetActive(true);
                _atom._state = new AtomRealState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }
    }

    private class AtomClassic3DState : IAtomState
    {
        private Atom _atom;
        Sequence seq;

        public AtomClassic3DState(Atom atom)
        {
            _atom = atom;
        }

        public void ChangeStateToClassic2D(Vector3? direction = null)
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 0f;

            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            foreach(Orbit orbit in _atom._orbitManager.orbits)
            {
                if(direction != null)
                {
                    //seq.Insert(0f, orbit.transform.DOLookAt(Camera.main.transform.position, _atom._transitionDuration));
                    seq.Insert(0f, orbit.transform.DORotateQuaternion(Quaternion.LookRotation(direction.Value), _atom._transitionDuration));
                }
                else
                {
                    seq.Insert(0f, orbit.transform.DOLocalRotateQuaternion(Quaternion.identity, _atom._transitionDuration));
                }
            }

            seq.AppendCallback(() =>
            {
                _atom._state = new AtomClassic2DState(_atom);
            });

            seq.Play();
        }

        public void ChangeStateToClassic3D()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 0f;

            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));
            foreach(Orbit orbit in _atom._orbitManager.orbits)
            {
                seq.Insert(0f, orbit.transform.DOLookAt(orbit.transform.position + orbit.initialDirection, _atom._transitionDuration));
            }

            seq.AppendCallback(() =>
            {
                _atom._state = new AtomClassic3DState(_atom);
            });

            seq.Play();
        }

        public void ChangeStateToNone()
        {

        }

        public void ChangeStateToReal()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;

            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.AppendCallback(() =>
            {
                _atom._classicModel.SetActive(false);

                foreach(Orbit orbit in _atom._orbitManager.orbits)
                {
                    orbit.orbitLine.rectTransform.gameObject.SetActive(false);
                }

                _atom._realModel.SetActive(true);

                _atom._state = new AtomRealState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }
    }

    private class AtomClassic2DState : IAtomState
    {
        private Atom _atom;
        Sequence seq;

        public AtomClassic2DState(Atom atom)
        {
            _atom = atom;
        }

        public void ChangeStateToClassic2D(Vector3? direction = null)
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 0f;

            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            foreach(Orbit orbit in _atom._orbitManager.orbits)
            {
                if(direction != null)
                {
                    seq.Insert(0f, orbit.transform.DORotateQuaternion(Quaternion.LookRotation(direction.Value), _atom._transitionDuration));
                }
                else
                {
                    //seq.Insert(0f, orbit.transform.DOLookAt(Camera.main.transform.position, _atom._transitionDuration));
                    seq.Insert(0f, orbit.transform.DOLocalRotateQuaternion(Quaternion.identity, _atom._transitionDuration));
                }
            }

            seq.AppendCallback(() =>
            {
                _atom._state = new AtomClassic2DState(_atom);
            });

            seq.Play();
        }

        public void ChangeStateToClassic3D()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 0f;

            seq.Insert(0f, _atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            foreach(Orbit orbit in _atom._orbitManager.orbits)
            {
                seq.Insert(0f, orbit.transform.DOLookAt(orbit.transform.position + orbit.initialDirection, _atom._transitionDuration));
            }

            seq.AppendCallback(() =>
            {
                _atom._state = new AtomClassic3DState(_atom);
            });

            seq.Play();
        }

        public void ChangeStateToNone()
        {
        }

        public void ChangeStateToReal()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;

            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.AppendCallback(() =>
            {
                _atom._classicModel.SetActive(false);

                foreach(Orbit orbit in _atom._orbitManager.orbits)
                {
                    orbit.orbitLine.rectTransform.gameObject.SetActive(false);
                }

                _atom._realModel.SetActive(true);

                _atom._state = new AtomRealState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }
    }

    private class AtomRealState : IAtomState
    {
        private Atom _atom;
        Sequence seq;

        public AtomRealState(Atom atom)
        {
            _atom = atom;
        }

        public void ChangeStateToClassic2D(Vector3? direction = null)
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;

            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.AppendCallback(() =>
            {
                _atom._classicModel.SetActive(true);

                foreach(Orbit orbit in _atom._orbitManager.orbits)
                {
                    if(direction != null)
                    {
                        orbit.transform.rotation = Quaternion.LookRotation(direction.Value);
                    }
                    else
                    {
                        orbit.transform.localRotation = Quaternion.identity;
                    }
                    
                    orbit.orbitLine.rectTransform.gameObject.SetActive(true);
                }

                _atom._realModel.SetActive(false);

                _atom._state = new AtomClassic2DState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }

        public void ChangeStateToClassic3D()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 1f;

            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.AppendCallback(() =>
            {
                _atom._classicModel.SetActive(true);

                foreach(Orbit orbit in _atom._orbitManager.orbits)
                {
                    orbit.transform.rotation = Quaternion.LookRotation(orbit.initialDirection);
                    orbit.orbitLine.rectTransform.gameObject.SetActive(true);
                }

                _atom._realModel.SetActive(false);

                _atom._state = new AtomClassic3DState(_atom);
            });

            toColor.a = 0f;
            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }

        public void ChangeStateToNone()
        {
        }

        public void ChangeStateToReal()
        {
            seq.Kill();
            seq = DOTween.Sequence();

            Color toColor = _atom._shell.material.color;
            toColor.a = 0f;

            seq.Append(_atom._shell.material.DOColor(toColor, _atom._transitionDuration));

            seq.Play();
        }
    }
    #endregion

    private IAtomState _state;

    private AtomInformation _atomInformation;
    public AtomInformation atomInformation { get { return _atomInformation; } }

    bool _isInitialized;

    [SerializeField]
    private GameObject _classicModel;

    [SerializeField]
    private GameObject _realModel;

    [SerializeField]
    private Core _classicCore;
    public Core classicCore { get { return _classicCore; } }

    [SerializeField]
    private Core _realCore;
    public Core realCore { get { return _classicCore; } }

    [SerializeField]
    private OrbitManager _orbitManager;
    public OrbitManager orbitManager { get { return _orbitManager; } }

    [SerializeField]
    private ProbabilityManager _probabilityManager;
    public ProbabilityManager probabilityManager { get { return _probabilityManager; } }

    [SerializeField]
    private MeshRenderer _shell;
    public MeshRenderer shell { get { return _shell; } }

    private float _transitionDuration = 1f;

    public void Initialize(AtomInformation atomInformation, int[] holes = null, int holesOffset = 0, List<List<float>> overridedPositions = null)
    {
        if(_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        _atomInformation = atomInformation;

        name = _atomInformation.name;

        _classicCore.Initialize(_atomInformation);
        _orbitManager.Initialize(_atomInformation, holes, holesOffset, overridedPositions);

        _realCore.Initialize(_atomInformation);
        _probabilityManager.Initialize(_atomInformation);

        float atomShellRadius = _orbitManager.orbits.Count * 2f + 1f;
        _shell.material.renderQueue = 3999;
        _shell.transform.localScale = new Vector3(atomShellRadius, atomShellRadius, atomShellRadius);

        _classicModel.SetActive(false);
        _realModel.SetActive(false);

        foreach(Orbit orbit in _orbitManager.orbits)
        {
            orbit.orbitLine.rectTransform.gameObject.SetActive(false);
        }

        Color newColor = _shell.material.color;
        newColor.a = 0f;
        _shell.material.color = newColor;

        //transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        _state = new AtomNoneState(this);
    }

    public void ChangeStateToNone()
    {
        _state.ChangeStateToNone();
    }

    public void ChangeStateToClassic3D()
    {
        _state.ChangeStateToClassic3D();
    }

    public void ChangeStateToClassic2D(Vector3? direction = null)
    {
        _state.ChangeStateToClassic2D(direction);
    }

    public void ChangeStateToReal()
    {
        _state.ChangeStateToReal();
    }

    public void ResetOrbits(float duration = 1f)
    {
        foreach(Orbit orbit in _orbitManager.orbits)
        {
            orbit.transform.DOLocalRotateQuaternion(Quaternion.identity, duration).Play();
        }
    }

    public List<Transform> GetLastOrbitElectrons()
    {
        return _orbitManager.orbits[_orbitManager.orbits.Count - 1].GetElectrons();
    }

    public List<Transform> GetOrbitElectrons(int orbitIndex)
    {
        return _orbitManager.orbits[orbitIndex].GetElectrons();
    }

    public List<Transform> GetOrbitHoles(int orbitIndex)
    {
        return _orbitManager.GetOrbitHoles(orbitIndex);
    }

    public void ChangeCore(Core newCore)
    {
        Destroy(_classicCore.gameObject);

        newCore.transform.SetParent(_classicModel.transform);
        _classicCore = newCore;
    }

    public Proton[] GetProtons()
    {
        return _classicCore.GetComponentsInChildren<Proton>();
    }

    public Neutron[] GetNeutrons()
    {
        return _classicCore.GetComponentsInChildren<Neutron>();
    }

    public void StopRotating(float duration = 0f)
    {
        _orbitManager.StopRotating(duration);
    }

    public void SetRotatingToInitial(float duration)
    {
        _orbitManager.SetRotatingToInitial(duration);
    }

    public void SetLastElectronToOtherAtom(Atom otherAtom, float duration)
    {
        //TODO: works not right, need to update electrons information in orbit classes in future

        Transform electron = GetLastOrbitElectrons()[0];
        Transform hole = otherAtom.GetOrbitHoles(otherAtom.orbitManager.orbits.Count - 1)[0];
        electron.DOMove(hole.position, duration).OnComplete(() =>
        {
            electron.SetParent(otherAtom.orbitManager.orbits[otherAtom.orbitManager.orbits.Count - 1].electronOrbit);
            Destroy(hole.gameObject);

            Orbit lastOrbit = _orbitManager.orbits[_orbitManager.orbits.Count - 1];
            lastOrbit.orbitLine.material = new Material(Shader.Find("Shader Forge/UnlitTransparent"));

            Color c = Color.gray;
            c.a = 0f;
            lastOrbit.orbitLine.material.DOColor(c, 1f).Play();
        }).Play();
    }

    public void RotateOrbitBy(int orbitIndex, Vector3 eulers)
    {
        _orbitManager.RotateOrbitBy(orbitIndex, eulers);
    }
}
