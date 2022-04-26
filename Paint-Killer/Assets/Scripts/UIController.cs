using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using InputManager;
//using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public GameObject leftPage;
    public GameObject rightPage;

    public float speed;

    private RectTransform rt_left;
    private RectTransform rt_right;

    bool active = false;
    bool inCenter_left = false;
    bool inCenter_right = false;

    Vector3 lastPos_left;
    Vector3 lastPos_right;

    public Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        rt_left  = leftPage.GetComponent<RectTransform>();
        rt_right = rightPage.GetComponent<RectTransform>();

        lastPos_left  = leftPage.transform.position;
        lastPos_right = rightPage.transform.position;

        newPos = new Vector3(150, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            if (Controller.Action4(Button.DOWN, 1) || Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                StartCoroutine(MoveLeftPage(!inCenter_left));
            }
            else if (Controller.Action5(Button.DOWN) || Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                StartCoroutine(MoveRightPage(!inCenter_right));
            }
        }
    }

    IEnumerator MoveLeftPage(bool moveOut)
    {
        active = true;
        newPos = new Vector3(rt_left.transform.position.x + 200, rt_left.transform.position.y, 0);

        //if (newPos.x < 0) { newPos.x = newPos.x * -1; }
        while (true)
        {
            if (moveOut)
            {
                rt_left.position = Vector3.Lerp(rt_left.position, newPos, speed * Time.deltaTime);
                if (Vector3.Distance(rt_left.position, newPos) < 0.1f) { break; }             
            }else
            {
                rt_left.position = Vector3.Lerp(rt_left.position, lastPos_left, speed * Time.deltaTime);
                if (Vector3.Distance(rt_left.position, lastPos_left) < 0.1f) { break; }
            }

            yield return new WaitForEndOfFrame();
        }

        inCenter_left = !inCenter_left;
        active = false;
    }

    IEnumerator MoveRightPage(bool moveOut)
    {
        active = true;
        newPos = new Vector3(rt_right.transform.position.x - 200, rt_left.transform.position.y, 0);

        //if (newPos.x > 0) { newPos.x = newPos.x * -1; } 
        while (true)
        {
            if (moveOut)
            {
                rt_right.position = Vector3.Lerp(rt_right.position, newPos, speed * Time.deltaTime);
                if (Vector3.Distance(rt_right.position, newPos) < 0.1f) { break; }
            }
            else
            {
                rt_right.position = Vector3.Lerp(rt_right.position, lastPos_right, speed * Time.deltaTime);
                if (Vector3.Distance(rt_right.position, lastPos_right) < 0.1f) { break; }
            }

            yield return new WaitForEndOfFrame();
        }

        inCenter_right = !inCenter_right;
        active = false;
    }
}
