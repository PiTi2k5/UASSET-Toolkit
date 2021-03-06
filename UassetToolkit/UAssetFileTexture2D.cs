﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UassetToolkit
{
    public class UAssetFileTexture2D : UAssetFile
    {
        //Package data
        public List<UProperty> properties;
        public Stream payload;

        public static UAssetFileTexture2D OpenFile(string pathname, bool isDebugEnabled, string classname, string rootPath)
        {
            using (FileStream fs = new FileStream(pathname, FileMode.Open, FileAccess.Read))
            {
                return OpenFile(fs, isDebugEnabled, classname, rootPath);
            }
        }

        public static UAssetFileTexture2D OpenFile(FileStream fs, bool isDebugEnabled, string classname, string rootPath)
        {
            UAssetFileTexture2D f = new UAssetFileTexture2D();
            f.ReadFile(fs, isDebugEnabled, classname, rootPath);
            return f;
        }

        public UAssetFileTexture2D OpenUAssetWithSameSettings(string path, string classname)
        {
            UAssetFileTexture2D f = UAssetFileTexture2D.OpenFile(path, isDebugModeEnabled, classname, rootPath);
            return f;
        }

        public override void ReadFile(Stream s, bool isDebugEnabled, string classname, string rootPath)
        {
            //Do initial init
            base.ReadFile(s, isDebugEnabled, classname, rootPath);

            //Now, read properties
            ReadImageProperties();

            //Read payload
            payload = CopyPayload();
        }

        void ReadImageProperties()
        {
            //Find the embedded game object with the data
            EmbeddedGameObjectTableHead result = null;
            foreach(var r in gameObjectEmbeds)
            {
                if (r.type == classname)
                    result = r;
            }

            //Crash if not correct
            if (result == null)
                throw new Exception("Could not find image property data.");

            //Go to
            stream.position = result.dataLocation;

            //Read
            properties = UProperty.ReadProperties(stream, this, null, false);
        }
    }
}
