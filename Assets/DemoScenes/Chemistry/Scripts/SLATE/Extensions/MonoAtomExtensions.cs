using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Gemeleon.Extensions
{
    public static class MonoAtomExtensions
    {
        public static void CreateAtomsAndParentTo(this MonoAtomFactory factory, string[] names, float size, AtomState state, Transform parent, Vector3 localStartPos, Vector3 cycleOffset)
        {
            int loop = 0;
   

            foreach(string name in names)
            {
                Atom atom = factory.Create(name);
                atom.transform.SetParent(parent);
              
                atom.transform.localPosition = localStartPos + (cycleOffset * loop++);

                switch (AtomFactory.atomInfo.name)
                {
                    case "Potassium":
                        atom.transform.localPosition = localStartPos - new Vector3(cycleOffset.x, .5f, cycleOffset.z);
                        break;
                    case "Rubidium":
                        atom.transform.localPosition = localStartPos - new Vector3(cycleOffset.x, .75f, cycleOffset.z);
                        break;
                    case "Caesium":
                        atom.transform.localPosition = localStartPos - new Vector3(cycleOffset.x, 1.02f, cycleOffset.z);
                        break;
                    case "Francium":
                        atom.transform.localPosition = localStartPos - new Vector3(cycleOffset.x, 1.33f, cycleOffset.z);
                        break;
                }
                //atom.transform.rotation = Quaternion.LookRotation(atom.transform.position - Camera.main.transform.position, Vector3.up);
                atom.transform.localRotation = Quaternion.identity;

                switch(state)
                {
                    case AtomState.None:
                        break;
                    case AtomState.Classic2D:
                        atom.ChangeStateToClassic2D();
                        break;
                    case AtomState.Classic3D:
                        atom.ChangeStateToClassic3D();
                        break;
                    case AtomState.Real:
                        atom.ChangeStateToReal();
                        break;
                }

                TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
                nameText.transform.SetParent(atom.transform);
                nameText.transform.localScale = Vector3.one;
                nameText.transform.localPosition = new Vector3(0f, -(atom.orbitManager.orbits.Count + 1f), 0f);

                switch (atom.name)
                {
                    case "Lutetium":
                    nameText.transform.localPosition = new Vector3(0f, -(atom.orbitManager.orbits.Count - .5f), 0f);
                        break;
                }

                nameText.transform.rotation = Quaternion.LookRotation(-atom.transform.forward);
                nameText.text = atom.name;

                nameText.fontSize = 8f;
                nameText.alignment = TextAlignmentOptions.Center;

                TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
                tc.width = 6f;
                tc.height = 1f;

                atom.transform.localScale = new Vector3(size, size, size);
            }
        }

        public static void CreateAtomWithHolesAndParentTo(this MonoAtomFactory factory, string name, float size, AtomState state, int[] holes, Transform parent, Vector3 localStartPos)
        {
            Atom atom = factory.Create(name, holes);
            atom.transform.SetParent(parent);
            atom.transform.localPosition = localStartPos;
            //atom.transform.rotation = Quaternion.LookRotation(atom.transform.position - Camera.main.transform.position, Vector3.up);
            atom.transform.localRotation = Quaternion.identity;

            switch(state)
            {
                case AtomState.None:
                    break;
                case AtomState.Classic2D:
                    atom.ChangeStateToClassic2D();
                    break;
                case AtomState.Classic3D:
                    atom.ChangeStateToClassic3D();
                    break;
                case AtomState.Real:
                    atom.ChangeStateToReal();
                    break;
            }

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -(atom.orbitManager.orbits.Count + 1f), 0f);
            nameText.transform.rotation = Quaternion.LookRotation(-atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            atom.transform.localScale = new Vector3(size, size, size);
        }

        public static void CreateAtomWithReplacedCore(this MonoAtomFactory factory, string name, float size, Core corePrefab, AtomState state, Transform parent, Vector3 localStartPos)
        {
            Atom atom = factory.Create(name);
            atom.transform.SetParent(parent);
            atom.transform.localPosition = localStartPos;
            //atom.transform.rotation = Quaternion.LookRotation(atom.transform.position - Camera.main.transform.position, Vector3.up);
            atom.transform.localRotation = Quaternion.identity;

            Core coreContainer = MonoBehaviour.Instantiate(corePrefab).GetComponent<Core>();
            atom.ChangeCore(coreContainer);
            coreContainer.transform.localPosition = Vector3.zero;
            coreContainer.transform.localRotation = Quaternion.identity;
            coreContainer.transform.localScale = new Vector3(1f, 1f, 1f);

            switch(state)
            {
                case AtomState.None:
                    break;
                case AtomState.Classic2D:
                    atom.ChangeStateToClassic2D();
                    break;
                case AtomState.Classic3D:
                    atom.ChangeStateToClassic3D();
                    break;
                case AtomState.Real:
                    atom.ChangeStateToReal();
                    break;
            }

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -(atom.orbitManager.orbits.Count + 1f), 0f);
            nameText.transform.rotation = Quaternion.LookRotation(-atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            atom.transform.localScale = new Vector3(size, size, size);
        }

        public static void CreateAtomWithOverridesAndParentTo(this MonoAtomFactory factory, string name, float size, AtomState state, int[] holes, int holesOffset, List<List<float>> overrides, Transform parent, Vector3 localStartPos)
        {
            Atom atom = factory.Create(name, holes, holesOffset, overrides);
            atom.transform.SetParent(parent);
            atom.transform.localPosition = localStartPos;
            //atom.transform.rotation = Quaternion.LookRotation(atom.transform.position - Camera.main.transform.position, Vector3.up);
            atom.transform.localRotation = Quaternion.identity;

            switch(state)
            {
                case AtomState.None:
                    break;
                case AtomState.Classic2D:
                    atom.ChangeStateToClassic2D();
                    break;
                case AtomState.Classic3D:
                    atom.ChangeStateToClassic3D();
                    break;
                case AtomState.Real:
                    atom.ChangeStateToReal();
                    break;
            }

            atom.StopRotating();
            atom.ResetOrbits();

            TextMeshPro nameText = new GameObject("NameText").AddComponent<TextMeshPro>();
            nameText.transform.SetParent(atom.transform);
            nameText.transform.localScale = Vector3.one;
            nameText.transform.localPosition = new Vector3(0f, -(atom.orbitManager.orbits.Count + 1f), 0f);
            nameText.transform.rotation = Quaternion.LookRotation(-atom.transform.forward);
            nameText.text = atom.name;

            nameText.fontSize = 8f;
            nameText.alignment = TextAlignmentOptions.Center;

            TextContainer tc = nameText.gameObject.GetComponent<TextContainer>();
            tc.width = 6f;
            tc.height = 1f;

            atom.transform.localScale = new Vector3(size, size, size);
        }
    }
}
