//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections.Generic;

//public class Gop2 : MonoBehaviour
//{
//    public TMP_Text diviseurText;
//    public TMP_Text dividendeText;
//    public TMP_Text resultText;
//    public TMP_Text soustractionText;
//    public TMP_Text resteText;

//    public GameObject diviseurGO;
//    public GameObject dividendeGO;
//    public GameObject resultGO;
//    public GameObject soustractionGO;
//    public GameObject resteGO;

//    [Header("Options de r�ponse")]
//    public TMP_Text[] optionTexts; // Assigne Option1, Option2, Option3... dans l'inspecteur

//    // Nouvelles variables pour g�rer 3 champs
//    private Dictionary<string, string> correctAnswers = new Dictionary<string, string>();
//    private List<DropZone2> activeDropZones = new List<DropZone2>();

//    void Start()
//    {
//        GenerateDivision();
//        HideThreeFields(); // Nouvelle m�thode pour cacher 3 champs
//        GenerateOptions(); // M�thode modifi�e
//    }

//    void GenerateDivision()
//    {
//        int diviseur = Random.Range(2, 10);
//        int result = Random.Range(2, 10);
//        int reste = Random.Range(0, diviseur);
//        int produit = diviseur * result;
//        int dividende = produit + reste;
//        int soustraction = produit;

//        // Affecter les objets 
//        diviseurText = diviseurGO.GetComponentInChildren<TMP_Text>();
//        dividendeText = dividendeGO.GetComponentInChildren<TMP_Text>();
//        resultText = resultGO.GetComponentInChildren<TMP_Text>();
//        soustractionText = soustractionGO.GetComponentInChildren<TMP_Text>();
//        resteText = resteGO.GetComponentInChildren<TMP_Text>();

//        // Affecter les textes
//        diviseurText.text = diviseur.ToString();
//        dividendeText.text = dividende.ToString();
//        resultText.text = result.ToString();
//        soustractionText.text = soustraction.ToString();
//        resteText.text = reste.ToString();
//    }

//    void HideThreeFields()
//    {
//        // Nettoyer les donn�es pr�c�dentes
//        correctAnswers.Clear();
//        activeDropZones.Clear();

//        // Cacher et configurer les 3 champs
//        SetupDropZone(resultText, "Result");
//        SetupDropZone(soustractionText, "Soustraction");
//        SetupDropZone(resteText, "Reste");
//    }

//    void SetupDropZone(TMP_Text textComponent, string fieldName)
//    {
//        GameObject textGO = textComponent.gameObject;
//        Transform parent = textGO.transform.parent;

//        // Sauvegarder la bonne r�ponse
//        correctAnswers[fieldName] = textComponent.text;

//        // Ajouter la dropzone
//        DropZone2 dropZone = parent.gameObject.AddComponent<DropZone2>();
//        dropZone.Initialize(textComponent, fieldName); // M�thode modifi�e
//        activeDropZones.Add(dropZone);

//        // Changer le texte en "?"
//        textComponent.text = "?";

//        // Effet visuel
//        ShowDropZoneVisual(textComponent);
//        ShowImage(textComponent);
//    }

//    void ShowDropZoneVisual(TMP_Text textComponent)
//    {
//        textComponent.color = Color.red;
//    }

//    void GenerateOptions()
//    {
//        // Cr�er une liste de toutes les bonnes r�ponses
//        List<string> allCorrectAnswers = new List<string>(correctAnswers.Values);

//        // M�langer les bonnes r�ponses pour les distribuer al�atoirement
//        for (int i = 0; i < allCorrectAnswers.Count; i++)
//        {
//            string temp = allCorrectAnswers[i];
//            int randomIndex = Random.Range(i, allCorrectAnswers.Count);
//            allCorrectAnswers[i] = allCorrectAnswers[randomIndex];
//            allCorrectAnswers[randomIndex] = temp;
//        }

//        // Assigner les options
//        for (int i = 0; i < optionTexts.Length; i++)
//        {
//            if (i < allCorrectAnswers.Count)
//            {
//                // Assigner une bonne r�ponse
//                optionTexts[i].text = allCorrectAnswers[i];
//            }
//            else
//            {
//                // G�n�rer une mauvaise r�ponse
//                int wrongAnswer;
//                do
//                {
//                    wrongAnswer = Random.Range(1, 20);
//                } while (allCorrectAnswers.Contains(wrongAnswer.ToString()));

//                optionTexts[i].text = wrongAnswer.ToString();
//            }

//            // S'assurer que chaque option a un composant Draggable
//            if (optionTexts[i].GetComponent<Draggable>() == null)
//            {
//                optionTexts[i].gameObject.AddComponent<Draggable>();
//            }
//        }
//    }

//    void ShowImage(TMP_Text textField)
//    {
//        Image img = textField.transform.parent.GetComponentInChildren<Image>(true);
//        if (img != null)
//            img.gameObject.SetActive(true);
//    }

//    public void OnSubmitClicked()
//    {
//        CheckAnswer();
//    }

//    public void CheckAnswer()
//    {
//        bool allCorrect = true;
//        int correctCount = 0;

//        foreach (DropZone2 dropZone in activeDropZones)
//        {
//            if (dropZone.targetText != null)
//            {
//                string placedAnswer = dropZone.targetText.text;
//                string correctAnswer = correctAnswers[dropZone.fieldName];

//                if (placedAnswer == "?")
//                {
//                    Debug.Log($"Veuillez placer une r�ponse pour {dropZone.fieldName}!");
//                    allCorrect = false;
//                }
//                else if (placedAnswer == correctAnswer)
//                {
//                    correctCount++;
//                    // Changer la couleur en vert pour indiquer que c'est correct
//                    dropZone.targetText.color = Color.green;
//                }
//                else
//                {
//                    allCorrect = false;
//                    // Garder la couleur rouge pour indiquer l'erreur
//                    dropZone.targetText.color = Color.red;
//                }
//            }
//        }

//        // Afficher le r�sultat
//        if (allCorrect && correctCount == 3)
//        {
//            Debug.Log("Parfait ! Toutes les r�ponses sont correctes !");
//        }
//        else
//        {
//            Debug.Log($"Vous avez {correctCount}/3 r�ponses correctes. Continuez !");
//        }
//    }

//    // M�thode pour recommencer
//    public void RestartLevel()
//    {
//        // Nettoyer les dropzones existantes
//        foreach (DropZone2 dropZone in activeDropZones)
//        {
//            if (dropZone != null)
//            {
//                dropZone.ClearDropZone();
//                Destroy(dropZone);
//            }
//        }

//        // Remettre toutes les options � leur place
//        foreach (TMP_Text option in optionTexts)
//        {
//            Draggable draggable = option.GetComponent<Draggable>();
//            if (draggable != null)
//            {
//                draggable.ReturnToOrigin();
//            }
//        }

//        // Regenerer le probl�me
//        GenerateDivision();
//        HideThreeFields();
//        GenerateOptions();
//    }
//}