using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour {

    FSM fsm = new FSM();



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

        fsm.StartState = fsm.states[HelenaStateType.Tutorial_Welcome];

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

                StartCoroutine(MagicCircleInputManager.I.GuideFadeIn());
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
}
