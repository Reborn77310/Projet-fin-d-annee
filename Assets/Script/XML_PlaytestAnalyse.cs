using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class XML_PlaytestAnalyse : MonoBehaviour
{ 
    public static int _elementValue;
    
    public static string elmtName;
    
    static XmlNodeList listeNoeuds;
    static XmlElement elmRoot;

    private string filePathAfterCreated;

    void Start()
    {
        CreateNewDocument();
    }

    void CreateNewDocument()
    {
        bool canCreate = true;
        for (int i = 0; i < 200; i++)
        {
            string filepath = Application.dataPath + @"/Données playtest/DonnéesPlaytest" + i + ".xml";
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(filepath) && canCreate)
            {
                xmlDoc.CreateElement("root");
                
                xmlDoc.Save(Path.Combine(Application.dataPath + @"/Données playtest/" , "DonnéesPlaytest" + i + ".xml"));
                canCreate = false;
                filePathAfterCreated = Application.dataPath + @"/Données playtest/DonnéesPlaytest" + i + ".xml";
            }
        }
        XmlTextWriter root = new XmlTextWriter(filePathAfterCreated,Encoding.UTF8);
        root.Formatting = Formatting.Indented;
        
        root.WriteStartElement("root");
        root.WriteEndElement();
        root.Close();
    }

    private void OnApplicationQuit()
    {
        SaveThis("Temps_en_secondes","Durée_totale", Mathf.RoundToInt(Time.time));
    }

    public void SaveThis (string groupeName,string elementName,int valueInt) {
        
        elmtName = elementName;
        _elementValue = valueInt;

        string filepath = filePathAfterCreated;
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(filepath))
        {
            xmlDoc.Load(filepath);
            elmRoot = xmlDoc.DocumentElement;
           
            XmlElement elmNew;

            listeNoeuds = null;
            listeNoeuds = xmlDoc.GetElementsByTagName(groupeName); // récupération de la liste des noeuds 

            elmNew = xmlDoc.CreateElement(groupeName);
            
            if (elmRoot.SelectSingleNode(elmNew.Name) != null) // verifie si le noeud existe
            {
                elmRoot.RemoveChild(elmRoot.SelectSingleNode(elmNew.Name)); // si oui on le supprime
               
            }
             
            SetValuesBeforeXmlSave(xmlDoc, elmNew); // lance la fonction qui rajoute un noeud
          
            elmRoot.AppendChild(elmNew);         // ajout noeud
            
            xmlDoc.Save(filepath);
        }
    }

    private static void SetValuesBeforeXmlSave(XmlDocument xmlDoc, XmlElement elmNew) 
    {
        
        foreach (XmlNode noeud in listeNoeuds)   // boucle sur la liste de noeuds contenu dans l'élement "string groupeName"                 
        {
            XmlNodeList listeContenu = noeud.ChildNodes;     // on récupere les contenus

            foreach (XmlNode elm in listeContenu)        
            {
                
                if (elm.Name != elmtName) // on empeche de recopier l'élément qu'on enregistre pour éviter la multiplication de celui ci en comparant le nom de l'élément
                {
                    XmlElement itemcopy = xmlDoc.CreateElement(elm.Name); // on ajoute l'element 
                    itemcopy.InnerText = "" + elm.InnerText;

                    elmNew.AppendChild(itemcopy);
                }
            }
        }

        XmlElement item = xmlDoc.CreateElement(elmtName);     // on creer un nouvel element 
        item.InnerText = "" + _elementValue; //enregistrement dans l'item

        elmNew.AppendChild(item); // ajout du noeud
    }
}
