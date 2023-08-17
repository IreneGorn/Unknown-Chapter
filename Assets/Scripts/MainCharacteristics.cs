using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class MainCharacteristics : MonoBehaviour
{
    [SerializeField] private double _physicalAbilities = 1; // физические способности
    [SerializeField] private double _perception = 1;        // восприятие
    [SerializeField] private double _intellect  = 0;        // интелект 
    
    private double GetPhysicalAbilities(double value)
    {
        return _physicalAbilities;
    }
    
    private double GetPerception(double value)
    {
        return _perception;
    }
    
    private double GetIntellect(double value)
    {
        return _intellect;
    }

    private void OnEnable()
    {
        // Lua.RegisterFunction("PhysicalAbilities", this, SymbolExtensions.GetMethodInfo(() => _physicalAbilities((double)0)));
        Lua.RegisterFunction("PhysicalAbilities", this, SymbolExtensions.GetMethodInfo(() => GetPhysicalAbilities(1)));
        Lua.RegisterFunction("Perception", this, SymbolExtensions.GetMethodInfo(() => GetPerception(1)));
        Lua.RegisterFunction("Intellect", this, SymbolExtensions.GetMethodInfo(() => GetIntellect(1)));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("PhysicalAbilities");
        Lua.UnregisterFunction("Perception");
        Lua.UnregisterFunction("Intellect");
    }
}
