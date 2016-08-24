using UnityEngine;
using System.Collections;

/// <summary>
/// 巡游
/// </summary>
public class IdleState : BaseCharacterState {
    private Vector3 des = new Vector3(30, 10, -10);
    private CharacterModel model;
    
    public IdleState(CharacterStateManager mgr) 
        : base(mgr) { 

    }

    public override void OnEnter(BaseCharacterState previousState, object data) {
        base.OnEnter(previousState, data);
        model = this.CharacterStateManager.CharacterBase.Model;
    }

    public override void OnUpdate() {
        if (!model.IsAtPosition(des))
        {
            model.Move(des, Time.deltaTime);
        }
        else
        {
            //des = Random.Range(-1f, 1f) * new Vector3(30, 10, -10);
            ChangeState(this.CharacterStateManager.EscapeState);
        }
        
        base.OnUpdate();
    }

}

/// <summary>
/// 受惊逃跑
/// </summary>
public class EscapeState : BaseCharacterState {
    private CharacterBase character;

    private Vector3 farAway = new Vector3(100, 100, 0);
    public EscapeState(CharacterStateManager mgr) 
        : base(mgr) {
            character = this.CharacterStateManager.CharacterBase;
    }

    public override void OnEnter(BaseCharacterState previousState, object data) {
        base.OnEnter(previousState, data);
        character.Speed = 30;
    }

    public override void OnUpdate() {
        if (!character.Model.IsAtPosition(farAway))
        {
            character.Model.Move(farAway, Time.deltaTime);
        }
        else
        {
            ChangeState(this.CharacterStateManager.IdleState);
        }

        base.OnUpdate();
    }


}

/// <summary>
/// 放松玩耍
/// </summary>
public class PlayingState : BaseCharacterState {

    public PlayingState(CharacterStateManager mgr)
        : base(mgr) {

    }

}

/// <summary>
/// 寻找食物
/// </summary>
public class FindSthToEatState : BaseCharacterState {

    public FindSthToEatState(CharacterStateManager mgr)
        : base(mgr) {

    }
}

/// <summary>
/// 吃 (饲料/小鱼)
/// </summary>
public class EatState : BaseCharacterState {

    public EatState(CharacterStateManager mgr)
        : base(mgr) {

    }

}

/// <summary>
/// 捕猎小鱼
/// </summary>
public class HuntState : BaseCharacterState {

    public HuntState(CharacterStateManager mgr)
        : base(mgr) {

    }
}

