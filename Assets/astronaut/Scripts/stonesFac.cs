using TMPro;
using UnityEngine;

public class stonesFac : MonoBehaviour
{
    public GameObject stonePrefab; // Un prefab qui contient les deux pierres (haut et bas)
    public float spawnInterval = 5f;
    public float spawnX = 10f;
    public float despawnX = -15f;

    private GameObject lastStone;

    void Start()
    {
        // On cr�e une au d�but
        SpawnStone();
    }

    void Update()
    {
        if (lastStone != null && lastStone.transform.position.x < despawnX)
        {
            SpawnStone();
        }
    }

    void SpawnStone()
    {
        Vector3 spawnPos = new Vector3(spawnX, 0, 0); // Y peut �tre ajust� pour random height
        GameObject newStone = Instantiate(stonePrefab, spawnPos, Quaternion.identity);

        TextMeshPro topStone = newStone.transform.Find("Top Stone/Text").GetComponent<TextMeshPro>();
        TextMeshPro BottomStone = newStone.transform.Find("Buttom Stone/Text").GetComponent<TextMeshPro>();


        OpGenerator opGen;
        opGen = GameObject.FindGameObjectWithTag("OpLogic").GetComponent<OpGenerator>();
        
        if (opGen != null)
        {
            opGen.GeneratorOperation(topStone, BottomStone);
        }
        // Mettre � jour la derni�re pierre pour surveiller quand elle sort
        lastStone = newStone;
    }
}
