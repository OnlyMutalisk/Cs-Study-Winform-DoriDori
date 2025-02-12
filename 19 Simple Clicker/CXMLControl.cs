﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace _18.Simple_Clicker
{
    class CXMLControl
    {
        public static string _TICK = "_TICK";
        public static string _TOTAL = "_TOTAL";
        public static string _LEVEL_1 = "_LEVEL_1";
        public static string _LEVEL_3 = "_LEVEL_3";
        public static string _LEVEL_50 = "_LEVEL_50";

        public  Dictionary<string, string> fXML_Reader(string strXMLPath)
        {
            string strRijndaelText = File.ReadAllText(strXMLPath);
            string strDECText = CRijndael.DecryptString(strRijndaelText, CRijndael._bkey);

            Dictionary<string, string> DXMLConfig = new Dictionary<string, string>();

            using (XmlReader rd = XmlReader.Create(new StringReader(strDECText)))
            {
                while(rd.Read())
                {
                    if(rd.Name.Equals("SETTING"))
                    {
                        string strID = rd["id"];
                        rd.Read();

                        string strTICK = rd.ReadElementContentAsString(_TICK, "");
                        DXMLConfig.Add(_TICK, strTICK);

                        string strTOTAL = rd.ReadElementContentAsString(_TOTAL, "");
                        DXMLConfig.Add(_TOTAL, strTOTAL);

                        string strLEVEL_1 = rd.ReadElementContentAsString(_LEVEL_1, "");
                        DXMLConfig.Add(_LEVEL_1, strLEVEL_1);

                        string strLEVEL_3 = rd.ReadElementContentAsString(_LEVEL_3, "");
                        DXMLConfig.Add(_LEVEL_3, strLEVEL_3);

                        string strLEVEL_50 = rd.ReadElementContentAsString(_LEVEL_50, "");
                        DXMLConfig.Add(_LEVEL_50, strLEVEL_50);

                    }
                }
            }

            return DXMLConfig;
        }

        public void fXML_Writer(string strXMLPath, Dictionary<string, string> DXMLConfig)
        {
            StringBuilder sb = new StringBuilder();

            // using(XmlWriter wr = XmlWriter.Create(strXMLPath))
            using(XmlWriter wr = XmlWriter.Create(sb))
            {
                wr.WriteStartDocument();

                //SETTING
                wr.WriteStartElement("SETTING");
                wr.WriteAttributeString("ID", "0001");

                wr.WriteElementString(_TICK, DXMLConfig[_TICK]);
                wr.WriteElementString(_TOTAL, DXMLConfig[_TOTAL]);
                wr.WriteElementString(_LEVEL_1, DXMLConfig[_LEVEL_1]);
                wr.WriteElementString(_LEVEL_3, DXMLConfig[_LEVEL_3]);
                wr.WriteElementString(_LEVEL_50, DXMLConfig[_LEVEL_50]);

                wr.WriteEndElement();
                wr.WriteEndDocument();
            }

            string strRijndaelText = CRijndael.EncryptString(sb.ToString(), CRijndael._bkey);

            File.WriteAllText(strXMLPath, strRijndaelText);
        }

    }
}
