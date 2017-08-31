using UnityEngine;

public class AddToSV_Root : MonoBehaviour
{
	private void Awake ()
    {
        MainAction();
    }

    private void Update()
    {
        if (transform.parent == null)
        {
            MainAction();
        }
    }

    private void MainAction()
    {
        SV_Root.Instance.AddToRoot(gameObject);
    }
}
