using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine<T>
{
    List<State<T>> StateList;
    public T Owner { get; set; }

    public State<T> InitialState { get; set; }
    State<T> CurrentState { get; set; }

    public StateMachine(T owner)
    {
        this.Owner = owner;
        StateList = new List<State<T>>();
    }

    void Start()
    {
        CurrentState = InitialState;
        CurrentState.EntryAction();
    }
    void Update()
    {
        if (CurrentState != null)
        {
            Transition<T> triggeredTransition = null;
            for (int i = 0; i < CurrentState.Transitions.Count; i++)
            {
                if (CurrentState.Transitions[i].IsTriggered())
                {
                    triggeredTransition = CurrentState.Transitions[i];
                }
            }
            if (triggeredTransition != null)
            {
                triggeredTransition.EntryAction();
                State<T> targetState = triggeredTransition.TargetState;
                CurrentState.ExitAction();
                triggeredTransition.ExitAction();
                targetState.EntryAction();
                CurrentState = targetState;
            }
            else
            {
                CurrentState.EntryAction();
            }
        }
    }
    void AddState(State<T> newState)
    {
        StateList.Add(newState);
    }

}
public abstract class Transition<T>
{
    public T Owner;
    public State<T> TargetState { get; set; }

    public Transition(T owner)
    {
        this.Owner = owner;
        
    }
    public virtual void EntryAction()
    {
        throw new System.NotImplementedException();
    }
    public virtual void ExitAction()
    {
        throw new System.NotImplementedException();
    }
    public virtual bool IsTriggered()
    {
        throw new System.NotImplementedException();
    }
}
public abstract class State<T>
{
    public T Owner;
    public List<Transition<T>> Transitions { get; private set; }

    public string Name { get; private set; }

    public State(T Owner)
    {
        this.Owner = Owner;
        Transitions = new List<Transition<T>>();
    }
    public virtual void Action()
    {
        throw new System.NotImplementedException();
    }
    public virtual void EntryAction()
    {
        throw new System.NotImplementedException();
    }
    public virtual void ExitAction()
    {
        throw new System.NotImplementedException();
    }
    public virtual string GetStateName()
    {
        throw new System.NotImplementedException();
    }
    public void AddTransition(Transition<T> transition)
    {
        Transitions.Add(transition);
    }
}
