using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM {

    public Dictionary<HelenaStateType, State> states = new Dictionary<HelenaStateType, State>();
    State current = null;
    State next = null;

    public void Running()
    {
        current.OnState();
    }
    
    public HelenaStateType SetNext(HelenaStateType state)
    {
        if ( states[state] != current)
            next = states[state];

        return state;
    }

    public State StartState
    {
        set {
            if (current == null)
                current = value;
        }
    }

    public void FixedUpdate() {
        if (next != null)
        {
            current = next;
            next = null;
            current.Reset();
        }
    }

    public bool isFirstFrame
    {
        get { return current.firstFrame; }
    }

}

public enum HelenaStateType { Tutorial_Welcome, Tutorial_Moving, Tutorial_FirstCast, Tutorial_TrainMagics, Tutorial_Mana, Tutorial_End,
                              MainGame_Stage1, MainGame_Stage2, MainGame_Stage3, MainGame_GameOver, MainGame_Clear};

public delegate void OnState();

public class State
{
    HelenaStateType type;
    OnState _on_state;

    public bool firstFrame = true;
    

    public State(HelenaStateType t, OnState s)
    {
        type = t;
        _on_state = s;
    }

    public void OnState()
    {
       _on_state();

        if (firstFrame)
            firstFrame = false;
    }

    public void Reset()
    {
        firstFrame = true;
    }


}