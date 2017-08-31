using UnityEngine;
//using UnityEditor;

public class SolarWindController : MonoBehaviour
{
    ParticleSystem _particleSystem;
    public GameObject MagneticFieldLayer1;
    public GameObject MagneticFieldLayer2;
    public GameObject MagneticFieldLayer3;
    public GameObject MagneticFieldLayer4;
    public GameObject CanvasHolder;

    [HideInInspector]
    public Material SolarWindMaterial;
    Material MagneticFieldLayer1Mat;
    Material MagneticFieldLayer2Mat;
    Material MagneticFieldLayer3Mat;
    Material MagneticFieldLayer4Mat;

    private bool _positionChanged = true;
    private Vector3 directionToTarget;
    private Vector3 _initialRotation = new Vector3(0f, 180f, 0f);
    private Vector3 _canvasHolderInitialRotation = new Vector3(0f, 270f, 0f);

    public void SetPosition(Vector3 newPos)
    {
        transform.localPosition = newPos;
        MagneticFieldLayer1Mat.SetFloat("_XSolarWindDirection", newPos.x);
        MagneticFieldLayer2Mat.SetFloat("_XSolarWindDirection", newPos.x);
        MagneticFieldLayer3Mat.SetFloat("_XSolarWindDirection", newPos.x);
        MagneticFieldLayer4Mat.SetFloat("_XSolarWindDirection", newPos.x);
        MagneticFieldLayer1Mat.SetFloat("_ZSolarWindDirection", newPos.z);
        MagneticFieldLayer2Mat.SetFloat("_ZSolarWindDirection", newPos.z);
        MagneticFieldLayer3Mat.SetFloat("_ZSolarWindDirection", newPos.z);
        MagneticFieldLayer4Mat.SetFloat("_ZSolarWindDirection", newPos.z);

        _positionChanged = true;
    }

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        /*SerializedObject so = new SerializedObject(_particleSystem);
        so.FindProperty("ShapeModule.angle").floatValue = 0f;
        so.ApplyModifiedProperties();*/

        MagneticFieldLayer1Mat = MagneticFieldLayer1.GetComponent<Renderer>().material;
        MagneticFieldLayer2Mat = MagneticFieldLayer2.GetComponent<Renderer>().material;
        MagneticFieldLayer3Mat = MagneticFieldLayer3.GetComponent<Renderer>().material;
        MagneticFieldLayer4Mat = MagneticFieldLayer4.GetComponent<Renderer>().material;
        SolarWindMaterial = _particleSystem.GetComponent<Renderer>().material;
        /*SerializedProperty it = _particleSystem.GetIterator();
        while (it.Next(true))
            Debug.Log(it.propertyPath);*/
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (_positionChanged)
        {
            _positionChanged = false;
            directionToTarget = -gameObject.transform.localPosition;
            directionToTarget.y = 0;
            /*Vector3 eulerRot = Quaternion.LookRotation(-directionToTarget) * (new Vector3(0f, 0f, 0f));
            eulerRot.x = 0f;
            eulerRot.z = 0f;*/
            gameObject.transform.localRotation = Quaternion.LookRotation(-directionToTarget) * Quaternion.Euler(_initialRotation);
            CanvasHolder.transform.localRotation = Quaternion.LookRotation(-directionToTarget) * Quaternion.Euler(_canvasHolderInitialRotation);
        }
    }
}