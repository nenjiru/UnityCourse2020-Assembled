using UnityEngine;
using UnityEngine.UI;

public class HitCounter : MonoBehaviour
{
    public Text counter;
    public int total = 0;
    private GameObject[] targets;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");

        for (int i = 0; i < targets.Length; i++)
        {
            HitTarget item = targets[i].GetComponent<HitTarget>();
            item.Hit += OnHit;
        }
    }

    private void OnHit(int score)
    {
        total += score;
        counter.text = "HIT " + total;
    }

    public void OnResetButton()
    {
        total = 0;
        counter.text = "HIT 0";
    }
}

