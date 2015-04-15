﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nett
{
    public class Toml
    {
        public static string WriteString<T>(T obj)
        {
            return WriteString(obj, TomlConfig.DefaultInstance);
        }

        public static string WriteString<T>(T obj, TomlConfig config)
        {
            TomlTable tt = TomlTable.From(obj, config);

            using (var ms = new MemoryStream(1024))
            {
                var sw = new StreamWriter(ms);
                tt.WriteTo(sw);
                sw.Flush();
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }

        public static T Read<T>(string toRead)
        {
            return Read<T>(toRead, TomlConfig.DefaultInstance);
        }

        public static T Read<T>(string toRead, TomlConfig tomlConfig)
        {
            TomlTable tt = Read(toRead);
            T result = tt.Get<T>(tomlConfig);
            return result;
        }

        public static TomlTable Read(string toRead)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toRead);
            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(toRead);
                writer.Flush();
                ms.Position = 0;
                return StreamTomlSerializer.Deserialize(ms);
            }
        }

        public static T ReadFile<T>(string filePath)
        {
            return ReadFile<T>(filePath, TomlConfig.DefaultInstance);
        }

        public static T ReadFile<T>(string filePath, TomlConfig config)
        {
            var tt = ReadFile(filePath, config);
            return tt.Get<T>(config);
        }

        public static TomlTable ReadFile(string filePath)
        {
            return ReadFile(filePath, TomlConfig.DefaultInstance);
        }

        public static TomlTable ReadFile(string filePath, TomlConfig config)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return StreamTomlSerializer.Deserialize(fs);
            }
        }
    }
}