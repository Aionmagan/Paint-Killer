using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

public class DisableCamUI : MonoBehaviour
{
    public GameObject[] obj_to_disable;
    // Update is called once per frame
    void Update()
    {

        for (int i = 1; i < 5; ++i)
        {
            if (Controller.Action0(Button.DOWN, i))
            {
                for (int j = 0; j < obj_to_disable.Length; ++j)
                    Destroy(obj_to_disable[j]);

                Destroy(this.gameObject);
            }
        }

    }
}
