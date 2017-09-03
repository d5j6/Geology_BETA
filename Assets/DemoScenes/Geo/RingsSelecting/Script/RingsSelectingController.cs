using UnityEngine;

public class RingsSelectingController : MonoBehaviour {

    private bool __enabled;
    private bool _enabled
    {
        set
        {
            if (value)
            {
                ring1.SetActive(true);
                ring2.SetActive(true);
                ring3.SetActive(true);
            }
            else
            {
                ring1.SetActive(false);
                ring2.SetActive(false);
                ring3.SetActive(false);
            }
            __enabled = value;
        }
        get
        {
            return __enabled;
        }
    }
    private float alpha = 0;
    private Vector3 rotationVector1 = new Vector3(-0.59f, 0.29f, 0.60f);
    private Vector3 rotationVector2 = new Vector3(0.23f, 0.53f, -0.31f);
    private Vector3 rotationVector3 = new Vector3(0.61f, -0.44f, -0.43f);
    private float speed = 0;
    private float speedMultiplier = 180f;

    public GameObject ring1;
    public GameObject ring2;
    public GameObject ring3;
    private Material mat1;
    private Material mat2;
    private Material mat3;

    private void Awake()
    {
        mat1 = ring1.GetComponent<Renderer>().material;
        mat2 = ring2.GetComponent<Renderer>().material;
        mat3 = ring3.GetComponent<Renderer>().material;

        HideImmediately();
    }

    public void ShowImmediately()
    {
        LeanTween.cancel(gameObject);
        Color c = Color.white;
        alpha = 1;
        speed = 1;
        c.a = 1;
        _enabled = true;
        mat1.color = c;
        mat2.color = c;
        mat3.color = c;
    }

    public void ShowVoid()
    {
        //Debug.Log("Trying to showVoid my Rings");
        _enabled = true;
        LeanTween.cancel(gameObject);
        Color c = Color.white;
        LeanTween.value(gameObject, alpha, 1, 1f).setOnUpdate((float val) =>
        {
            alpha = val;
            c.a = alpha;
            if (mat1 != null)
            {
                mat1.color = c;
                mat2.color = c;
                mat3.color = c;
            }
            speed = val;
        });
    }

    public void Show(System.Action callback = null)
    {
        _enabled = true;
        LeanTween.cancel(gameObject);
        Color c = Color.white;
        LeanTween.value(gameObject, alpha, 1, 1f).setOnUpdate((float val) =>
        {
            alpha = val;
            c.a = alpha;
            mat1.color = c;
            mat2.color = c;
            mat3.color = c;
            speed = val;
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void HideVoid()
    {
        LeanTween.cancel(gameObject);
        Color c = Color.white;
        LeanTween.value(gameObject, alpha, 0, 1f).setOnUpdate((float val) =>
        {
            alpha = val;
            c.a = alpha;
            mat1.color = c;
            mat2.color = c;
            mat3.color = c;
            speed = val;
        }).setOnComplete(() =>
        {
            _enabled = false;
        });
    }

    public void Hide(System.Action callback = null)
    {
        LeanTween.cancel(gameObject);
        Color c = Color.white;
        //Debug.Log("name: " + gameObject.name + " - " + gameObject.transform.parent.gameObject.name + " - " + gameObject.transform.parent.parent.gameObject.name);
        LeanTween.value(gameObject, alpha, 0, 1f).setOnUpdate((float val) =>
        {
            alpha = val;
            c.a = alpha;
            mat1.color = c;
            mat2.color = c;
            mat3.color = c;
            speed = val;
        }).setOnComplete(() =>
        {
            _enabled = false;
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void HideImmediately()
    {
        LeanTween.cancel(gameObject);
        Color c = Color.white;
        alpha = 0;
        speed = 0;
        c.a = 0;
        _enabled = false;
        mat1.color = c;
        mat2.color = c;
        mat3.color = c;
    }

    private void Update()
    {
        if (_enabled)
        {
            ring1.transform.Rotate(rotationVector1 * speed * Time.deltaTime * speedMultiplier);
            ring2.transform.Rotate(rotationVector2 * speed * Time.deltaTime * speedMultiplier);
            ring3.transform.Rotate(rotationVector3 * speed * Time.deltaTime * speedMultiplier);
        }
    }
}
