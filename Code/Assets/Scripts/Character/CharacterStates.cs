using UnityEngine;
using System.Collections;
using FootStudio.Util;

namespace FootStudio.Framework
{
    public class TestCharacterStateManager : BaseStateManager<CharacterAction>
    {
        protected override string Tag
        {
            get
            {
                return "TestCharacterStateManager";
            }
        }

        public TestCharacter CharacterBase
        {
            get;
            protected set;
        }

        #region States

        public IdleState IdleState
        {
            get;
            private set;
        }

        #endregion

        public TestCharacterStateManager(TestCharacter character)
            : base()
        {
            IdleState = new IdleState(this);

            if (character != null)
            {
                CharacterBase = character;
            }
        }

        public override void Init()
        {
            EnterInitialState(IdleState);
        }

    }


    public class IdleState : BaseState<CharacterAction>
    {
        TestCharacter character;
        TestCharacterStateManager characterStateManager;
        
        public IdleState(TestCharacterStateManager mgr)
            : base(mgr)
        {
            character = mgr.CharacterBase as TestCharacter;
            characterStateManager = mgr;

            Debug.LogError("IdleState : " + character.tag);
        }

        public override void OnEnter(BaseState<CharacterAction> previousState, object data)
        {
            base.OnEnter(previousState, data);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnAction(CharacterAction characterAction, object data)
        {
            GameLog.Debug(Tag + characterAction);
        }

        public override void OnExit(BaseState<CharacterAction> nextState, object data)
        {
            base.OnExit(nextState, data);
        }
    }
}