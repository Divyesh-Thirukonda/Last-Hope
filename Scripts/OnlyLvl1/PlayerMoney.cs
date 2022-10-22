using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoney : MonoBehaviour
{
    public float money;
    public Text moneyText;

    


    void Start()
    {
        

        money = 100f;
        moneyText.text = money.ToString();
    }

    

    public void addMoney(float moneyToAdd)
    {
        money += moneyToAdd;
        moneyText.text = money.ToString();
    }

    public void subtractMoney(float moneyToSubtract)
    {
        if (money - moneyToSubtract < 0f)
        {
            //HAHA FUNNY
        } else
        {
            money -= moneyToSubtract;
            moneyText.text = money.ToString();
        }
    }

    public void ArmorMoneyGoDown()
    {
        subtractMoney(5f);
    }

    public void ArmorMoneyGoUp()
    {
        addMoney(5f);
    }


    public void SwordMoneyGoDown()
    {
        subtractMoney(10f);
    }

    public void SwordMoneyGoUp()
    {
        addMoney(10f);
    }
}
