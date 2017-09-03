using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using GSequence = Gemeleon.Sequence;
using TMPro;

public class SequenceInitializator : MonoBehaviour
{
    private bool _isInitialized;

    [SerializeField]
    private AudioClip _professorHelloAudio;

    public void Initialize()
    {
        if(_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        BaseGIDInitialize();
    }

    private void BaseGIDInitialize()
    {
        GSequence baseGIDSequence = new GSequence();

        #region firstSub
        SubSequence firstSub = new SubSequence("firstSub");
        firstSub.subSequence.SetAutoKill(false);
        firstSub.nextSubSequenceIndex = 2;

        ProfessorController professor = null;
        PeriodicTable periodicTable = GameObject.FindObjectOfType<PeriodicTable>();
        QuestionButton button = null;
        DetailedElement detailedElement = null;

        // Professor fly to table and saying
        firstSub.subSequence.AppendCallback(() =>
        {
            professor = Instantiate(PrefabManager.Instance.professorPrefab).GetComponent<ProfessorController>();
            PlatformController platfrom = professor.GetComponentInChildren<PlatformController>();
            platfrom.transform.position = Camera.main.transform.position - Camera.main.transform.forward;
            platfrom.transform.position += Vector3.up;

            professor.GetTarget().transform.position = periodicTable.transform.position + periodicTable.transform.forward + periodicTable.transform.right;
            professor.GetTarget().transform.LookAt(periodicTable.transform);
            //AudioManager.Instance.PlayAudio(_professorHelloAudio);
        });

        // table shape highlighting
        firstSub.subSequence.InsertCallback(1f, () =>
        {
            professor.ActivateGestures();

            periodicTable.shape.enabled = true;

            periodicTable.shape.material.DOColor(new Color(1f, 1f, 0f, 0.4f), 0.4f).SetLoops(8, LoopType.Yoyo).OnComplete(() => { periodicTable.shape.enabled = false; }).Play();
        });

        firstSub.subSequence.InsertCallback(5f, () =>
        {
            professor.DeactivateGestures();

            button = Instantiate(PrefabManager.Instance.questionButtonPrefab).GetComponent<QuestionButton>();
            button.transform.position = professor.transform.GetChild(0).position + new Vector3(0f, 0.6f, 0f);

            //button.onClickEvent += () =>
            //{
            //    firstSub.nextSubSequenceIndex = 1;

            //    button.FadeOut();
            //};

            //button.FadeIn();
        });

        firstSub.subSequence.InsertCallback(10f, () =>
        {
            //button.FadeOut();
        });
        #endregion

        #region secondSub
        SubSequence secondSub = new SubSequence("secondSub");
        secondSub.subSequence.SetAutoKill(false);
        secondSub.nextSubSequenceIndex = 2;

        secondSub.subSequence.AppendCallback(() =>
        {
            Destroy(button);
            //AudioManager.Instance.PlayAudio(_professorHelloAudio);
        });

        secondSub.subSequence.InsertCallback(1f, () =>
        {
            periodicTable.transform.DOScale(0f, 1f).Play();
        });

        secondSub.subSequence.InsertCallback(2f, () =>
        {
            detailedElement = Instantiate(PrefabManager.Instance.detailedElementPrefab).GetComponent<DetailedElement>();
            detailedElement.transform.localScale = Vector3.zero;
            detailedElement.transform.position = periodicTable.transform.position;
            detailedElement.transform.rotation = Quaternion.LookRotation(-periodicTable.transform.forward);

            detailedElement.transform.DOScale(new Vector3(1f, 1f, 0.05f), 1f).Play();
        });

        secondSub.subSequence.InsertCallback(3f, () =>
        {
            detailedElement.indexText.DOColor(Color.white, 0.4f).SetLoops(6, LoopType.Yoyo).Play();
        });

        secondSub.subSequence.InsertCallback(6f, () =>
        {
            detailedElement.shortNameText.DOColor(Color.white, 0.4f).SetLoops(6, LoopType.Yoyo).Play();
            detailedElement.fullNameText.DOColor(Color.white, 0.4f).SetLoops(6, LoopType.Yoyo).Play();
        });

        secondSub.subSequence.InsertCallback(9f, () =>
        {
            detailedElement.molarText.DOColor(Color.white, 0.4f).SetLoops(6, LoopType.Yoyo).Play();
        });

        secondSub.subSequence.InsertCallback(12f, () =>
        {
            detailedElement.transform.DOScale(0f, 1f).Play();
        });

        secondSub.subSequence.InsertCallback(13f, () =>
        {
            Destroy(detailedElement);
            periodicTable.transform.DOScale(1f, 1f).SetEase(Ease.InOutElastic).Play();
        });

        secondSub.subSequence.AppendInterval(1f);
        #endregion

        #region thirdSub
        SubSequence thirdSub = new SubSequence("thirdSub");
        thirdSub.subSequence.SetAutoKill(false);
        thirdSub.nextSubSequenceIndex = -1;

        thirdSub.subSequence.AppendCallback(() =>
        {
            //AudioManager.Instance.PlayAudio(_professorHelloAudio);
        });

        thirdSub.subSequence.InsertCallback(0f, () =>
        {
            periodicTable.periodShape.enabled = true;
            periodicTable.periodShape.material.DOColor(Color.yellow, 0.4f).SetLoops(10, LoopType.Yoyo).OnComplete(() =>
            {
                periodicTable.periodShape.enabled = false;
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(4f, () =>
        {
            periodicTable.groupShape.enabled = true;
            periodicTable.groupShape.material.DOColor(Color.yellow, 0.4f).SetLoops(10, LoopType.Yoyo).OnComplete(() =>
            {
                periodicTable.groupShape.enabled = false;
            }).Play();
        });

        AtomFactory atomFactory = new AtomFactory();
        int offsetTime = 0;

        Transform atomsContainer = new GameObject("AtomsContainer").transform;
        atomsContainer.SetParent(periodicTable.transform);
        atomsContainer.transform.localPosition = new Vector3(-0.75f, 0.4f, 0f);
        atomsContainer.transform.localRotation = Quaternion.Euler(0f, -140f, 0f);
        atomsContainer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        Transform elShapes;
        elShapes = new GameObject("ElShapes").transform;

        for(int i = 0; i < 3; i++)
        {
            Transform period = periodicTable.transform.GetChild(i);

            int k = 0;
            foreach(Transform element in period)
            {
                if(element.GetComponent<TableElement>() == null)
                {
                    continue;
                }

                int j = i;
                int tmpOffsetTime = offsetTime++;
                Transform tmpElement = element;

                Atom atom = null;

                thirdSub.subSequence.InsertCallback(8f + tmpOffsetTime / 2f, () =>
                {
                    periodicTable.elementShape.transform.position = tmpElement.position;

                    atom = atomFactory.CreateAtom(tmpElement.name);
                    //atom.Initialize(DataManager.Instance.GetAtominfo(tmpElement.name));
                    atom.transform.SetParent(atomsContainer);

                    atom.ChangeStateToClassic3D();

                    atom.transform.localPosition = new Vector3(k++ * 0.4f, -(j) * 0.4f, 0f);
                    atom.transform.localEulerAngles = Vector3.zero;
                    atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                    Transform elShape = Instantiate(PrefabManager.Instance.tableElementShapePrefab).transform;
                    elShape.position = tmpElement.position;
                    elShape.rotation = tmpElement.rotation;
                    elShape.SetParent(elShapes);
                    elShape.name = atom.name;
                    elShape.GetComponent<MeshRenderer>().material.DOColor(Color.yellow, 0.5f).Play();

                    TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
                    nameText.transform.SetParent(atom.transform);
                    nameText.transform.localScale = Vector3.one;
                    nameText.transform.localPosition = new Vector3(0f, -4f, 0f);
                    nameText.transform.localRotation = Quaternion.identity;
                    nameText.text = atom.name;

                    nameText.fontSize = 8f;
                    nameText.alignment = TextAlignmentOptions.Center;

                    TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
                    tc.width = 6f;
                    tc.height = 1f;
                });
            }
        }

        thirdSub.subSequence.InsertCallback(8f, () =>
        {
            //periodicTable.elementShape.enabled = true;
            //periodicTable.elementShape.material.DOColor(Color.yellow, 0.5f).SetLoops(36, LoopType.Yoyo).OnComplete(() =>
            //{
            //    periodicTable.elementShape.enabled = false;
            //}).Play();

            professor.GetTarget().transform.position = periodicTable.transform.TransformPoint(-1.8f, -0.15f, 1.6f);
            professor.GetTarget().transform.LookAt(atomsContainer.transform.position);
            professor.ActivateGestures();
        });

        thirdSub.subSequence.InsertCallback(18f, () =>
        {
            for(int i = 0; i < 10; i++)
            {
                elShapes.GetChild(i).GetComponent<MeshRenderer>().material.DOColor(new Color(1f, 1f, 0f, 0f), 1f).Play();
                atomsContainer.GetChild(i).DOScale(0f, 1f).Play();
            }
        });

        thirdSub.subSequence.InsertCallback(19f, () =>
        {
            for(int i = 0; i < 10; i++)
            {
                Destroy(elShapes.GetChild(i).gameObject);
                Destroy(atomsContainer.GetChild(i).gameObject);
            }

            atomsContainer.DOLocalMove(new Vector3(0.875f, 0f, 0.2f), 1f).Play();
            atomsContainer.DOLocalRotate(new Vector3(0f, 180f, 0f), 1f).Play();
        });

        thirdSub.subSequence.InsertCallback(20f, () =>
        {
            foreach(Transform atomTransform in atomsContainer)
            {
                atomTransform.GetComponent<Atom>().ResetOrbits(1f);
            }

            professor.GetTarget().position = periodicTable.transform.TransformPoint(new Vector3(1.4f, 0.2f, 1f));
            professor.GetTarget().LookAt(periodicTable.transform.position);
        });

        for(int i = 0; i < 8; i++)
        {
            int j = i;
            thirdSub.subSequence.InsertCallback(21f + j * 2, () =>
            {
                elShapes.GetChild(j).GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(10, LoopType.Yoyo).Play();

                List<Transform> electrons = atomsContainer.GetChild(j).GetComponent<Atom>().GetLastOrbitElectrons();

                foreach(Transform electron in electrons)
                {
                    electron.DOScale(0.6f, 0.2f).SetLoops(10, LoopType.Yoyo).Play();
                }
            });
        }

        thirdSub.subSequence.InsertCallback(37f, () =>
        {
            for(int i = 0; i < 8; i++)
            {
                elShapes.GetChild(i).GetComponent<MeshRenderer>().material.DOColor(new Color(1f, 1f, 0f, 0f), 1f).Play();
                atomsContainer.GetChild(i).DOScale(0f, 1f).Play();
            }
        });

        thirdSub.subSequence.InsertCallback(38f, () =>
        {
            for(int i = 0; i < 8; i++)
            {
                Destroy(elShapes.GetChild(i).gameObject);
                Destroy(atomsContainer.GetChild(i).gameObject);
            }

            atomsContainer.position = periodicTable.transform.TransformPoint(new Vector3(0f, -0.5f, 1f));
            atomsContainer.localRotation = Quaternion.identity;
        });

        thirdSub.subSequence.InsertCallback(38f, () =>
        {
            periodicTable.HighLightGroup(0, 0.2f, 10);

            Atom atom = atomFactory.CreateAtom("Sodium");

            //atom.Initialize(DataManager.Instance.GetAtominfo("Sodium"));
            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            foreach(Transform electron in atom.GetLastOrbitElectrons())
            {
                electron.DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
            }

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(-atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(42f, () =>
        {
            periodicTable.HighLightGroup(1, 0.2f, 10);

            TableElement tElement = periodicTable.GetRandomTableElementInGroup(1);
            Atom atom = atomFactory.CreateAtom(tElement.atomName);

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.AppendInterval(4f);

        thirdSub.subSequence.InsertCallback(46f, () =>
        {
            periodicTable.HighLightGroup(2, 0.2f, 10);

            Atom atom = atomFactory.CreateAtom("Nickel", new int[] { 0, 0, 2, 0 });

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            foreach(Transform hole in atom.GetOrbitHoles(2))
            {
                hole.DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
            }

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(50f, () =>
        {
            periodicTable.HighLightGroup(3, 0.2f, 10);

            Atom atom = atomFactory.CreateAtom("Aluminium");

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToReal();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        //thirdSub.subSequence.AppendInterval(4f);

        thirdSub.subSequence.InsertCallback(54f, () =>
        {
            periodicTable.HighLightGroup(4, 0.2f, 10);

            TableElement tElement = periodicTable.GetRandomTableElementInGroup(4);
            Atom atom = atomFactory.CreateAtom(tElement.atomName);

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        //thirdSub.subSequence.AppendInterval(4f);

        thirdSub.subSequence.InsertCallback(58f, () =>
        {
            periodicTable.HighLightGroup(5, 0.2f, 10);

            TableElement tElement = periodicTable.GetRandomTableElementInGroup(5);
            Atom atom = atomFactory.CreateAtom(tElement.atomName);

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        //thirdSub.subSequence.AppendInterval(4f);

        thirdSub.subSequence.InsertCallback(62f, () =>
        {
            periodicTable.HighLightGroup(5, 0.2f, 10);

            Atom atom = atomFactory.CreateAtom("Magnesium");

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            foreach(Transform electrons in atom.GetLastOrbitElectrons())
            {
                electrons.DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
            }

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(66f, () =>
        {
            periodicTable.HighLightGroup(6, 0.2f, 10);

            Atom atom = atomFactory.CreateAtom("Cerium");

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(70f, () =>
        {
            periodicTable.HighLightGroup(7, 0.2f, 10);

            Atom atom = atomFactory.CreateAtom("Uranium");

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(3f);
            seq.AppendCallback(() =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(74f, () =>
        {
            Transform elShape = Instantiate(PrefabManager.Instance.tableElementShapePrefab).transform;
            Transform hydrogenTransfrom = periodicTable.transform.GetChild(1).GetChild(0);
            elShape.position = hydrogenTransfrom.position;
            elShape.rotation = hydrogenTransfrom.rotation;
            elShape.SetParent(elShapes);
            elShape.name = hydrogenTransfrom.name;
            elShape.GetComponent<MeshRenderer>().material.DOColor(Color.yellow, 0.5f).Play();

            Atom atom = atomFactory.CreateAtom("Helium");

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToClassic2D();

            atom.transform.localPosition = Vector3.zero;
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            Core coreContainer = Instantiate(PrefabManager.Instance.heliumCorePrefab).GetComponent<Core>();
            atom.ChangeCore(coreContainer);
            //coreContainer.transform.SetParent(atom.transform.GetChild(0));
            coreContainer.transform.localPosition = Vector3.zero;
            coreContainer.transform.localRotation = Quaternion.identity;
            coreContainer.transform.localScale = new Vector3(1f, 1f, 1f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;


            TextMeshPro energyText = new GameObject("EnergyText").AddComponent<TextMeshPro>();
            energyText.transform.SetParent(atom.transform);
            energyText.transform.localScale = Vector3.one;
            energyText.transform.localPosition = new Vector3(-atom.orbitManager.orbits.Count - 1f, 1f, 0f);
            energyText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            energyText.text = "";

            energyText.fontSize = 12f;
            energyText.alignment = TextAlignmentOptions.Center;

            TextContainer etc = energyText.gameObject.GetComponent<TextContainer>();
            etc.width = 2f;
            etc.height = 2f;


            Sequence seq = DOTween.Sequence();
            seq.InsertCallback(3f, () =>
            {
                energyText.color = new Color(0, 0.5f, 1f);
                energyText.text = "-";

                foreach(Transform electron in atom.GetLastOrbitElectrons())
                {
                    electron.DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                    electron.GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                }
            });

            seq.InsertCallback(7f, () =>
            {
                energyText.color = Color.red;
                energyText.text = "+";
                foreach(Proton proton in atom.GetProtons())
                {
                    proton.GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                }
            });

            seq.InsertCallback(11f, () =>
            {
                energyText.color = Color.white;
                energyText.text = "0";
                foreach(Neutron neutron in atom.GetNeutrons())
                {
                    neutron.GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                }
            });

            seq.InsertCallback(15f, () =>
            {
                atom.shell.material.DOColor(Color.white, 1f).Play();
            });

            seq.InsertCallback(16f, () =>
            {
                energyText.color = Color.red;
                energyText.text = "+";
                atom.shell.material.DOColor(Color.red, 1f).Play();
            });

            seq.InsertCallback(17f, () =>
            {
                energyText.color = new Color(0, 0.5f, 1f);
                energyText.text = "-";
                atom.shell.material.DOColor(new Color(0, 0.5f, 1f), 1f).Play();
            });

            seq.InsertCallback(18f, () =>
            {
                elShape.GetComponent<MeshRenderer>().material.DOColor(new Color(1f, 1f, 0f, 0f), 1f);
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(elShape.gameObject, 1f);
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(94f, () =>
        {
            Atom atomSodium = atomFactory.CreateAtom("Sodium");

            atomSodium.transform.SetParent(atomsContainer);

            atomSodium.ChangeStateToClassic2D();
            atomSodium.ResetOrbits(1f);

            atomSodium.transform.localPosition = new Vector3(0.2f, 0f, 0f);
            atomSodium.transform.localEulerAngles = Vector3.zero;
            atomSodium.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomSodium.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomSodium.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomSodium.transform.forward);
            nameText.text = atomSodium.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Atom atomChlorine = atomFactory.CreateAtom("Chlorine", new int[] { 0, 0, 1 });

            atomChlorine.transform.SetParent(atomsContainer);

            atomChlorine.ChangeStateToClassic2D();
            atomChlorine.ResetOrbits(1f);

            atomChlorine.transform.localPosition = new Vector3(-0.2f, 0f, 0f); ;
            atomChlorine.transform.localEulerAngles = Vector3.zero;
            atomChlorine.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomChlorine.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomChlorine.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomChlorine.transform.forward);
            nameText.text = atomChlorine.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();

            seq.InsertCallback(5f, () =>
            {
                atomSodium.StopRotating(1.4f);
            });

            seq.InsertCallback(8f, () =>
            {
                atomChlorine.StopRotating(1.4f);
            });

            seq.InsertCallback(9f, () =>
            {
                atomSodium.GetLastOrbitElectrons()[0].DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                atomSodium.GetLastOrbitElectrons()[0].GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(20, LoopType.Yoyo).Play();

                atomChlorine.GetOrbitHoles(2)[0].DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                atomChlorine.GetOrbitHoles(2)[0].GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
            });

            seq.InsertCallback(13f, () =>
            {
                atomSodium.SetLastElectronToOtherAtom(atomChlorine, 1f);
            });

            seq.InsertCallback(14f, () =>
            {
                atomSodium.SetRotatingToInitial(2f);
                atomChlorine.SetRotatingToInitial(2f);
            });

            seq.InsertCallback(16f, () =>
            {
                atomSodium.shell.material.color = new Color(1f, 0f, 0f, 0);
                atomSodium.shell.material.DOColor(Color.red, 1f).Play();

                atomChlorine.shell.material.color = new Color(0f, 0.5f, 1f, 0f);
                atomChlorine.shell.material.DOColor(new Color(0f, 0.5f, 1f, 1f), 1f).Play();
            });

            seq.InsertCallback(17f, () =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);

                atomsContainer.GetChild(1).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(1).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(112f, () =>
        {
            Atom atom = atomFactory.CreateAtom("Beryllium");

            atom.transform.SetParent(atomsContainer);

            atom.ChangeStateToReal();

            atom.transform.localPosition = new Vector3(0f, 0f, 0f);
            atom.transform.localEulerAngles = Vector3.zero;
            atom.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atom.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.InsertCallback(4f, () =>
            {
                atomsContainer.GetChild(0).DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(117f, () =>
        {
            Atom atomHelium = atomFactory.CreateAtom("Helium");

            atomHelium.transform.SetParent(atomsContainer);

            atomHelium.ChangeStateToReal();

            atomHelium.transform.localPosition = new Vector3(0.6f, 0f, 0f);
            atomHelium.transform.localEulerAngles = Vector3.zero;
            atomHelium.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomHelium.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomHelium.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomHelium.transform.forward);
            nameText.text = atomHelium.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Atom atomCarbon = atomFactory.CreateAtom("Carbon");

            atomCarbon.transform.SetParent(atomsContainer);

            atomCarbon.ChangeStateToReal();

            atomCarbon.transform.localPosition = new Vector3(0.2f, 0f, 0f);
            atomCarbon.transform.localEulerAngles = Vector3.zero;
            atomCarbon.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomCarbon.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomCarbon.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomCarbon.transform.forward);
            nameText.text = atomCarbon.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Atom atomOxygen = atomFactory.CreateAtom("Oxygen");

            atomOxygen.transform.SetParent(atomsContainer);

            atomOxygen.ChangeStateToReal();

            atomOxygen.transform.localPosition = new Vector3(-0.2f, 0f, 0f);
            atomOxygen.transform.localEulerAngles = Vector3.zero;
            atomOxygen.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomOxygen.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomOxygen.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomOxygen.transform.forward);
            nameText.text = atomOxygen.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Atom atomNeon = atomFactory.CreateAtom("Neon");

            atomNeon.transform.SetParent(atomsContainer);

            atomNeon.ChangeStateToReal();

            atomNeon.transform.localPosition = new Vector3(-0.6f, 0f, 0f);
            atomNeon.transform.localEulerAngles = Vector3.zero;
            atomNeon.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomNeon.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomNeon.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomNeon.transform.forward);
            nameText.text = atomNeon.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.InsertCallback(4f, () =>
            {
                atomHelium.transform.DOScale(0f, 1f).Play();
                Destroy(atomHelium.gameObject, 1f);

                atomCarbon.transform.DOScale(0f, 1f).Play();
                Destroy(atomCarbon.gameObject, 1f);

                atomOxygen.transform.DOScale(0f, 1f).Play();
                Destroy(atomOxygen.gameObject, 1f);

                atomNeon.transform.DOScale(0f, 1f).Play();
                Destroy(atomNeon.gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(122f, () =>
        {
            Atom atomHydrogen = atomFactory.CreateAtom("Hydrogen");

            atomHydrogen.transform.SetParent(atomsContainer);

            atomHydrogen.ChangeStateToClassic3D();

            atomHydrogen.transform.localPosition = new Vector3(0f, 0f, 0f);
            atomHydrogen.transform.localEulerAngles = Vector3.zero;
            atomHydrogen.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomHydrogen.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomHydrogen.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomHydrogen.transform.forward);
            nameText.text = atomHydrogen.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            Sequence seq = DOTween.Sequence();
            seq.InsertCallback(4f, () =>
            {
                atomHydrogen.transform.DOScale(0f, 1f).Play();
                Destroy(atomHydrogen.gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(127f, () =>
        {
            Sequence seq = DOTween.Sequence();

            seq.InsertCallback(0f, () =>
            {
                Atom atomSilicon = atomFactory.CreateAtom("Silicon");

                atomSilicon.transform.SetParent(atomsContainer);

                atomSilicon.ChangeStateToReal();

                atomSilicon.transform.localPosition = new Vector3(0.4f, 0f, 0f);
                atomSilicon.transform.localEulerAngles = Vector3.zero;
                atomSilicon.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
                nameText.transform.SetParent(atomSilicon.transform);
                nameText.transform.localScale = Vector3.one;
                nameText.transform.localPosition = new Vector3(0f, -atomSilicon.orbitManager.orbits.Count - 1f, 0f);
                nameText.transform.localRotation = Quaternion.LookRotation(atomSilicon.transform.forward);
                nameText.text = atomSilicon.name;

                nameText.fontSize = 8f;
                nameText.alignment = TextAlignmentOptions.Center;

                TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
                tc.width = 6f;
                tc.height = 1f;
            });

            seq.InsertCallback(4f, () =>
            {
                Atom atomSulfur = atomFactory.CreateAtom("Sulfur");

                atomSulfur.transform.SetParent(atomsContainer);

                atomSulfur.ChangeStateToReal();

                atomSulfur.transform.localPosition = new Vector3(0f, 0f, 0f);
                atomSulfur.transform.localEulerAngles = Vector3.zero;
                atomSulfur.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
                nameText.transform.SetParent(atomSulfur.transform);
                nameText.transform.localScale = Vector3.one;
                nameText.transform.localPosition = new Vector3(0f, -atomSulfur.orbitManager.orbits.Count - 1f, 0f);
                nameText.transform.localRotation = Quaternion.LookRotation(atomSulfur.transform.forward);
                nameText.text = atomSulfur.name;

                nameText.fontSize = 8f;
                nameText.alignment = TextAlignmentOptions.Center;

                TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
                tc.width = 6f;
                tc.height = 1f;
            });

            seq.InsertCallback(8f, () =>
            {
                Atom atomArgon = atomFactory.CreateAtom("Argon");

                atomArgon.transform.SetParent(atomsContainer);

                atomArgon.ChangeStateToReal();

                atomArgon.transform.localPosition = new Vector3(-0.4f, 0f, 0f);
                atomArgon.transform.localEulerAngles = Vector3.zero;
                atomArgon.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
                nameText.transform.SetParent(atomArgon.transform);
                nameText.transform.localScale = Vector3.one;
                nameText.transform.localPosition = new Vector3(0f, -atomArgon.orbitManager.orbits.Count - 1f, 0f);
                nameText.transform.localRotation = Quaternion.LookRotation(atomArgon.transform.forward);
                nameText.text = atomArgon.name;

                nameText.fontSize = 8f;
                nameText.alignment = TextAlignmentOptions.Center;

                TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
                tc.width = 6f;
                tc.height = 1f;
            });

            seq.InsertCallback(12f, () =>
            {
                atomsContainer.GetChild(0).transform.DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(0).gameObject, 1f);

                atomsContainer.GetChild(1).transform.DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(1).gameObject, 1f);

                atomsContainer.GetChild(2).transform.DOScale(0f, 1f).Play();
                Destroy(atomsContainer.GetChild(2).gameObject, 1f);
            }).Play();
        });

        thirdSub.subSequence.InsertCallback(140f, () =>
        {
            List<List<float>> sodiumOverridedPositions = new List<List<float>>();
            sodiumOverridedPositions.Add(new List<float>() { 0f, 0.5f });
            sodiumOverridedPositions.Add(new List<float>() { 1f - 0.02f, 0.02f, 0.25f - 0.02f, 0.25f + 0.02f, 0.5f - 0.02f, 0.5f + 0.02f, 0.75f - 0.02f, 0.75f + 0.02f });
            sodiumOverridedPositions.Add(new List<float>() { 0.15f });

            List<List<float>> chlorineOverridedPositions = new List<List<float>>();
            chlorineOverridedPositions.Add(new List<float>() { 0f, 0.5f });
            chlorineOverridedPositions.Add(new List<float>() { 1f - 0.02f, 0.02f, 0.25f - 0.02f, 0.25f + 0.02f, 0.5f - 0.02f, 0.5f + 0.02f, 0.75f - 0.02f, 0.75f + 0.02f });
            chlorineOverridedPositions.Add(new List<float>() { 1f - 0.02f, 0.02f, 0.25f - 0.02f, 0.25f + 0.02f, 0.5f - 0.02f, 0.5f + 0.02f, 0.75f - 0.02f, 0.75f + 0.02f });

            Atom atomSodium = atomFactory.CreateAtom("Sodium", null, 0, sodiumOverridedPositions);

            atomSodium.transform.SetParent(atomsContainer);

            atomSodium.ChangeStateToClassic2D();
            atomSodium.StopRotating();
            atomSodium.ResetOrbits();

            atomSodium.transform.localPosition = new Vector3(0.2f, 0f, 0f);
            atomSodium.transform.localEulerAngles = Vector3.zero;
            atomSodium.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomSodium.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomSodium.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomSodium.transform.forward);
            nameText.text = atomSodium.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            atomSodium.orbitManager.orbits[2].electronOrbit.transform.localEulerAngles = new Vector3(0f, 0f, 160f);

            Atom atomChlorine = atomFactory.CreateAtom("Chlorine", new int[] { 0, 0, 1 }, 0, chlorineOverridedPositions);

            atomChlorine.transform.SetParent(atomsContainer);

            atomChlorine.ChangeStateToClassic2D();
            atomChlorine.StopRotating();
            atomChlorine.ResetOrbits();

            atomChlorine.transform.localPosition = new Vector3(-0.2f, 0f, 0f);
            atomChlorine.transform.localEulerAngles = Vector3.zero;
            atomChlorine.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atomChlorine.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -atomChlorine.orbitManager.orbits.Count - 1f, 0f);
            nameText.transform.localRotation = Quaternion.LookRotation(atomChlorine.transform.forward);
            nameText.text = atomChlorine.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            atomChlorine.orbitManager.orbits[2].electronOrbit.transform.localEulerAngles = new Vector3(0f, 0f, 180f);

            Sequence seq = DOTween.Sequence();

            seq.InsertCallback(4f, () =>
            {
                atomSodium.GetLastOrbitElectrons()[0].DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
                atomSodium.GetLastOrbitElectrons()[0].GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.2f).SetLoops(20, LoopType.Yoyo).Play();

                atomChlorine.GetOrbitHoles(2)[0].DOScale(0.6f, 0.2f).SetLoops(20, LoopType.Yoyo).Play();
            });

            seq.InsertCallback(8f, () =>
            {
                atomSodium.SetLastElectronToOtherAtom(atomChlorine, 1f);
            });

            seq.InsertCallback(9f, () =>
            {
                atomChlorine.SetRotatingToInitial(4);
                atomSodium.SetRotatingToInitial(4);

                nameText = new GameObject("+1").AddComponent<TextMeshPro>();
                nameText.transform.SetParent(atomSodium.transform);
                nameText.transform.localScale = Vector3.one;
                nameText.transform.localPosition = new Vector3(0f, atomChlorine.orbitManager.orbits.Count + 1f, 0f);
                nameText.transform.localRotation = Quaternion.LookRotation(atomChlorine.transform.forward);
                nameText.text = "+1";

                nameText.fontSize = 8f;
                nameText.alignment = TextAlignmentOptions.Center;

                tc = nameText.gameObject.GetComponent<TextContainer>();
                tc.width = 6f;
                tc.height = 1f;

                nameText = new GameObject("-1").AddComponent<TextMeshPro>();
                nameText.transform.SetParent(atomChlorine.transform);
                nameText.transform.localScale = Vector3.one;
                nameText.transform.localPosition = new Vector3(0f, atomChlorine.orbitManager.orbits.Count + 1f, 0f);
                nameText.transform.localRotation = Quaternion.LookRotation(atomChlorine.transform.forward);
                nameText.text = "-1";

                nameText.fontSize = 8f;
                nameText.alignment = TextAlignmentOptions.Center;

                tc = nameText.gameObject.GetComponent<TextContainer>();
                tc.width = 6f;
                tc.height = 1f;

                atomSodium.shell.material.color = new Color(1f, 0f, 0f, 0f);
                atomSodium.shell.material.DOColor(Color.red, 1f).Play();

                atomChlorine.shell.material.color = new Color(0f, 0.5f, 1f, 0f);
                atomChlorine.shell.material.DOColor(new Color(0f, 0.5f, 1f, 1f), 1f).Play();

            });

            seq.InsertCallback(13f, () =>
            {
                atomSodium.transform.DOScale(0f, 1f).Play();
                Destroy(atomSodium.gameObject, 1f);

                atomChlorine.transform.DOScale(0f, 1f).Play();
                Destroy(atomChlorine.gameObject, 1f);
            }).Play();
        });

        //thirdSub.subSequence.InsertCallback(154f, () =>
        //{

        //});

        //thirdSub.subSequence.InsertCallback(74f, () =>
        //{
        //    Destroy(atomsContainer.gameObject);
        //    Destroy(elShapes.gameObject);
        //    Destroy(button.gameObject);

        //    professor.GetTarget().position = Camera.main.transform.TransformPoint(0f, 1f, -2f);
        //    professor.GetTarget().LookAt(Camera.main.transform.position);

        //    Destroy(professor.gameObject, 1f);

        //    PlayerManager.Instance.ChangeStateToDefault();
        //});

        #endregion

        baseGIDSequence.AddSubSequence(firstSub);
        baseGIDSequence.AddSubSequence(secondSub);
        baseGIDSequence.AddSubSequence(thirdSub);

        SequenceManager.Instance.AddSequence("BaseGID", baseGIDSequence);
    }
}
