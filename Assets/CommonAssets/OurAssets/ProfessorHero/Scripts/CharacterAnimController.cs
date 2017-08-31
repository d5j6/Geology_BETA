using UnityEngine;
using System.Collections;


public class CharacterAnimController : MonoBehaviour {

	Animator animator; //Объявление компонента Animator
	Animation AnimLayer;

    public float WS = 0;
    public float AD = 0;
    public bool Hello = false;
    public bool Talking = false;

    float v; //Переменная для принятия значения "Vertical" (-1, 1), для перемещения персонажа вперёд/назад
	float h; //Переменная для принятия значения "Horizontal" (-1, 1), для вращения персонажа влево/вправо
	public float ProfessorRotationSpeed = 40.0f; //Скорость вращения персонажа вокруг оси Y, во время ходьбы
	public float LayerWeight = 0.3f; //Переменная для "веса" слоёв анимации
	public float LayerWeightChangeValue = 0.01f; //Переменная для числа на которое будет увлечиваться/уменьшаться "вес" слоёв
    bool hello_gesture; //Переменная отвечающая за жест "Поздороваться/Помахать рукой"
	bool talking_gesture; //Переменная отвечающая за включение анимации "Разговор, жестикуляцию"
	public KeyCode HelloGestureKey; //Назначить клавишу для жеста "Поздороваться"
	public KeyCode TalkingGesturesKey; //Назначить клавишу для запуска анимации "Разговор с жестами"
	public KeyCode IncreaseWeighKey; //Назначить клавишу для увеличения "веса" слоя на предустановленное значение
	public KeyCode DecreaseWeightKey; //Назначить клавишу для уменьшения "веса" слоя на предустановленное значение
	//int layers; //Переменная содержащая в себе число слоёв анимации



	void Start () {
		animator = GetComponent<Animator>(); //Назначить компонент Animator
		animator.SetLayerWeight(1, LayerWeight); //При старте назначить "вес" слоя анимации
		//layers = animator.layerCount; //Присвоение переменной числа слоёв анимации
    }

	void Update () {
		h = AD; //Отслеживание нажатий клавиш W/S (вперёд/надаз) (W: 0,1; S: -1,0)
		v = WS;  //Отслеживание нажатий клавш A/D (влево/вправо) (A: -1,0; D: 0,1);
        hello_gesture = Hello;
        if (Hello)
        {
            //Debug.Log("Hello = true");
            Hello = false;
        }
		//hello_gesture = Input.GetKeyUp(HelloGestureKey); //Отслеживание нажатия клавиши отвечающей за анимацию "Помахать рукой"

        if (Talking)
        {
            animator.SetBool("Talk_Gestures", !animator.GetBool("Talk_Gestures"));
            Talking = false;
        }
		/*if(Input.GetKeyUp(TalkingGesturesKey)) { //При нажатии клавиши включить анимацию "Разговор с жестами".
			animator.SetBool("Talk_Gestures", !animator.GetBool("Talk_Gestures")); //При повторном нажатии отключить анимацию
		}*/
    }

	void FixedUpdate(){
		animator.SetFloat ("V_Input", v); //Включение анимации хождения вперёд/назад
        animator.SetFloat("Turning", h); //Включение анимации вращения персонажа на месте
		animator.SetBool("Hello_Gesture", hello_gesture); //Включение анимации "Помахать рукой"

        //if (Input.GetAxis("Vertical") != 0)
        if (v != 0)
        { //Проверка нажатия клавиш вперёд/назад
            transform.Rotate(new Vector3(0, h * Time.deltaTime * ProfessorRotationSpeed, 0)); //Вращение персонажа по оси Y, только когда персонаж двигается вперёд/назад
		}
			
		if(Input.GetKeyUp(IncreaseWeighKey)) { //При нажатии клавиши для увеличения "веса" слоя, добавить к "весу" слоя предустановленное значение
			animator.SetLayerWeight(1, LayerWeight+=LayerWeightChangeValue);
		}
		if(Input.GetKeyUp(DecreaseWeightKey)) { //При нажатии клавиши для уменьшения "веса" слоя, убавить от "веса" слоя предустановленное значение
			animator.SetLayerWeight(1, LayerWeight-=LayerWeightChangeValue);
		}
	}
}
