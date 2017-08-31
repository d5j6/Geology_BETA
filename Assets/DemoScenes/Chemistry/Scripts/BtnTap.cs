using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;


public class BtnTap : MonoBehaviour, IInteractive
{
    private AudioSource audioSource;

    [SerializeField]
    private string chapterName;

    [SerializeField]
    private List<ActionType> _allowedActions;

    private Text _text;

    private Tween _colorTween;

    [SerializeField]
    private Color _selfColor;

    [SerializeField]
    private Color _highlightColor = Color.white;

    [SerializeField]
    private float _duration = 1f;

    private bool isActiveBtn = false;
    // Use this for initialization
    void Start()
    {
        _text = GetComponentInChildren<Text>();
        _text.color = Color.white;

        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource = AudioManager.Instance.AudioSourceSettings(audioSource);
    }

    // Update is called once per frame
    void Update() { }

    public List<ActionType> GetAllowedActions()
    {
        return _allowedActions;
    }

    public void OnGestureTap()
    {
        if (PlayerManager.Instance.Strategy == InputStrategyFacade.Strategies.Default)
        {
            audioSource.Play();
            RunAnumation();
        }
    }

    public void RunAnumation(bool fromSharing = false)
    {
        //if (!CutsceneManager.Instance.IsStop)
        switch (gameObject.name)
        {
            case "Demo":
                CutsceneManager.Instance.PlaySectionNow(playDemo: true);
                // CutsceneManager.Instance.PlaySectionNow(playDemo: true);
                break;
            case "Stop":
                CutsceneManager.Instance.SkipCutscene();
                CutsceneManager.Instance.StopCutscene();
                break;
            default:
                CutsceneManager.Instance.PlaySectionNow(sectionName: chapterName);
                break;
        }

        if (!fromSharing)
            SV_Sharing.Instance.SendInt(GetComponent<IDHolder>().ID, "run_animation");
    }

    public void OnGazeEnter()
    {
        if (PlayerManager.Instance.Strategy == InputStrategyFacade.Strategies.Default &&
            !isActiveBtn)
            HighlightMenuItem();
    }


    public void OnGazeLeave()
    {
        if (PlayerManager.Instance.Strategy == InputStrategyFacade.Strategies.Default &&
            !isActiveBtn)
            DehighlightMenuItem();
    }

    public void DeactivateButton(string chaptersName)
    {
        _text.color = Color.white;
        isActiveBtn = false;
    }

    public void ActivateButton(string chaptersName)
    {
        _text.color = Color.white;
        if (chaptersName == _text.text)
        {
            _text.color = _highlightColor;
            isActiveBtn = true;

        }
    }

    public void HighlightMenuItem(bool fromSharing = false)
    {
        ChangeTextColor(_highlightColor);
        //SV
        if (!fromSharing)
            SV_Sharing.Instance.SendInt(GetComponent<IDHolder>().ID, "highlight_menu_item");
    }

    public void DehighlightMenuItem(bool fromSharing = false)
    {
        ChangeTextColor(_selfColor);
        //SV
        if (!fromSharing)
            SV_Sharing.Instance.SendInt(GetComponent<IDHolder>().ID, "dehighlight_menu_item");
    }

    public void ChangeTextColor(Color color)
    {
        _colorTween.Kill();
        _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, color, _duration).Play();
    }

    public bool TryToDrag() { return false; }
    public bool TryToResize() { return false; }
    public void StopDrag() { }
}


//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using DG.Tweening;
//using UnityEngine.UI;


//public class BtnTap : MonoBehaviour, IInteractive
//{
//    [SerializeField]
//    private string chapterName;

//    [SerializeField]
//    private List<ActionType> _allowedActions;

//    private Text _text;

//    private Tween _colorTween;

//    [SerializeField]
//    private Color _selfColor;

//    [SerializeField]
//    private Color _highlightColor = Color.white;

//    [SerializeField]
//    private float _duration = 1f;

//    private bool isActiveBtn = false;
//    // Use this for initialization
//    void Start()
//    {
//        _text = GetComponentInChildren<Text>();
//        _text.color = Color.white;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public List<ActionType> GetAllowedActions()
//    {
//        return _allowedActions;
//    }

//    public void OnGestureTap()
//    {
//        //if ()
//        //{

//        //}
//        if (!CutsceneManager.Instance.isStop)
//        {
//            CutsceneManager.Instance.PlaySectionNow(sectionName: chapterName, btnTap: this);
//        }        
//    }

//    public void OnGazeEnter()
//    {
//        Debug.Log("Enter");
//        if (!isActiveBtn)
//        {
//            _colorTween.Kill();
//            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _highlightColor, _duration).Play();
//        }
//    }

//    public void OnGazeLeave()
//    {
//        if (!isActiveBtn)
//        {
//            _colorTween.Kill();
//            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
//        }   
//    }

//    public void DeactivateButton(string chaptersName)
//    {

//        _text.color = Color.white;
//        isActiveBtn = false;
////        else
////        {
////            _colorTween.Kill();
////            _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
////			_colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, Color.white, 1f).Play();
////        }
//    }

//	public void ActivateButton(string chaptersName)
//    {
//        _text.color = Color.white;
//        if (chaptersName == _text.text)
//		{
//			_text.color = _highlightColor;
//            isActiveBtn = true;

//		}
////
////        _colorTween.Kill();
////        _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, _selfColor, _duration).Play();
////        _colorTween = DOTween.To(() => { return _text.color; }, (c) => { _text.color = c; }, Color.white, 2f).Play();
//    }

//    public bool TryToDrag() { return false; }
//    public bool TryToResize() { return false; }
//    public void StopDrag() { }
//}


