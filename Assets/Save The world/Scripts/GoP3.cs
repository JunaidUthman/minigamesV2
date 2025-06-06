using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoP3 : MonoBehaviour
{


    [Header("UI Elements")]
    public TMP_Text dividendeText;
    public TMP_Text diviseurText;
    public TMP_Text quotientText;
    public TMP_Text resteText;
    public TMP_Text soustr1Text;
    public TMP_Text soustrResultText;
    public TMP_Text soustr2Text;
    public TMP_Text[] optionTexts; // Les options � glisser



    private List<DropZone> activeDropZones = new List<DropZone>();
    private Dictionary<DropZone, string> correctAnswers = new Dictionary<DropZone, string>();

    void Start()
    {
        GenerateProblem();
    }

    void GenerateProblem()
    {
        // Nettoyer les dropzones pr�c�dentes
        //ClearAllDropZones();

        // G�n�rer une division simple
        int diviseur = Random.Range(2, 15); // diviseur simple

        int factor1 = Random.Range(4, 15);  // multiple 1
        int factor2 = Random.Range(2, 10);  // multiple 2

        int produit1 = diviseur * factor1;
        int produit2 = diviseur * factor2;

        int quotient = factor1 + factor2;

        int reste = Random.Range(0, diviseur); // peut �tre 0 ou plus, mais < diviseur

        int dividu = produit1 + produit2 + reste;

        // Assignation aux champs TMP_Text
        diviseurText.text = diviseur.ToString();
        dividendeText.text = dividu.ToString();
        quotientText.text = quotient.ToString();
        resteText.text = reste.ToString();

        soustr1Text.text = produit1.ToString(); // premi�re soustraction
        soustrResultText.text = (dividu - produit1).ToString(); // reste interm�diaire
        soustr2Text.text = produit2.ToString(); // deuxi�me soustraction

        // 3 champs � cacher 
        List<TMP_Text> availableFields = new List<TMP_Text>
        {
            quotientText,
            resteText,
            soustr1Text,
            soustrResultText,
            soustr2Text
        };

        //M�langer la liste
        for (int i = 0; i < availableFields.Count; i++)
        {
            TMP_Text temp = availableFields[i];
            int randomIndex = Random.Range(i, availableFields.Count);
            availableFields[i] = availableFields[randomIndex];
            availableFields[randomIndex] = temp;
        }

        // Prendre les 3 premiers pour les cacher
        for (int i = 0; i < 4; i++)
        {
            string correctValue = availableFields[i].text;
            SetupDropZone(availableFields[i], correctValue);
        }

        // G�n�rer les options avec les bonnes r�ponses
        GenerateOptions();
    }

    void SetupDropZone(TMP_Text textComponent, string correctAnswer)
    {
        GameObject textGO = textComponent.gameObject;
        Transform parent = textGO.transform.parent;

        // Ajouter la dropzone
        DropZone dropZone = parent.gameObject.AddComponent<DropZone>();
        dropZone.Initialize(textComponent);

        // Changer le texte en "?"
        textComponent.text = "?";

        // Effet visuel pour indiquer une zone de drop
        textComponent.color = Color.red;

        // Enregistrer la dropzone et sa bonne r�ponse
        activeDropZones.Add(dropZone);
        correctAnswers[dropZone] = correctAnswer;
    }

    void GenerateOptions()
    {
        // Prendre les 4 bonnes r�ponses
        List<string> trueAnswers = new List<string>(correctAnswers.Values);
        List<string> allOptions = new List<string>(trueAnswers);

        //afficher les bonnes r�ponses dans les options
        for (int i = 0; i < trueAnswers.Count && i < optionTexts.Length; i++)
        {

            Debug.Log(trueAnswers[i]);

        }


        // Ajouter 2 mauvaises r�ponses diff�rentes
        while (allOptions.Count < 6)
        {
            int baseValue = int.Parse(trueAnswers[Random.Range(0, trueAnswers.Count)]);
            int wrong = baseValue + Random.Range(-10, 11);

            // S'assurer qu'elle est positive et non d�j� incluse
            if (wrong >= 0 && !allOptions.Contains(wrong.ToString()))
            {
                allOptions.Add(wrong.ToString());
            }
        }

        // M�langer les options
        for (int i = 0; i < allOptions.Count; i++)
        {
            int rand = Random.Range(i, allOptions.Count);
            (allOptions[i], allOptions[rand]) = (allOptions[rand], allOptions[i]);
        }

        // Assigner aux textes d�option (suppos� avoir 6 TMP_Text)
        for (int i = 0; i < optionTexts.Length && i < allOptions.Count; i++)
        {
            optionTexts[i].text = allOptions[i];

            if (optionTexts[i].GetComponent<Draggable>() == null)
            {
                optionTexts[i].gameObject.AddComponent<Draggable>();
            }
        }
    }


    public void OnSubmitClicked()
    {
        CheckAnswers();
    }

    void CheckAnswers()
    {
        int correctCount = 0;
        bool allFilled = true;

        foreach (DropZone dropZone in activeDropZones)
        {
            if (dropZone.targetText.text == "?")
            {
                allFilled = false;
                break;
            }

            if (dropZone.targetText.text == correctAnswers[dropZone])
            {
                correctCount++;
                dropZone.targetText.color = Color.green;
            }
            else
            {
                dropZone.targetText.color = Color.red;
            }
        }

        if (!allFilled)
        {
            Debug.Log("Veuillez remplir tous les champs avant de soumettre !");
            return;
        }

        if (correctCount == activeDropZones.Count)
        {
            Debug.Log("Toutes les r�ponses sont correctes !");
        }
        else
        {
            Debug.Log("Certaines r�ponses sont incorrectes, essayez encore !");
        }
        Debug.Log($"Nombre de r�ponses correctes : {correctCount} sur {activeDropZones.Count}");
    }




    //void ClearAllDropZones()
    //{
    //    foreach (DropZone dropZone in activeDropZones)
    //    {
    //        if (dropZone != null)
    //        {
    //            dropZone.ClearDropZone();
    //            Destroy(dropZone);
    //        }
    //    }

    //    activeDropZones.Clear();
    //    correctAnswers.Clear();

    //    // Remettre toutes les options � leur place
    //    foreach (TMP_Text option in optionTexts)
    //    {
    //        Draggable draggable = option.GetComponent<Draggable>();
    //        if (draggable != null)
    //        {
    //            draggable.ReturnToOrigin();
    //        }
    //    }
    //}


}