using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EvolutionState
{
    BeforeAnyEvolution,
    EatenLAndEvolved,
    NoLEvolvedFinal
}

public enum SubState
{
    WetWings,
    Hungry,
    Healthy,
}

public abstract class CharacterState 
{
    public abstract float GetWalkSpeed();
    public abstract float GetJumpHeight();
    public abstract bool CanDoubleJump();
    public abstract bool CanFloat();
}

public class BeforeAnyEvolutionState : CharacterState
{
    private SubState currentSubState;
    
    public BeforeAnyEvolutionState(SubState initialSubState)
    {
        currentSubState = initialSubState;
    }
    
    public void SetSubState(SubState newSubState)
    {
        currentSubState = newSubState;
    }
    
    public override float GetWalkSpeed()
    {
        switch (currentSubState)
        {
            case SubState.WetWings:
                return 0.55f;
            case SubState.Hungry:
                return 0.4f;
            case SubState.Healthy:
                return 1.0f;
            default:
                return 1.0f;
        }
    }
    
    public override float GetJumpHeight()
    {
        switch (currentSubState)
        {
            case SubState.WetWings:
                return 0.5f;
            case SubState.Hungry:
                return 0.75f;
            case SubState.Healthy:
                return 1.0f;
            default:
                return 1.0f;
        }
    }

    public override bool CanDoubleJump()
    {
        return false;
    }
    
    public override bool CanFloat()
    {
        return false;
    }
}

public class EatenLAndEvolvedState : CharacterState
{
    private SubState currentSubState;
    
    public EatenLAndEvolvedState(SubState initialSubState)
    {
        currentSubState = initialSubState;
    }
    
    public void SetSubState(SubState newSubState)
    {
        currentSubState = newSubState;
    }
    
    public override float GetWalkSpeed()
    {
        switch (currentSubState)
        {
            case SubState.WetWings:
                return 1.0f;
            case SubState.Hungry:
                return 0.85f;
            case SubState.Healthy:
                return 1.2f;
            default:
                return 1.2f;
        }
    }
    
    public override float GetJumpHeight()
    {
        switch (currentSubState)
        {
            case SubState.WetWings:
                return 1.0f;
            case SubState.Hungry:
                return 0.75f;
            case SubState.Healthy:
                return 1.5f;
            default:
                return 1.5f;
        }
    }

    public override bool CanDoubleJump()
    {
        return true;
    }
    
    public override bool CanFloat()
    {
        return false;
    }
}

public class NoLEvolvedFinalState : CharacterState
{
    private SubState currentSubState;
    
    public NoLEvolvedFinalState(SubState initialSubState)
    {
        currentSubState = initialSubState;
    }
    
    public void SetSubState(SubState newSubState)
    {
        currentSubState = newSubState;
    }
    
    public override float GetWalkSpeed()
    {
        switch (currentSubState)
        {
            case SubState.WetWings:
                return 1.0f;
            case SubState.Hungry:
                return 0.85f;
            case SubState.Healthy:
                return 1.2f;
            default:
                return 1.2f;
        }
    }
    
    public override float GetJumpHeight()
    {
        switch (currentSubState)
        {
            case SubState.WetWings:
                return 1.0f;
            case SubState.Hungry:
                return 0.75f;
            case SubState.Healthy:
                return 2.0f;
            default:
                return 2.0f;
        }
    }

    public override bool CanDoubleJump()
    {
        return true;
    }
    
    public override bool CanFloat()
    {
        return true;
    }
}
