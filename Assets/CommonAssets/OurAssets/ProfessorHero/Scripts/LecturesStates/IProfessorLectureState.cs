public interface IProfessorLectureState
{
    void enter(System.Action callback = null);
    void exit(System.Action callback = null);
    bool tryInterrupt(System.Action callback = null);
}
