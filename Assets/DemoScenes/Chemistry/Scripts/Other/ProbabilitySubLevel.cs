using UnityEngine;
using System.Collections;

public class ProbabilitySubLevel : MonoBehaviour
{
    private AtomFormula _atomFormula;

    private bool _isInitialized;

    public void Initialize(AtomFormula formula)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized = true;

        _atomFormula = formula;

        switch (_atomFormula.form)
        {
            case ProbabilityForm.S:
                if (_atomFormula.level == 1 || AtomFactory.atomInfo.name == "Beryllium") GenerateSSubLevel();
                break;
            case ProbabilityForm.P:
                GeneratePSubLevel();
                break;
            case ProbabilityForm.D:
                GenerateDSubLevel();
                break;
            case ProbabilityForm.F:
                GenerateFSubLevel();
                break;
            default:
                break;
        }
    }

    private void GenerateSSubLevel()
    {
        GameObject _realForm;

        _realForm = Instantiate(PrefabManager.Instance.sSubLevelPrefab);
        _realForm.transform.SetParent(transform);
        _realForm.transform.localPosition = Vector3.zero;
        _realForm.transform.localRotation = Quaternion.identity;

        float probabilityRadius = _atomFormula.level * 2f;
        _realForm.transform.localScale = new Vector3(probabilityRadius, probabilityRadius, probabilityRadius);

        _realForm.GetComponent<MeshRenderer>().material.renderQueue = 3000 + (_atomFormula.level * 2);
    }

    private void GeneratePSubLevel()
    {

        int formCount = _atomFormula.electronsCount / 2;
        formCount = (_atomFormula.electronsCount % 2 == 0) ? formCount : formCount + 1;

        Vector3[] rotations = new Vector3[] { new Vector3(0f, 0f, 0f), new Vector3(0f, 90f, 0f), new Vector3(0f, 0f, 90f) };

        for (int i = 0; i < formCount; i++)
        {
            GameObject _realForm;
            _realForm = Instantiate(PrefabManager.Instance.pSubLevelPrefab);
            _realForm.transform.SetParent(transform);
            _realForm.transform.localPosition = Vector3.zero;
            _realForm.transform.localRotation = Quaternion.Euler(rotations[i]);

            float probabilityRadius = _atomFormula.level * 2f;
            _realForm.transform.localScale = new Vector3(probabilityRadius, probabilityRadius, probabilityRadius);

            foreach (MeshRenderer mesh in _realForm.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material.renderQueue = 3000 + (_atomFormula.level * 2) - 1;
            }
        }
    }

    private void GenerateDSubLevel()
    {
        int formCount = _atomFormula.electronsCount / 2;
        formCount = (_atomFormula.electronsCount % 2 == 0) ? formCount : formCount + 1;

        Vector3[] rotations = new Vector3[] { new Vector3(0f, 0f, 0f), new Vector3(0f, 90f, 0f), new Vector3(0f, 0f, 90f) };

        for (int i = 0; i < formCount; i++)
        {
            GameObject _realForm;
            _realForm = Instantiate(PrefabManager.Instance.dSubLevelPrefab);
            _realForm.transform.SetParent(transform);
            _realForm.transform.localPosition = Vector3.zero;
            _realForm.transform.localRotation = Quaternion.Euler(rotations[i]);

            float probabilityRadius = _atomFormula.level * 0.6f;
            _realForm.transform.localScale = new Vector3(probabilityRadius, probabilityRadius, probabilityRadius);

            foreach (MeshRenderer mesh in _realForm.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material.renderQueue = 3000 + (_atomFormula.level * 2) - 1;
            }
        }
    }

    private void GenerateFSubLevel()
    {
        GameObject _realForm;
        _realForm = Instantiate(PrefabManager.Instance.fSubLevelPrefub);
        _realForm.transform.SetParent(transform);
        _realForm.transform.localPosition = Vector3.zero;

        float probabilityRadius = _atomFormula.level * 0.4f;
        _realForm.transform.localScale = new Vector3(probabilityRadius, probabilityRadius, probabilityRadius);

        foreach (MeshRenderer mesh in _realForm.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.renderQueue = 3000 + (_atomFormula.level * 2) - 1;
        }
    }
}
