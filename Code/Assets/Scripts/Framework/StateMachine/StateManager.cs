
using System;
using FootStudio.Util;


namespace FootStudio.StateManagement
{
	public abstract class StateManager
	{
		/// <summary>
		/// action在当前帧已经被触发
		/// </summary>
		/// <value><c>true</c> if action triggered in update; otherwise, <c>false</c>.</value>
		public static bool ActionTriggeredInUpdate
		{
			get;
			set;
		}

		/// <summary>
		/// 状态正在转换，涉及到场景加载， AfterUpdate后才会为false，如果涉及到场景加载，加载完成后才会为false
		/// 游戏的主状态正在转换，这个时候不能做其它的状态转换
		/// </summary>
		/// <value><c>true</c> if state changing; otherwise, <c>false</c>.</value>
		public static bool StateChanging
		{
			get;
			set;
		}

		/// <summary>
		/// 用来判断状态是否转换完成
		/// </summary>
		/// <value><c>true</c> if state changed internal; otherwise, <c>false</c>.</value>
		public static bool StateChangedInternal
		{
			get;
			set;
		}

		/// <summary>
		/// update后游戏主状态才算完成
		/// </summary>
		public static void AfterUpdate()
		{
			StateManager.ActionTriggeredInUpdate = false;
			if (!StateManager.StateChangedInternal)
			{
				StateManager.StateChanging = false;
			}
		}
		
		public abstract bool IsActive();
		
		public virtual void OnGameStateExit(object state, object data)
		{
		}
		
		public virtual void OnGameStateEnter(object state, object data)
		{
		}
	}

	public abstract class StateManager<S, A> : StateManager where S : StateManager<S, A>.State {
		
		public abstract class State
		{
			
			protected StateManager<S, A> StateManager;
			
			public State(StateManager<S, A> stateManager)
			{
				this.StateManager = stateManager;
			}

            public virtual void OnEnter(S previousState, object data) { }

            public virtual void OnExit(S nextState, object data) { }

            public virtual void OnAction(A gameAction, object data) { }
			
			public virtual S OnPreEnter(S previousState, object data)
			{
				return this as S;
			}
			
			public virtual void OnPreExit(S nextState, object data)
			{
			}
			
			public virtual void OnAppResume()
			{
			}
			
			public virtual void OnAppPause()
			{
			}
			
			public virtual void OnUpdate()
			{
			}
			
			protected virtual bool ChangeState(S newState)
			{
				return this.ChangeState(newState, null);
			}
			
			protected virtual bool ChangeState(S newState, object data)
			{
				return this.StateManager.ChangeState(newState, data);
			}
		}

		public delegate void StateChangeEvent(S state, object data);
		
		private const string MsgInvalidToFireAction = "It's invalid to fire an action inside any of the State's methods. Fired action {0} in current state {1}.";
		
		private const string MsgCantCallOnActionTwiceFromTheSameStack = "Can't call OnAction from the same stack twice or more...";
		
		public StateManager<S, A>.StateChangeEvent OnStatePreEnter;
		
		public StateManager<S, A>.StateChangeEvent OnStatePreExit;
		
		public StateManager<S, A>.StateChangeEvent OnStateExit;
		
		public StateManager<S, A>.StateChangeEvent OnStateEnter;
		
		protected bool ActionProcessing;
		
		protected object Data;

		/// <summary>
		/// 执行action传入的数据，如果action里面有状态转换，那么可以取到这个数据使用
		/// </summary>
		private object PassForwardData;
		
		protected bool OnActionExecuting;
		
		protected virtual string Tag
		{
			get
			{
				return base.GetType().Name;
			}
		}
		
		public S CurrentState
		{
			get;
			protected set;
		}
		
		public S PreviousState
		{
			get;
			protected set;
		}
		
		public S NextState
		{
			get;
			protected set;
		}

		/// <summary>
		/// 强制更新状态
		/// </summary>
		/// <value><c>true</c> if force state reload; otherwise, <c>false</c>.</value>
		public bool ForceStateReload
		{
			get;
			set;
		}
		
		public override bool IsActive()
		{
			return this.CurrentState != null;
		}
		
		public virtual void OnAppResume()
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.OnAppResume();
			}
		}
		
		public virtual void OnAppPause()
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.OnAppPause();
			}
		}

		/// <summary>
		/// 执行某个Action
		/// </summary>
		/// <returns><c>true</c>, if action was fired, <c>false</c> otherwise.</returns>
		/// <param name="gameAction">Game action.</param>
		public bool FireAction(A gameAction)
		{
			return this.FireAction(gameAction, null);
		}

		/// <summary>
		/// 在状态中执行某个动作, 带参数
		/// </summary>
		/// <returns><c>true</c>, if action was fired, <c>false</c> otherwise.</returns>
		/// <param name="gameAction">Game action.</param>
		/// <param name="data">Data.</param>
		public virtual bool FireAction(A gameAction, object data)
		{
			if (this.CurrentState == null)
			{
				return false;
			}
			Assert.IsTrue(!this.ActionProcessing, MsgInvalidToFireAction, new object[]
			{
				gameAction,
				this.CurrentState
			});
			if (StateManager.StateChanging)
			{
				return false;
			}
			this.ActionProcessing = true;
			Assert.IsTrue(!this.OnActionExecuting, MsgCantCallOnActionTwiceFromTheSameStack, new object[0]);
			this.PassForwardData = data;
			this.HandleFireAction(gameAction, data);
			this.PassForwardData = null;
			this.ActionProcessing = false;
			StateManager.ActionTriggeredInUpdate = true;
			return true;
		}

		/// <summary>
		/// 执行当前状态的Action
		/// </summary>
		/// <param name="gameAction">Game action.</param>
		/// <param name="data">Data.</param>
		protected virtual void HandleFireAction(A gameAction, object data)
		{
			this.CurrentState.OnAction(gameAction, data);
		}

		/// <summary>
		/// 是否可以中断状态转换，返回true表示不可以
		/// 如果正在转换，并且新状态为空，那么可以转换
		/// </summary>
		/// <returns><c>true</c>, if state change was blocked, <c>false</c> otherwise.</returns>
		/// <param name="newState">New state.</param>
		protected virtual bool BlockStateChange(S newState)
		{
			return StateManager.StateChanging && newState != null;
		}
		
		protected virtual bool ChangeState(S newState, object data)
		{
			if (this.BlockStateChange(newState))
			{
				return false;
			}
			//进入空状态，如果是主游戏状态，那么会退出游戏
			if (newState == null)
			{
				if (this.CurrentState != null)
				{
					data = (data ?? this.PassForwardData);
					this.OnStatePreExitEvent(this.CurrentState, (S)((object)null), data);
					this.OnStateExitEvent(this.CurrentState, (S)((object)null), data);
				}
				this.CurrentState = (S)((object)null);
				this.ToNullState();
				return true;
			}
			if (newState != this.CurrentState || this.ForceStateReload)
			{
				data = (data ?? this.PassForwardData);
				this.Data = data;
				this.NextState = newState;
				this.PreviousState = this.CurrentState;
				if (this.CurrentState != null)
				{
					this.OnStatePreExitEvent(this.CurrentState, this.NextState, this.Data);
				}
				this.OnStatePreEnterEvent(this.NextState, this.PreviousState, data);
				this.StartStateChange();
				this.ForceStateReload = false;
			}
			return true;
		}
		
		protected virtual void OnStateChanged()
		{
			StateManager.StateChangedInternal = false; 
			if (this.PreviousState != null)
			{
				this.OnStateExitEvent(this.PreviousState, this.NextState, this.Data);
			}
			this.CurrentState = this.NextState;
			this.OnStateEnterEvent(this.CurrentState, this.PreviousState, this.Data);
			this.Data = null;
		}
		
		protected virtual void StartStateChange()
		{
			StateManager.StateChangedInternal = true;
			this.OnStateChanged();
		}
		
		protected abstract void ToNullState();
		
		public virtual void OnUpdate()
		{
			if (this.CurrentState == null)
			{
				return;
			}
			if (StateManager.StateChanging)
			{
				return;
			}
            
			this.CurrentState.OnUpdate ();
		}
		
		protected void OnStatePreEnterEvent(S callState, S state, object data)
		{
			callState.OnPreEnter(state, data);
			if (this.OnStatePreEnter != null)
			{
				this.OnStatePreEnter(state, data);
			}
		}
		
		private void OnStatePreExitEvent(S currentState, S state, object data)
		{
			currentState.OnPreExit(state, data);
			if (this.OnStatePreExit != null)
			{
				this.OnStatePreExit(state, data);
			}
		}
		
		protected virtual void OnStateExitEvent(S currentState, S state, object data)
		{
			currentState.OnExit(state, data);
			if (this.OnStateExit != null)
			{
				this.OnStateExit(state, data);
			}
		}
		
		private void OnStateEnterEvent(S currentState, S state, object data)
		{
			currentState.OnEnter(state, data);
			if (this.OnStateEnter != null)
			{
				this.OnStateEnter(state, data);
			}
		}
	}
}

