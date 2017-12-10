using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour {

    public GameObject MonsterManager;
    public float StopWatch = 0;
    FSM fsm = new FSM();


    bool firstStart = true;

    bool LeftTriggerButtonDown = false;


    // Static variables for singleton
    private static GameFlowManager _manager = null;
    public static GameFlowManager I
    {
        get { return _manager; }
    }

    // Use this for initialization
    void Start () {

        // Singleton
        if (I == null)
        {
            _manager = this;
        }
        else if (I != this)
        {
            Destroy(gameObject);
        }

        fsm.states.Add(HelenaStateType.Tutorial_Welcome, new State(HelenaStateType.Tutorial_Welcome, OnStateTutorialWelcome));
        fsm.states.Add(HelenaStateType.Tutorial_Moving, new State(HelenaStateType.Tutorial_Moving, OnStateTutorialMoving));
        fsm.states.Add(HelenaStateType.Tutorial_FirstCast, new State(HelenaStateType.Tutorial_FirstCast, OnStateTutorialFirstCast));
        fsm.states.Add(HelenaStateType.Tutorial_TrainMagics, new State(HelenaStateType.Tutorial_TrainMagics, OnStateTutorialTrainMagics));
        fsm.states.Add(HelenaStateType.Tutorial_Mana, new State(HelenaStateType.Tutorial_Mana, OnStateTutorialMana));
        fsm.states.Add(HelenaStateType.Tutorial_End, new State(HelenaStateType.Tutorial_End, OnStateTutorialEnd));

        fsm.states.Add(HelenaStateType.MainGame_Stage1, new State(HelenaStateType.MainGame_Stage1, OnStateMainGameStage1));
        fsm.states.Add(HelenaStateType.MainGame_Stage2, new State(HelenaStateType.MainGame_Stage2, OnStateMainGameStage2));
        fsm.states.Add(HelenaStateType.MainGame_Stage3, new State(HelenaStateType.MainGame_Stage3, OnStateMainGameStage3));
        fsm.states.Add(HelenaStateType.MainGame_Stage4, new State(HelenaStateType.MainGame_Stage4, OnStateMainGameStage4));
        fsm.states.Add(HelenaStateType.MainGame_Stage5, new State(HelenaStateType.MainGame_Stage5, OnStateMainGameStage5));
        fsm.states.Add(HelenaStateType.MainGame_GameOver, new State(HelenaStateType.MainGame_GameOver, OnStateMainGameGameOver));
        fsm.states.Add(HelenaStateType.MainGame_Clear, new State(HelenaStateType.MainGame_Clear, OnStateMainGameClear));
        //   fsm.StartState = fsm.states[HelenaStateType.Tutorial_Welcome];
        fsm.StartState = fsm.states[HelenaStateType.MainGame_Stage5];
    }

	// Update is called once per frame
	void Update () {
        fsm.Running();
	}

    void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    void OnStateTutorialWelcome()
    {
        if (fsm.isFirstFrame)
            UIManager.I.ChangeTutorialSystemUI("Welcome, Player! \nPress Y to start tutorial!");
        
        if (OVRInput.GetDown(OVRInput.RawButton.Y, OVRInput.Controller.Active))
        {
            fsm.SetNext(HelenaStateType.Tutorial_Moving);
        }
    }

    void OnStateTutorialMoving()
    {
        if (fsm.isFirstFrame)
            UIManager.I.ChangeTutorialSystemUI("Press Left Hand Trigger to move around");

        if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.Active) > 0)
        {
            fsm.SetNext(HelenaStateType.Tutorial_FirstCast);
        }
    }

    void OnStateTutorialFirstCast()
    {
        if (fsm.isFirstFrame)
            UIManager.I.ChangeTutorialSystemUI("Do Cast! \nPress Left Index Trigger.");

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active) > 0)
        {
            if (LeftTriggerButtonDown == false)
            {
                UIManager.I.ChangeTutorialSystemUI("Touch the top vertex and draw a circle Through vertices. If finished, Depress trigger.");
                LeftTriggerButtonDown = true;

                StartCoroutine(MagicCircleDrawManager.I.GuideFadeIn());
            }
        }
        else
        {
            if (LeftTriggerButtonDown == true)
                fsm.SetNext(HelenaStateType.Tutorial_TrainMagics);
        }
        
    }

    void OnStateTutorialTrainMagics()
    {
        if (fsm.isFirstFrame)
            UIManager.I.ChangeTutorialSystemUI("Good! Now, let's cast some other magics.");
    }

    void OnStateTutorialMana()
    {

    }

    void OnStateTutorialEnd()
    {

    }

    void OnStateMainGameStage1()
    {
        MonsterSpawner MS = MonsterManager.GetComponent<MonsterSpawner>();
        if (firstStart)
        {  
            MS.Activation_NORMAL = true;
            MS.Activation_TOTEM = false;
            MS.Activation_SWARM = true;
            MS.Activation_SHIELD = false;
            MS.Activation_FLY = false;
            MS.Activation_BIRD = false;

            MS.SpawnTime_NORMAL = 5f;
            MS.SpawnTime_SWARM = 10f;
            MS.TimeCounter_NORMAL = 5f;
            MS.TimeCounter_SWARM = 5f;

            firstStart = false;
        }
        StopWatch += Time.deltaTime;
        if(StopWatch >= 60)
        {
            MS.NumOfMonster = 0;
            MS.TimeCounter_NORMAL = 0f;
            MS.TimeCounter_SWARM = 0f;
            StopWatch = 0;
            firstStart = true;
            fsm.SetNext(HelenaStateType.MainGame_Stage2);
        }
    }

    void OnStateMainGameStage2()
    {
        MonsterSpawner MS = MonsterManager.GetComponent<MonsterSpawner>();
        if (firstStart)
        {
            MS.Activation_NORMAL = false;
            MS.Activation_TOTEM = true;
            MS.Activation_SWARM = false;
            MS.Activation_SHIELD = true;
            MS.Activation_FLY = false;
            MS.Activation_BIRD = false;

            MS.SpawnTime_TOTEM = 10f;
            MS.SpawnTime_SHIELD = 17f;
            MS.TimeCounter_TOTEM = 10f;
            MS.TimeCounter_SHIELD = 12f;

            firstStart = false;
        }
        StopWatch += Time.deltaTime;
        if (StopWatch >= 60)
        {
            MS.NumOfMonster = 0;
            MS.TimeCounter_TOTEM = 0f;
            MS.TimeCounter_SHIELD = 0f;
            StopWatch = 0;
            firstStart = true;
            fsm.SetNext(HelenaStateType.MainGame_Stage3);
        }
    }

    void OnStateMainGameStage3()
    {
        MonsterSpawner MS = MonsterManager.GetComponent<MonsterSpawner>();
        if (firstStart)
        {
            MS.Activation_NORMAL = false;
            MS.Activation_TOTEM = false;
            MS.Activation_SWARM = false;
            MS.Activation_SHIELD = false;
            MS.Activation_FLY = true;
            MS.Activation_BIRD = true;

            MS.SpawnTime_FLY = 10f;
            MS.SpawnTime_BIRD = 20f;
            MS.TimeCounter_FLY = 10f;
            MS.TimeCounter_BIRD = 15f;

            firstStart = false;
        }
        StopWatch += Time.deltaTime;
        if (StopWatch >= 60)
        {
            MS.NumOfMonster = 0;
            MS.TimeCounter_FLY = 0f;
            MS.TimeCounter_BIRD = 0f;
            StopWatch = 0;
            firstStart = true;
            fsm.SetNext(HelenaStateType.MainGame_Stage3);
        }
    }

    void OnStateMainGameStage4()
    {
        MonsterSpawner MS = MonsterManager.GetComponent<MonsterSpawner>();
        if (firstStart)
        {
            MS.Activation_NORMAL = true;
            MS.Activation_TOTEM = false;
            MS.Activation_SWARM = false;
            MS.Activation_SHIELD = false;
            MS.Activation_FLY = false;
            MS.Activation_BIRD = false;

            MS.SpawnTime_NORMAL = 1f;
            MS.TimeCounter_NORMAL = 1f;

            firstStart = false;
        }
        StopWatch += Time.deltaTime;
        if(MS.NumOfMonster > 40 && MS.Activation_NORMAL)
        {
            MS.TimeCounter_NORMAL = 0f;
            MS.Activation_NORMAL = false;
            MS.SummonBoss();
        }
        if (StopWatch >= 60)
        {
            MS.NumOfMonster = 0;
            MS.TimeCounter_FLY = 0f;
            MS.TimeCounter_BIRD = 0f;
            StopWatch = 0;
            firstStart = true;
            fsm.SetNext(HelenaStateType.MainGame_Stage3);
        }
    }

    void OnStateMainGameStage5()
    {
        MonsterSpawner MS = MonsterManager.GetComponent<MonsterSpawner>();
        if (firstStart)
        {
            MS.Activation_NORMAL = true;
            MS.Activation_TOTEM = true;
            MS.Activation_SWARM = true;
            MS.Activation_SHIELD = true;
            MS.Activation_FLY = true;
            MS.Activation_BIRD = true;

            MS.SpawnTime_NORMAL = 10f;
            MS.SpawnTime_TOTEM = 25f;
            MS.SpawnTime_SWARM = 20f;
            MS.SpawnTime_SHIELD = 30f;
            MS.SpawnTime_FLY = 20f;
            MS.SpawnTime_BIRD = 30f;
            MS.TimeCounter_NORMAL = 10f;
            MS.TimeCounter_TOTEM = 0f;
            MS.TimeCounter_SWARM = 0f;
            MS.TimeCounter_SHIELD = 0f;
            MS.TimeCounter_FLY = 0f;
            MS.TimeCounter_BIRD = 0f;

            firstStart = false;
        }
        StopWatch += Time.deltaTime;
        if (MS.NumOfMonster > 80 && MS.Activation_NORMAL)
        {
            MS.TimeCounter_NORMAL = 0f;
            MS.TimeCounter_TOTEM = 0f;
            MS.TimeCounter_SWARM = 0f;
            MS.TimeCounter_SHIELD = 0f;
            MS.TimeCounter_FLY = 0f;
            MS.TimeCounter_BIRD = 0f;
            MS.Activation_NORMAL = false;
            MS.Activation_TOTEM = false;
            MS.Activation_SWARM = false;
            MS.Activation_SHIELD = false;
            MS.Activation_FLY = false;
            MS.Activation_BIRD = false;
            MS.SummonBoss();
        }
        /*
        if (StopWatch >= 60)
        {
            MS.TimeCounter_FLY = 0f;
            MS.TimeCounter_BIRD = 0f;
            StopWatch = 0;
            firstStart = true;
            fsm.SetNext(HelenaStateType.MainGame_Stage3);
        }
        */
    }

    void OnStateMainGameGameOver()
    {
        Debug.Log("Game Over");
    }

    void OnStateMainGameClear()
    {

    }

    public void Transition(HelenaStateType next)
    {
        fsm.SetNext(next);
    }
}