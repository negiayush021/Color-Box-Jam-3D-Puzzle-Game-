using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;

    public float maxLimit;
    public float minLimit;

    public bool OnlyRightDir;
    public bool OnlyLeftDir;
    public bool OnlyDownDir;
    public bool OnlyUpDir;

    public GameObject this_block_Prefab;
    bool isDragging = false;
    bool wallTouched = false;

    public enum MoveAxis
    {
        X,Z
    }
    public MoveAxis moveAxis;

    public enum BlockColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Pink,
        Orange,
        Violet,
        Grey
    }
    public BlockColor blockcolor;
    Vector3 GetPointerPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return Touchscreen.current.primaryTouch.position.ReadValue();
        return Mouse.current.position.ReadValue();
    }

    bool PointerPressedThisFrame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;
        if (Mouse.current != null)
            return Mouse.current.press.wasPressedThisFrame;

        return false;
    }

    bool PointerHeld()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return true;
        if (Mouse.current != null)
            return Mouse.current.press.isPressed;

        return false;
    }

    bool PointerReleasedThisFrame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            return true;
        if (Mouse.current != null)
            return Mouse.current.press.wasReleasedThisFrame;

        return false;
    }



    private void Start()
    {
        cam = Camera.main;
        transform.GetChild(1).localPosition = new Vector3(0, .45f, 0);


        if (OnlyRightDir)
        {
            minLimit = transform.position.x;
        }
        else if (OnlyLeftDir)
        {
            maxLimit = transform.position.x;
        }

        if (OnlyDownDir)
            maxLimit = transform.position.z;
        else if(OnlyUpDir)
            minLimit = transform.position.z;
    }


    private void Update()
    {
        if (PointerPressedThisFrame())
        {
            Ray ray = cam.ScreenPointToRay(GetPointerPosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    //closing tutorial
                    if (GameManager.Instance.current_level == 1)
                        GameManager.Instance.close_tutorial_level1();
                    else if (GameManager.Instance.current_level == 5)
                        GameManager.Instance.close_tutorial_level5();
                    else if (GameManager.Instance.current_level == 11)
                        GameManager.Instance.close_tutorial_level11();


                    this.gameObject.transform.GetChild(0).tag = "Untagged";
                    offset = transform.position - hit.point;
                    isDragging = true;

                    // using hammer power
                    if (CanvasManager.instance.IsThirdPowerUse)
                    {
                        if (this.gameObject.tag == "Obstacle")
                            StartCoroutine(Destroying_Obstacle());
                        else
                            print("its not an obstacle");
                    }


                    if (CanvasManager.instance.canVibrate)
                    {
                        GameManager.Instance.LightImpactHaptic();
                    }

                    if (GameManager.Instance.current_level == 5)
                    {
                        GameManager.Instance.close_tutorial_level5();
                    }

                    if (GameManager.Instance.current_level == 11)
                    {
                        GameManager.Instance.close_tutorial_level11();
                    }

                    if (!CanvasManager.instance.freezeCountDown)
                    {
                        Timer.Instance.counting_down = true;
                    }

                }

            }

            
        }

        if (PointerHeld() && isDragging)
        {
            Ray ray = cam.ScreenPointToRay(GetPointerPosition());
            Plane plane = new Plane(Vector3.up, transform.position);

            if (plane.Raycast(ray , out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                float target;

                if (moveAxis == MoveAxis.X)
                    target = worldPoint.x + offset.x;
                else
                    target = worldPoint.z + offset.z;
                target = Mathf.Clamp(target, minLimit, maxLimit);

                float current;

                if (moveAxis == MoveAxis.X)
                    current = transform.position.x;
                else
                    current = transform.position.z;

                float distanceToMove = target - current;
                float step = Mathf.Sign(distanceToMove) * 0.1f; // step size (adjust if needed)

                bool blocked = false;

                // Sweep test: step towards targetX and stop if we find block in the way
                while (Mathf.Abs(current - target) > 0.01f)
                {
                    current += step;

                    Vector3 testPos;
                    if (moveAxis == MoveAxis.X)
                        testPos = new Vector3(current, transform.position.y, transform.position.z);
                    else
                        testPos = new Vector3(transform.position.x, transform.position.y, current);

                    foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block"))
                        {
                            if (Vector3.Distance(testPos, block.transform.position) < 0.9f)
                            {
                                blocked = true;
                                break; // found block in path -> stop movement
                            }

                        }

                    if (blocked) break; // exit while loop only

                    // If stepping beyond target, clamp to target
                    if ((step > 0 && current > target) || (step < 0 && current < target))
                    {
                        current = target;
                        break;
                    }
                }

                if (!blocked) // only move if not blocked
                {
                    // Move to last safe position
                    if (moveAxis == MoveAxis.X)
                        transform.position = new Vector3(current, transform.position.y, transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x, transform.position.y, current);

                }


            }

        }

        if (PointerReleasedThisFrame())
        {
            transform.GetChild(0).tag = "Block";
            if (wallTouched == false)
            {
                snapToGrid();
            }
            isDragging = false;
        }
    }


    IEnumerator Destroying_Obstacle()
    {
        GameObject hammer = Instantiate(CanvasManager.instance.Hammer_prefab, new Vector3(transform.position.x, transform.position.y+ 1, transform.position.z - .5f) , Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        MusicManager.instance.PlayClip(17);
        yield return new WaitForSeconds(.2f);
        Destroy(hammer);
        GameObject smoke = Instantiate(CanvasManager.instance.smoke_effect , new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        Destroy(this.gameObject);
        print("destroyed");
        Timer.Instance.counting_down = true;
        CanvasManager.instance.IsThirdPowerUse = false;
        CanvasManager.instance.PowerUseTxt_Object.SetActive(false);
        GameManager.Instance.coolDownIMG.SetActive(false);
    }

  

    void snapToGrid()
    {
        float SnappedX = Mathf.Round(transform.position.x);
        float SnappedZ = Mathf.Round(transform.position.z);

        transform.position = new Vector3(SnappedX, transform.position.y, SnappedZ);

        if (OnlyRightDir)
        {
            minLimit = transform.position.x;
        }
        else if (OnlyLeftDir)
        {
            maxLimit = transform.position.x;
        }

        if (OnlyDownDir)
            maxLimit = transform.position.z;
        else if (OnlyUpDir)
            minLimit = transform.position.z;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<wall>().wallcolor == blockcolor)
        {
            GameManager.Instance.no_of_blocks--;
            wallTouched = true;
            GameManager.Instance.update_Queue(this_block_Prefab);
            GameManager.Instance.anim = transform.GetChild(0).GetComponent<Animator>();
            GameManager.Instance.anim.SetTrigger("Play1to0");
            Destroy(this.gameObject, 1);
        }

    }


}
