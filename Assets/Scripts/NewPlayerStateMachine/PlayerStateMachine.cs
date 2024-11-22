using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{

    [field: SerializeField] public Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(new IdlePlayerState(this));
    }

}
