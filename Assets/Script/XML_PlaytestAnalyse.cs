using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class XML_PlaytestAnalyse : MonoBehaviour
{
    public static string _elementValue;

    public static string elmtName;

    static XmlNodeList listeNoeuds;
    static XmlElement elmRoot;

    private string filePathAfterCreated;

    public static float DureeDuCombat1 = 0.0f;
    public static bool firstFight = false;
    public static float DureeDuCombat2 = 0.0f;
    public static string IntegrityAfterFirstFight;
    public static string IntegrityAfterSecondFight;
    public static int NumberOfPause = 0;
    public static float TimePaused = 0;
    public static List<float> TimeBetweenTwoOperations = new List<float>();
    public static int NumberOfCardRecupCombat1 = 0;
    public static int NumberOfCardRecupCombat2 = 0;
    public static int[] RotationSlot2premiercombat = new int[4];
    public static int[] RotationSlot2deuxiemecombat = new int[4];
    public static int[] NumberOverdriveSetupsCombat1 = new int[6];
    public static int[] NumberOverdriveSetupsCombat2 = new int[6];
    public static int NumberOfOverdriveCombat1 = 0;
    public static int NumberOfOverdriveCombat2 = 0;
    public static int[] CompteurUtilisationEquipementCombat1 = new int[6];
    public static int[] CompteurUtilisationEquipementCombat2 = new int[6];
    public static int[] SalleAdverseViseePremierCombat = new int[4];
    public static int[] SalleNestViseePremierCombat = new int[4];
    public static int[] SalleAdverseViseeDeuxiemeCombat = new int[4];
    public static int[] SalleNestViseeDeuxiemeCombat = new int[4];
    public static int NombreActionEffectueSurSalleQuiPreparaitUneActionPremierCombat = 0;
    public static int NombreActionEffectueSurSalleQuiPreparaitUneActionDeuxiemeCombat = 0;
    public static int NombreActionEffectueSurSalleQuiRecevaitUneActionPremierCombat = 0;
    public static int NombreActionEffectueSurSalleQuiRecevaitUneActionDeuxiemeCombat = 0;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            RotationSlot2premiercombat[i] = 0;
            RotationSlot2deuxiemecombat[i] = 0;
            SalleAdverseViseeDeuxiemeCombat[i] = 0;
            SalleAdverseViseePremierCombat[i] = 0;
            SalleNestViseePremierCombat[i] = 0;
            SalleNestViseeDeuxiemeCombat[i] = 0;
        }
        for (int i = 0; i < 6; i++)
        {
            NumberOverdriveSetupsCombat1[i] = 0;
            NumberOverdriveSetupsCombat2[i] = 0;
            CompteurUtilisationEquipementCombat1[i] = 0;
            CompteurUtilisationEquipementCombat2[i] = 0;
        }
    }

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

                xmlDoc.Save(Path.Combine(Application.dataPath + @"/Données playtest/", "DonnéesPlaytest" + i + ".xml"));
                canCreate = false;
                filePathAfterCreated = Application.dataPath + @"/Données playtest/DonnéesPlaytest" + i + ".xml";
            }
        }
        XmlTextWriter root = new XmlTextWriter(filePathAfterCreated, Encoding.UTF8);
        root.Formatting = Formatting.Indented;

        root.WriteStartElement("root");
        root.WriteEndElement();
        root.Close();
    }

    private void OnApplicationQuit()
    {
        SaveThis("Temps_en_secondes", "Durée_totale", Mathf.RoundToInt(Time.time).ToString());
        SaveThis("Temps_en_secondes", "Durée_premier_combat", Mathf.RoundToInt(DureeDuCombat1).ToString());
        SaveThis("Temps_en_secondes", "Durée_deuxieme_combat", Mathf.RoundToInt(DureeDuCombat2).ToString());
        SaveThis("Infos_véhicule", "Intégrité_après_premier_combat", IntegrityAfterFirstFight);
        SaveThis("Infos_véhicule", "Intégrité_après_deuxième_combat", IntegrityAfterSecondFight);
        SaveThis("Infos_joueurs", "Nombres_de_pauses", NumberOfPause.ToString());
        SaveThis("Temps_en_secondes", "Temps_de_pause", Mathf.RoundToInt(TimePaused).ToString() + " ça peut pas marcher pour l'instant pck quand c'est pause le time est à 0");

        float tempsMoyen = 0;
        for (int i = 0; i < TimeBetweenTwoOperations.Count; i++)
        {
            tempsMoyen += TimeBetweenTwoOperations[i];
        }
        tempsMoyen /= TimeBetweenTwoOperations.Count;

        SaveThis("Infos_joueurs", "Recuperation_cartes_du_terrain_combat1", NumberOfCardRecupCombat1.ToString());
        SaveThis("Infos_joueurs", "Recuperation_cartes_du_terrain_combat2", NumberOfCardRecupCombat2.ToString());

        for (int i = 0; i < 4; i++)
        {
            SaveThis("Infos_salles", "Rotation_module_2_premier_combat_salle_" + i, RotationSlot2premiercombat[i].ToString());
        }
        for (int i = 0; i < 4; i++)
        {
            SaveThis("Infos_salles", "Rotation_module_2_deuxieme_combat_salle_" + i, RotationSlot2deuxiemecombat[i].ToString());
        }

        SaveThis("Infos_salles", "Nombre_overdrive_combat1", NumberOfOverdriveCombat1.ToString());
        SaveThis("Infos_salles", "Tourelle_BK1_nombre_overdrive_combat1", NumberOverdriveSetupsCombat1[0].ToString());
        SaveThis("Infos_salles", "Tourelle_BK2_nombre_overdrive_combat1", NumberOverdriveSetupsCombat1[1].ToString());
        SaveThis("Infos_salles", "Canon_IEM_nombre_overdrive_combat1", NumberOverdriveSetupsCombat1[2].ToString());
        SaveThis("Infos_salles", "Brouilleur_nombre_overdrive_combat1", NumberOverdriveSetupsCombat1[3].ToString());
        SaveThis("Infos_salles", "Turbine_nombre_overdrive_combat1", NumberOverdriveSetupsCombat1[4].ToString());
        SaveThis("Infos_salles", "HGoS_nombre_overdrive_combat1", NumberOverdriveSetupsCombat1[5].ToString());

        SaveThis("Infos_salles", "Nombre_overdrive_combat2", NumberOfOverdriveCombat2.ToString());
        SaveThis("Infos_salles", "Tourelle_BK1_nombre_overdrive_combat2", NumberOverdriveSetupsCombat2[0].ToString());
        SaveThis("Infos_salles", "Tourelle_BK2_nombre_overdrive_combat2", NumberOverdriveSetupsCombat2[1].ToString());
        SaveThis("Infos_salles", "Canon_IEM_nombre_overdrive_combat2", NumberOverdriveSetupsCombat2[2].ToString());
        SaveThis("Infos_salles", "Brouilleur_nombre_overdrive_combat2", NumberOverdriveSetupsCombat2[3].ToString());
        SaveThis("Infos_salles", "Turbine_nombre_overdrive_combat2", NumberOverdriveSetupsCombat2[4].ToString());
        SaveThis("Infos_salles", "HGoS_nombre_overdrive_combat2", NumberOverdriveSetupsCombat2[5].ToString());

        float totalCombat1 = 0;
        float totalCombat2 = 0;
        float pourcentageUtilisationEquipementSpecialCombat1 = 0;
        float pourcentageUtilisationEquipementSpecialCombat2 = 0;

        for (int i = 0; i < 6; i++)
        {
            totalCombat1 += CompteurUtilisationEquipementCombat1[i];
            totalCombat2 += CompteurUtilisationEquipementCombat2[i];
        }
        pourcentageUtilisationEquipementSpecialCombat1 = CompteurUtilisationEquipementCombat1[4] + CompteurUtilisationEquipementCombat1[5];
        pourcentageUtilisationEquipementSpecialCombat2 = CompteurUtilisationEquipementCombat2[4] + CompteurUtilisationEquipementCombat2[5];
        SaveThis("Infos_salles", "Pourcentage_utilisation_equipement_special_combat1", pourcentageUtilisationEquipementSpecialCombat1 + " utilisations pour " + totalCombat1 + " actions totales" + "(" + (pourcentageUtilisationEquipementSpecialCombat1 / totalCombat1) * 100 + "%)");
        SaveThis("Infos_salles", "Pourcentage_utilisation_equipement_special_combat2", pourcentageUtilisationEquipementSpecialCombat2 + " utilisations pour " + totalCombat2 + " actions totales" + "(" + (pourcentageUtilisationEquipementSpecialCombat2 / totalCombat2) * 100 + "%)");

        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_0_adverse_a_été_visée_par_le_joueur_premier_combat", SalleAdverseViseePremierCombat[0].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_1_adverse_a_été_visée_par_le_joueur_premier_combat", SalleAdverseViseePremierCombat[1].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_2_adverse_a_été_visée_par_le_joueur_premier_combat", SalleAdverseViseePremierCombat[2].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_3_adverse_a_été_visée_par_le_joueur_premier_combat", SalleAdverseViseePremierCombat[3].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_0_nest_a_été_visée_par_le_joueur_premier_combat", SalleNestViseePremierCombat[0].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_1_nest_a_été_visée_par_le_joueur_premier_combat", SalleNestViseePremierCombat[1].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_2_nest_a_été_visée_par_le_joueur_premier_combat", SalleNestViseePremierCombat[2].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_3_nest_a_été_visée_par_le_joueur_premier_combat", SalleNestViseePremierCombat[3].ToString());

        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_0_adverse_a_été_visée_par_le_joueur_deuxieme_combat", SalleAdverseViseeDeuxiemeCombat[0].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_1_adverse_a_été_visée_par_le_joueur_deuxieme_combat", SalleAdverseViseeDeuxiemeCombat[1].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_2_adverse_a_été_visée_par_le_joueur_deuxieme_combat", SalleAdverseViseeDeuxiemeCombat[2].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_3_adverse_a_été_visée_par_le_joueur_deuxieme_combat", SalleAdverseViseeDeuxiemeCombat[3].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_0_nest_a_été_visée_par_le_joueur_deuxieme_combat", SalleNestViseeDeuxiemeCombat[0].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_1_nest_a_été_visée_par_le_joueur_deuxieme_combat", SalleNestViseeDeuxiemeCombat[1].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_2_nest_a_été_visée_par_le_joueur_deuxieme_combat", SalleNestViseeDeuxiemeCombat[2].ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_la_salle_3_nest_a_été_visée_par_le_joueur_deuxieme_combat", SalleNestViseeDeuxiemeCombat[3].ToString());

        SaveThis("Infos_joueurs", "Nombre_de_fois_que_le_joueur_a_visé_une_salle_adverse_qui_préparait_une_action_premier_combat", NombreActionEffectueSurSalleQuiPreparaitUneActionPremierCombat.ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_le_joueur_a_visé_une_salle_adverse_qui_préparait_une_action_deuxieme_combat", NombreActionEffectueSurSalleQuiPreparaitUneActionDeuxiemeCombat.ToString());

        SaveThis("Infos_joueurs", "Nombre_de_fois_que_le_joueur_a_visé_une_salle_du_nest_qui_recevait_une_attaque_premier_combat", NombreActionEffectueSurSalleQuiRecevaitUneActionPremierCombat.ToString());
        SaveThis("Infos_joueurs", "Nombre_de_fois_que_le_joueur_a_visé_une_salle_du_nest_qui_recevait_une_attaque_deuxieme_combat", NombreActionEffectueSurSalleQuiRecevaitUneActionDeuxiemeCombat.ToString());
    }

    public void SaveThis(string groupeName, string elementName, string valueInt)
    {

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
