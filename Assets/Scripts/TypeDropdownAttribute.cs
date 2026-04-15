using System;
using UnityEngine;

public class TypeDropdownAttribute : PropertyAttribute {
    public Type type;

    public TypeDropdownAttribute(Type type) {
        this.type = type;
    }
}
