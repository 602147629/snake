using UnityEngine;
using System.Collections;
using System;
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    public string name;
    public bool autoRegister;

    public ModuleAttribute(string name,bool autoRegister)
    {
        this.name = name;
        this.autoRegister = autoRegister;
    }
}
