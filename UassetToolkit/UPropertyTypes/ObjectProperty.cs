﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UassetToolkit.UPropertyTypes
{
    public class ObjectProperty : UProperty
    {
        public ObjectPropertyType objectRefType;

        public int objectIndex; //Only used if the above is ObjectPropertyType.TypeID
        public string className; //Only used if the above is ObjectPropertyType.TypePath

        [JsonIgnore]
        public UAssetFile source;

        public ObjectProperty(IOMemoryStream ms, UAssetFile f) : base(ms, f)
        {

        }

        public override void Read(IOMemoryStream ms, UAssetFile f)
        {
            source = f;
            //If this is an array, assume 4
            if (isArray)
                length = 4;
            //If the length is four (only seems to happen on version 5), this is an integer.
            if (length == 4)
            {
                objectRefType = ObjectPropertyType.TypeID;
                objectIndex = ms.ReadInt();
            }
            else if (length >= 8)
            {
                //Read type
                int type = ms.ReadInt();
                if (type > 1 || type < 0)
                    throw new Exception($"Unknown ref type! Expected 0 or 1, but got {type} instead!");
                //Convert this to our enum
                objectRefType = (ObjectPropertyType)type;
                //Depending on the type, read it in.
                if (objectRefType == ObjectPropertyType.TypeID)
                    objectIndex = ms.ReadInt();
                if (objectRefType == ObjectPropertyType.TypePath)
                    className = ms.ReadNameTableEntry(f);
            }
            else
            {
                throw new Exception($"Unknown object ref length! Expected 4 or >= 8, but got {length} instead.");
            }
        }

        public override string WriteString()
        {
            return $"objectRefType={objectRefType.ToString()}, objectIndex={objectIndex}, className={className}, sourceClassname={source.classname}";
        }

        public UAssetFileBlueprint GetReferencedFileBlueprint()
        {
            if (source.GetType() != typeof(UAssetFileBlueprint))
                throw new Exception("Only UAssetFileBlueprint are supported for getting refrenced files with this method.");
            return ((UAssetFileBlueprint)source).GetReferencedUAsset(this);
        }

        public enum ObjectPropertyType
        {
            TypeID = 0,
            TypePath = 1
        }
    }
}
