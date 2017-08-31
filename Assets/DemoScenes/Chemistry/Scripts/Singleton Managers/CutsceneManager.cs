using System;
using UnityEngine;
using System.Collections;
using Slate;
using System.Collections.Generic;

public class CutsceneManager : Singleton<CutsceneManager>
{
    #region Private Fields

    [SerializeField]
    private Cutscene baseGID1;
    [SerializeField]
    private Cutscene baseGID2;
    [SerializeField]
    private Cutscene baseGIDEnd;
    [SerializeField]
    private Cutscene utilCutscene;

    private Cutscene currentCutscene;

    private string nextChapterForPlay;
    private string currentSectionName;

    private string[] allSections;
    private string[] baseGid1Sections;
    private string[] baseGid2Sections;
    private string[] baseGidEndSections;

    private Dictionary<string, Cutscene> chaptersDictionary;

    private bool isSkipped = false;

    #endregion

    #region Public Properties

    public static bool IsPlaying { get; set; }
    public bool BaseGid1SectionFound { get; set; }
    public bool IsStop { get; set; }

    #endregion

    private void Test()
    {
        if (!isSkipped)
        {
            Debug.Log("SecondCutscene");
            baseGID2.Play(() => { Debug.Log("BASE 2"); baseGIDEnd.Play(() => { ActivateButton(); SkipCutscene(); }); currentCutscene = baseGIDEnd; IsPlaying = true; }); baseGID2.JumpToSection("Groups and periods"); currentCutscene = baseGID2;
        }
    }

    private void InitializeChapters()
    {
        chaptersDictionary = new Dictionary<string, Cutscene>();

        foreach (string chapterName in baseGID1.GetSectionNames())
            chaptersDictionary.Add(chapterName, baseGID1);

        foreach (string chapterName in baseGID2.GetSectionNames())
            chaptersDictionary.Add(chapterName, baseGID2);

        foreach (string chapterName in baseGIDEnd.GetSectionNames())
            chaptersDictionary.Add(chapterName, baseGIDEnd);
    }

    void Start()
    {
        InitializeChapters();

        Cutscene.OnCutsceneStopped += Cutscene_OnCutsceneStopped;
        baseGid1Sections = baseGID1.GetSectionNames();
        baseGid2Sections = baseGID2.GetSectionNames();
        baseGidEndSections = baseGIDEnd.GetSectionNames();

        allSections = new string[baseGid1Sections.Length + baseGid2Sections.Length + baseGidEndSections.Length];

        for (int i = 0; i < baseGid1Sections.Length; i++)
        {
            allSections[i] = baseGid1Sections[i];
        }

        for (int i = 0; i < baseGid2Sections.Length; i++)
        {
            allSections[i + baseGid1Sections.Length] = baseGid2Sections[i];
        }

        for (int i = 0; i < baseGidEndSections.Length; i++)
        {
            allSections[i + baseGid2Sections.Length + baseGid1Sections.Length] = baseGidEndSections[i];
        }
    }

    private void Cutscene_OnCutsceneStopped(Cutscene obj)
    {
        IsPlaying = false;

        if (obj.name == "BaseGIDCutsceneEnd")
        {
            baseGIDEnd.RewindNoUndo();
            baseGID2.RewindNoUndo();
            baseGID1.RewindNoUndo();
        }
    }

    public void StopCutscene()
    {
        IsStop = true;

        if (currentCutscene != null)
        {
            currentCutscene.Stop();
            return;
        }

        if (baseGID1 != null && baseGID1.isActive)
        {
            baseGID1.Stop(Cutscene.StopMode.Skip);
            return;
        }

        if (baseGID2 != null && baseGID2.isActive)
        {
            baseGID2.Stop(Cutscene.StopMode.Rewind);
            return;
        }

        if (baseGIDEnd != null && baseGIDEnd.isActive)
        {
            baseGIDEnd.Stop(Cutscene.StopMode.Skip);
            return;
        }
    }

    public void PlayBaseGIDCutscene1()
    {
        isSkipped = false;
        currentCutscene = baseGID1;
        baseGID1.Play(() => {  Test(); });
        //DeactivateButton();
    }

    public void PlayBaseGIDCutscene2(string sectionName = "")
    {
        IsPlaying = true;

        baseGID2.Play();
        if (!string.IsNullOrEmpty(sectionName))
            baseGID2.JumpToSection(sectionName);

        currentCutscene = baseGID2;
    }

    public void SkipCutscene(string sectionName = "")
    {
        isSkipped = true;
        IsPlaying = true;

        DeactivateButton(sectionName);

        if (baseGID2 != null)
        {
            baseGID2.Stop(Cutscene.StopMode.SkipRewindNoUndo);
        }

        if (!IsPlaying)
        {
            return;
        }

        if (currentCutscene != null)
        {
            if (currentCutscene == baseGID1)
            {
                currentCutscene.Stop(Cutscene.StopMode.Skip);
            }
            else
            {
                currentCutscene.Stop(Cutscene.StopMode.Rewind);
            }

            currentCutscene.Stop();
        }

        Destroy(GameObject.Find("_AudioSources"));
        ActivateButton();
    }

    private void Update()
    {
        //    if (Input.GetKeyDown(KeyCode.L))
        //    {
        //        //PlaySectionNow("Test1");
        //        NextChapter();
        //    }

        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        //PlaySectionNow("Test1");
        //        PreviewsChapter();
        //    }
    }

    public void PlaySectionNow(bool playDemo = false, string sectionName = "")
    {
        if (sectionName == "")
            sectionName = allSections[0];

        Debug.Log("!" + sectionName);

        if (chaptersDictionary.ContainsKey(sectionName))
        {
            Debug.Log("!SectionName key was found: " + sectionName);
            PlayingControll(sectionName, playDemo);
        }
    }

    private void PlayingControll(string sectionName = "", bool playDemo = false)
    {
        Cutscene cutscene = chaptersDictionary[sectionName];

        if (cutscene == baseGID2)
        {
            if (currentCutscene != baseGID2)
            {
                SkipFirstCutSceneForInit();
                currentCutscene = baseGID2;
            }
        }
        else
            currentCutscene = baseGID1;

        IsStop = false;
        currentSectionName = sectionName;

        StartCoroutine(ChangeChapterDelay(sectionName, playDemo));
    }

    public void ChangeCutsceneState()
    {
        if (currentCutscene != null)
        {
            currentCutscene.Stop();
        }
    }

    private void SkipFirstCutSceneForInit()
    {
        baseGID1.Play();
        baseGID1.Stop(Cutscene.StopMode.Skip);
    }

    public void ActivateButton(string sectionName = "", bool fromSharing = false)
    {
        IsPlaying = true;

        foreach (BtnTap item in StartScenario.chaptersBtn)
        {
            item.ActivateButton(sectionName);
        }

        if (!fromSharing)
            SV_Sharing.Instance.SendBool(true, "activate_menu_items");
    }

    public void DeactivateButton(string chapterName = "", bool fromSharing = false)
    {
        IsPlaying = false;

        foreach (BtnTap item in StartScenario.chaptersBtn)
        {
            item.DeactivateButton(chapterName);
        }


        if (!fromSharing)
            SV_Sharing.Instance.SendBool(true, "deactivate_menu_items");
    }

    public void NextChapter()
    {
        nextChapterForPlay = "";

        int indexCurrentSection = Array.IndexOf(allSections, currentSectionName);

        if (indexCurrentSection < allSections.Length)
            nextChapterForPlay = allSections[indexCurrentSection + 1];

        StartCoroutine(ChapterDelay());
    }

    public void PreviewsChapter()
    {
        nextChapterForPlay = "";

        int indexCurrentSection = Array.IndexOf(allSections, currentSectionName);

        if (indexCurrentSection > 0)
            nextChapterForPlay = allSections[indexCurrentSection - 1];

        StartCoroutine(ChapterDelay());
    }

    public IEnumerator ChapterDelay()
    {
        SkipCutscene();
        yield return new WaitForSeconds(1f);

        if (nextChapterForPlay != String.Empty)
        {
            PlaySectionNow(sectionName: nextChapterForPlay);
        }
    }

    public IEnumerator ChangeChapterDelay(string sectionName, bool isAllSections = false)
    {

        if (isAllSections)
        {
            SkipCutscene(sectionName);

            yield return new WaitForSeconds(.5f);

            bool canPlay = true;
            Debug.Log("All sections true");
            currentCutscene = baseGID1;

            foreach (var item in allSections)
            {
                if (canPlay)
                {
                    canPlay = false;
                    isSkipped = false;

                    currentCutscene.PlaySection(item, Cutscene.WrapMode.Once, () => StartPlayingAll(ref canPlay, item));
                    ActivateButton(item);
                }

                yield return new WaitUntil(() => canPlay);
            }
        }
        else
        {
            if (!IsStop)
            {
                SkipCutscene(sectionName);
                yield return new WaitForSeconds(.2f);
                currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => DeactivateButton());
                ActivateButton(sectionName);
            }
        }
    }

    private void StartPlayingAll(ref bool canPlay, string sectionName)
    {
        if (!isSkipped)
        {
            canPlay = true;
            currentCutscene = baseGID2;
            DeactivateButton(sectionName);
        }
    }
}
