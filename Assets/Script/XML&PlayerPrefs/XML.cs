using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class XML : MonoBehaviour
{
    public InstantiatePictos Ip;
    public InputField InputFieldValue;
    string wantedText;

    void Start()
    {
        Ip = GameObject.Find("GameMaster").GetComponent<InstantiatePictos>();
        LoadXmlStatByType();
    }

    public void LoadXmlStatByType()
    {
        if (InputFieldValue.text != "")
        {
            wantedText = InputFieldValue.text;
        }
        else
        {
            wantedText = "RepartitionEquilibre";
        }

        var xmlDoc = new XmlDocument();
        if(File.Exists(Application.dataPath + "/" + wantedText + ".xml")) 
        {
            xmlDoc.Load(Application.dataPath + "/" + wantedText + ".xml");

            var root = xmlDoc.SelectSingleNode("root").ChildNodes;

            foreach (XmlNode sg in root)
            {
                if (sg.Name == "Time")
                {
                    Ip.TimeRandMinInclusiv = int.Parse(sg.SelectSingleNode("TimeMin").InnerText);
                    Ip.TimeRandMaxExclusiv = int.Parse(sg.SelectSingleNode("TimeMax").InnerText);
                }
                if (sg.Name == "Rand")
                {
                    Ip.MinGenRandInclusiv = int.Parse(sg.SelectSingleNode("RandMin").InnerText);
                    Ip.MaxGenRandExclusiv = int.Parse(sg.SelectSingleNode("RandMax").InnerText);
                }
                if (sg.Name == "Repart")
                {
                    Ip.ChanceOfSpawnBleu = int.Parse(sg.SelectSingleNode("Blue").InnerText);
                    Ip.ChanceOfSpawnOrange = int.Parse(sg.SelectSingleNode("Orange").InnerText);
                    Ip.ChanceOfSpawnRouge = int.Parse(sg.SelectSingleNode("Red").InnerText);
                }
            }
        }
    }
}
