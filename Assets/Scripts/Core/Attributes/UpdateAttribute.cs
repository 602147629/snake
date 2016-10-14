using UnityEngine;
using System.Collections;
using System;
[AttributeUsage(AttributeTargets.Class)]
public class UpdateAttribute : Attribute
{
    public string name;
    public bool autoRegister;

    public UpdateAttribute(string name,bool autoRegister)
    {
        this.name = name;
        this.autoRegister = autoRegister;
    }
}
