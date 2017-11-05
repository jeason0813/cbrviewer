#region Header
// *******************************************************************************************
// Authors     : Erik Molenaar
// *******************************************************************************************
#endregion // Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CBR_Viewer.Model
{
    public static class SettingsTools
    {
        #region Xml
        public static XmlDocument GetXmlDocument(string filename)
        {
            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(filename))
            {

                XmlTextReader reader = new XmlTextReader(filename);
                try
                {
                    doc.Load(reader);

                    reader.Close();
                }
                catch (XmlException)
                {
                    //throw exception;
                    return null;
                }
            }
            return doc;
        }

        public static XmlTextWriter GetXmlWriter(string filename)
        {
            if (System.IO.File.Exists(filename) == true)
            {
                System.IO.File.Delete(filename);
            }
            // xml
            XmlTextWriter writer = new XmlTextWriter(filename, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            writer.WriteStartDocument();
            writer.WriteStartElement("settings");

            writer.WriteComment(" ©Erik Molenaar; All rights reserved");
            writer.WriteComment(" Automatically Generated File. Do Not Modify! ");
            return writer;
        }

        public static void CloseXmlWriter(XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        static public void SaveXmlDocument(string fileName, XmlDocument doc)
        {
            if ((fileName != "") && (doc != null))
            {
                XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 3;
                doc.Save(writer);
                writer.Flush();
                writer.Close();
            }
        }
        #endregion Xml

        #region Path
        public static string GetConfigFileName(string appName, bool useLocalPath)
        {
            string result = ITB(GetConfigPath(appName, useLocalPath));

            result += appName + ".config";
            return result;
        }

        public static string GetConfigPath(string appName, bool useLocalPath)
        {
            string result;
            if (useLocalPath == true)
            {
                result = SettingsTools.ITB(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
            else
            {
                result = SettingsTools.ITB(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            }
            System.IO.Directory.CreateDirectory(result + @"Gazillion-Bytes");
            result += SettingsTools.ITB("Gazillion-Bytes");
            System.IO.Directory.CreateDirectory(result + appName);
            result += SettingsTools.ITB(appName);

            return result;
        }

        public static string ITB(string path)
        {
            string result = path.Replace('/', '\\');
            if ((result.Length > 0) && (result[result.Length - 1] != '\\'))
            {
                result += '\\';
            }
            return result;
        }
        #endregion Path

        public static void WriteSettings()
        {
            string path = GetConfigFileName("CBReader", true);

            XmlTextWriter writer = GetXmlWriter(path);
            foreach (System.Configuration.SettingsProperty sp in global::CBR_Viewer.Properties.Settings.Default.Properties)
            {
                writer.WriteStartElement("prop");
                writer.WriteAttributeString("name", sp.Name);
                writer.WriteAttributeString("type", sp.PropertyType.ToString());

                writer.WriteAttributeString("val", global::CBR_Viewer.Properties.Settings.Default[sp.Name].ToString());
                
                writer.WriteEndElement();
            }

            CloseXmlWriter(writer);
        }

        public static void ReadSettings()
        {
            string path = GetConfigFileName("CBReader", true);

            XmlDocument doc = GetXmlDocument(path);
            if (doc == null)
            {
                return;
            }
            foreach (XmlNode node in doc.SelectNodes("./settings//prop"))
            {
                XmlElement elm = (XmlElement)node;
                string name = elm.GetAttribute("name");
                string value = elm.GetAttribute("val");
                Type t = global::CBR_Viewer.Properties.Settings.Default.Properties[name].PropertyType;
                
                if (t.BaseType == typeof(Enum))
                {
                    var v = Enum.Parse(t, value);
                    global::CBR_Viewer.Properties.Settings.Default[name] = v;
                }
                else
                {
                    var v = Convert.ChangeType(value, t);
                    global::CBR_Viewer.Properties.Settings.Default[name] = v;
                }
            }
        }
    }
}
