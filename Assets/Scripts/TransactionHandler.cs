using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionHandler : MonoBehaviour
{
    void Start() {
        this.gameObject.SetActive(false);
    }
    
    public void CloseTransaction()
    {
        this.gameObject.SetActive(false);

    }
}
