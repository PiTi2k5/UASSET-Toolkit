﻿using System;
using System.Collections.Generic;
using System.Text;
using UassetToolkit.UPropertyTypes;

namespace UassetToolkit
{
    public class PropertyReader
    {
        public List<UProperty> props;

        public PropertyReader(List<UProperty> props)
        {
            this.props = props;
        }

        public UProperty GetProperty(string name, int index = -1)
        {
            //Find
            foreach(UProperty p in props)
            {
                if (p.name == name && (index == -1 || index == p.index))
                    return p;
            }
            return null;
        }

        public T GetProperty<T>(string name, int index = -1)
        {
            //Get property
            UProperty p = GetProperty(name, index);

            //If it was not found, return default
            if (p == null)
                return default(T);

            //Cast
            return (T)Convert.ChangeType(p, typeof(T));
        }

        public string GetPropertyString(string name, string defaultValue, int index = -1)
        {
            var r = GetProperty<StrProperty>(name, index);

            if (defaultValue == null && r == null)
                throw new Exception($"Required attribute is missing for name {name}.");

            if (r == null)
                return defaultValue;
            else
                return r.data;
        }

        public float GetPropertyFloat(string name, float? defaultValue, int index = -1)
        {
            var r = GetProperty<FloatProperty>(name, index);

            if (defaultValue == null && r == null)
                throw new Exception($"Required attribute is missing for name {name}.");

            if (r == null)
                return defaultValue.Value;
            else
                return r.data;
        }

        public int GetPropertyInt(string name, int? defaultValue, int index = -1)
        {
            var r = GetProperty<IntProperty>(name, index);

            if (defaultValue == null && r == null)
                throw new Exception($"Required attribute is missing for name {name}.");

            if (r == null)
                return defaultValue.Value;
            else
                return r.data;
        }

        public bool GetPropertyBool(string name, bool? defaultValue, int index = -1)
        {
            var r = GetProperty<BoolProperty>(name, index);

            if (defaultValue == null && r == null)
                throw new Exception($"Required attribute is missing for name {name}.");

            if (r == null)
                return defaultValue.Value;
            else
                return r.flag;
        }
    }
}
