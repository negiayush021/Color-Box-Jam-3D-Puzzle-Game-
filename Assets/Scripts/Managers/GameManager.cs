using Solo.MOST_IN_ONE;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] ParticleSystem destroy;
   

     public GameObject coolDownPlane;
    public GameObject coolDownIMG;

    public Transform Ready_Queue;
    public Transform[] places;
    public bool[] places_empty = new bool[8];
    public int no_of_places;
    public int no_of_places_occupied = 0;

    public int no_of_blocks;

    public Animator anim;

    [SerializeField] GameObject musicManagerObject;
    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < 20; i++)
        {
            if (PlayerPrefs.GetInt("Level cleared") == i)
            {
                musicManagerObject.SetActive(true);
            }
        }

    }

   

    private void Start()
    {
        Application.targetFrameRate = 60;

        Shuffle(spawnPool);

        StartCoroutine(spawnQueue());
        for (int i = 0; i < places.Length; i++)
        {
            places_empty[i] = true;
        }

        if (PlayerPrefs.GetInt("Level 1 tuto") == 0)
        {
            if (current_level == 1)
            {
                open_tutorial_level1();
            }

        }
        if (PlayerPrefs.GetInt("Level 5 tuto") == 0)
        {
            if (current_level == 5)
            {
                open_tutorial_level5();
            }
        }

        if (PlayerPrefs.GetInt("Level 11 tuto")== 0)
        {
            if (current_level == 11)
            {
                open_tutorial_level11();
            }
        }

        if (PlayerPrefs.GetInt("FreezePowtuto") == 0)
        {
            if (current_level == 4)
            {
                CanvasManager.instance.freezeTuto.SetActive(true);
            }
        }
        else
        {
            if (current_level == 4)
            {
                CanvasManager.instance.freezeTuto.SetActive(false);
            }
           
        }

        if (PlayerPrefs.GetInt("FillQueuePowTuto") == 0)
        {
            if (current_level == 9)
            {
                CanvasManager.instance.FillQueueTuto.SetActive(true);
            }
        }
        else
        {
            if (current_level == 9)
            {
                CanvasManager.instance.FillQueueTuto.SetActive(false);
            }

        }


        if (PlayerPrefs.GetInt("FillQueuePowTuto") == 0)
        {
            if (current_level == 9)
            {
                CanvasManager.instance.FillQueueTuto.SetActive(true);
            }
        }
        else
        {
            if (current_level == 9)
            {
                CanvasManager.instance.FillQueueTuto.SetActive(false);
            }

        }


        if (PlayerPrefs.GetInt("DestroyObstaclePowTuto") == 0)
        {
            if (current_level == 14)
            {
                CanvasManager.instance.DestroyObstacleTuto.SetActive(true);
            }
        }
        else
        {
            if (current_level == 14)
            {
                CanvasManager.instance.DestroyObstacleTuto.SetActive(false);
            }

        }
    } 


    public GameObject tutorial_UI;
    public GameObject plane_for_tutorial;

     void open_tutorial_level1()
    {
        tutorial_UI.SetActive(true);
        plane_for_tutorial.SetActive(true);
    }
    public void close_tutorial_level1()
    {
        tutorial_UI.SetActive(false);
        plane_for_tutorial.SetActive(false);
        PlayerPrefs.SetInt("Level 1 tuto", 1);

    }


    void open_tutorial_level5()
    {
        tutorial_UI.SetActive(true);
        plane_for_tutorial.SetActive(true);
    }
    public void close_tutorial_level5()
    {
        tutorial_UI.SetActive(false);
        plane_for_tutorial.SetActive(false);
        PlayerPrefs.SetInt("Level 5 tuto", 1);
    }


    void open_tutorial_level11()
    {
        tutorial_UI.SetActive(true);
        //plane_for_tutorial.SetActive(true);
    }
    public void close_tutorial_level11()
    {
        tutorial_UI.SetActive(false);
       // plane_for_tutorial.SetActive(false);
        PlayerPrefs.SetInt("Level 11 tuto", 1);
    }


    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    [SerializeField] GameObject GameOverScreen;
   
    public void closeGameOverScreen()
    {
        MusicManager.instance.PlayClip(6);
        Timer.Instance.counting_down = true;
        GameOverScreen.SetActive(false);
    }

    //this is for restart window
    [SerializeField] GameObject heartloseanim;

    //this is for runOutOfTime window
    [SerializeField] GameObject heartloseanim2;

    //this is for OutOfSpace window
    [SerializeField] GameObject heartloseanim3;
    public void retry()
    {
        fadePanel.SetActive(true);
        StartCoroutine(FadeAndRestart());
    }

    public void restart()
    {
        Timer.Instance.counting_down = false;
        GameOverScreen.SetActive(true);
        MusicManager.instance.PlayClip(5);
    }

    IEnumerator FadeAndRestart()
    {
        heartloseanim.SetActive(true);
        heartloseanim2.SetActive(true);
        heartloseanim3 .SetActive(true);
        MusicManager.instance.PlayClip(7);
        yield return new WaitForSeconds(.5f);
        
        // Fade from 0 to 1 alpha
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = fadePanelimg.color;
            c.a = t;
            fadePanelimg.color = c;
            yield return null;


        }

        // Make sure it ends at full opacity
        Color finalColor = fadePanelimg.color;
        finalColor.a = 1;
        fadePanelimg.color = finalColor;
        

        // Optionally restart level after fade
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    private void Update()
    {

        Violet_queue_exist = GameObject.FindGameObjectsWithTag("Violet Queue").Length > 0;
        blue_queue_exist = GameObject.FindGameObjectsWithTag("Blue Queue").Length > 0;
        red_queue_exist = GameObject.FindGameObjectsWithTag("Red Queue").Length > 0;
        green_queue_exist = GameObject.FindGameObjectsWithTag("Green Queue").Length > 0;
        yellow_queue_exist = GameObject.FindGameObjectsWithTag("Yellow Queue").Length > 0;
        pink_queue_exist = GameObject.FindGameObjectsWithTag("Pink Queue").Length > 0;
        Orange_queue_exist = GameObject.FindGameObjectsWithTag("Orange Queue").Length > 0;


        if ((no_of_places_occupied < places.Length - 1))
        {
            red_alert_off();
        }
    }

    public void update_Queue(GameObject Block)
    {
        
        StartCoroutine(Updating_Queue(Block));   

    }


    GameObject cube_red;
    GameObject cube_blue;
    GameObject cube_green;
    GameObject cube_yellow;
    GameObject cube_pink;
    GameObject cube_violet;
    GameObject cube_Orange;

    IEnumerator Updating_Queue(GameObject Block)
    {
        
        yield return new WaitForSeconds(.4f);
       // yield return null;
        if (no_of_places_occupied < places.Length)
        {
            for (int i = 0; i < places.Length; i++)
            {
                if (places_empty[i] == true)
                {
                    if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Red)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_red = Instantiate(Block, places[i].position , Quaternion.Euler(90,0,0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_red.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        
                        
                        cube_red.transform.SetParent(Ready_Queue.transform, false);
                        cube_red.transform.position = places[i].position + new Vector3(0, .25f, 0);
                        
                        
                      
                        StartCoroutine(TransferBlock(cube_red));
                        break;
                    }
                    else if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Blue)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_blue = Instantiate(Block, places[i].position, Quaternion.Euler(90, 0, 0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_blue.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                       
                        cube_blue.transform.SetParent(Ready_Queue.transform, false);
                        cube_blue.transform.position = places[i].position + new Vector3(0, 0.25f, 0);
                        
                        
                       
                        StartCoroutine(TransferBlock(cube_blue));
                        break;

                    }
                    else if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Green)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_green = Instantiate(Block, places[i].position, Quaternion.Euler(90,0,0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_green.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                        cube_green.transform.SetParent(Ready_Queue.transform, false);
                        cube_green.transform.position = places[i].position + new Vector3(0, 0.25f, 0);
                        
                        
                       
                        StartCoroutine(TransferBlock(cube_green));
                        break;

                    }
                    else if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Yellow)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_yellow = Instantiate(Block, places[i].position, Quaternion.Euler(90, 0, 0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_yellow.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                        cube_yellow.transform.SetParent(Ready_Queue.transform, false);
                        cube_yellow.transform.position = places[i].position + new Vector3(0, 0.25f, 0);
                        
                        
                       
                        StartCoroutine(TransferBlock(cube_yellow));
                        break;

                    }
                    else if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Pink)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_pink = Instantiate(Block, places[i].localPosition, Quaternion.Euler(90, 0, 0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_pink.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                        cube_pink.transform.SetParent(Ready_Queue.transform, false);
                        cube_pink.transform.position = places[i].position + new Vector3(0, 0.25f, 0);
                        
                       
                        
                        StartCoroutine(TransferBlock(cube_pink));
                        break;

                    }
                    else if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Violet)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_violet = Instantiate(Block, places[i].position, Quaternion.Euler(90, 0, 0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_violet.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                        cube_violet.transform.SetParent(Ready_Queue.transform, false);
                        cube_violet.transform.position = places[i].position + new Vector3(0, 0.25f, 0);
                        
                        

                        StartCoroutine(TransferBlock(cube_violet));
                        break;

                    }
                    else if (Block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Orange)
                    {
                        MusicManager.instance.PlayClip(5);
                        cube_Orange = Instantiate(Block, places[i].position, Quaternion.Euler(90, 0, 0));
                        places_empty[i] = false;
                        no_of_places_occupied++;
                        cube_Orange.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                        cube_Orange.transform.SetParent(Ready_Queue.transform, false);
                        cube_Orange.transform.position = places[i].position + new Vector3(0, .25f, 0);



                        StartCoroutine(TransferBlock(cube_Orange));
                        break;
                    }
                }
            }

                



        }
    }

    public Material Red;
    public Material Blue;
    public Material Green;
    public Material Yellow;
    public Material Pink;
    public Material Violet;
    public Material Orange;

  public void transfer(GameObject block)
    {
        StartCoroutine(TransferBlock(block));
    }

    
    public IEnumerator TransferBlock(GameObject block)
    {

        
        yield return new WaitForSeconds(.2f);
        
        
            if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Pink)
            {

                if (pink_queue_exist == true)
                {
                

                    if (queue5_no_of_blocks < 3)
                    {
                        no_of_places_occupied--;
                    

                        GameObject objectwithtag = GameObject.FindGameObjectWithTag("Pink Queue");
                    
                        StartCoroutine(lerping(block, block.transform.position,
                               objectwithtag.transform.GetChild(queue5_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                               objectwithtag.transform));
                        for (int i = 0; i < places_empty.Length; i++)
                        {
                            if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                            {
                                places_empty[i] = true;
                            }
                        }

                    }


                }

            }
            else if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Red)
            {
                if (red_queue_exist)
                {
                
                    if (queue1_no_of_blocks < 3)
                    {
                        no_of_places_occupied--;

                        GameObject objectwithtag = GameObject.FindGameObjectWithTag("Red Queue");
                        StartCoroutine(lerping(block, block.transform.position,
                               objectwithtag.transform.GetChild(queue1_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                               objectwithtag.transform));
                    
                        for (int i = 0; i < places_empty.Length; i++)
                        {
                            if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                            {
                                places_empty[i] = true;
                            }
                        }
                    }

                }



            }
            else if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Blue)
            {
                if (blue_queue_exist)
                {

                    if (queue2_no_of_blocks < 3)
                    {
                        no_of_places_occupied--;

                        GameObject objectwithtag = GameObject.FindGameObjectWithTag("Blue Queue");
                    
                    StartCoroutine(lerping(block, block.transform.position,
                               objectwithtag.transform.GetChild(queue2_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                               objectwithtag.transform));

                        for (int i = 0; i < places_empty.Length; i++)
                        {
                            if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                            {
                                places_empty[i] = true;
                            }
                        }
                    }
                }



            }
            else if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Green)
            {
                if (green_queue_exist)
                {


                    if (queue3_no_of_blocks < 3)
                    {
                        no_of_places_occupied--;

                        GameObject objectwithtag = GameObject.FindGameObjectWithTag("Green Queue");
                        StartCoroutine(lerping(block, block.transform.position,
                               objectwithtag.transform.GetChild(queue3_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                               objectwithtag.transform));

                        for (int i = 0; i < places_empty.Length; i++)
                        {
                            if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                            {
                                places_empty[i] = true;
                            }
                        }
                    }
                }



            }
            else if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Yellow)
            {
                if (yellow_queue_exist)
                {


                    if (queue4_no_of_blocks < 3)
                    {
                        no_of_places_occupied--;

                        GameObject objectwithtag = GameObject.FindGameObjectWithTag("Yellow Queue");
                        StartCoroutine(lerping(block, block.transform.position,
                               objectwithtag.transform.GetChild(queue4_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                               objectwithtag.transform));

                        for (int i = 0; i < places_empty.Length; i++)
                        {
                            if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                            {
                                places_empty[i] = true;
                            }
                        }
                    }
                }



            }
        else if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Violet)
        {
            if (Violet_queue_exist)
            {


                if (queue6_no_of_blocks < 3)
                {
                    no_of_places_occupied--;

                    GameObject objectwithtag = GameObject.FindGameObjectWithTag("Violet Queue");
                    StartCoroutine(lerping(block, block.transform.position,
                           objectwithtag.transform.GetChild(queue6_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                           objectwithtag.transform));

                    for (int i = 0; i < places_empty.Length; i++)
                    {
                        if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                        {
                            places_empty[i] = true;
                        }
                    }
                }
            }



        }
        else if (block.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Orange)
        {
            if (Orange_queue_exist)
            {

                if (queue7_no_of_blocks < 3)
                {
                    no_of_places_occupied--;

                    GameObject objectwithtag = GameObject.FindGameObjectWithTag("Orange Queue");
                    StartCoroutine(lerping(block, block.transform.position,
                           objectwithtag.transform.GetChild(queue7_no_of_blocks).position + new Vector3(0, 0.5f, 0),
                           objectwithtag.transform));

                    for (int i = 0; i < places_empty.Length; i++)
                    {
                        if ((Vector3.Distance(block.transform.position, places[i].transform.position) < 0.5f))
                        {
                            places_empty[i] = true;
                        }
                    }
                }

            }



        }




        if (no_of_places == no_of_places_occupied)
        {
            //open OutOfSpace window
            CanvasManager.instance.OutOfSpace_screen.SetActive(true);
        }
            
        if (red_alert_screen.activeSelf == false)
        {
            if (no_of_places_occupied >= places.Length - 1)
            {
                StartCoroutine(red_alert_on());
                print("red alert");
            }
            
        }
        
        
        
        

    }

   

    IEnumerator Destroy_Queue()
    {
        if (no_of_blocks <= 0)
        {
            Timer.Instance.counting_down = false;
        }
        
        if (queue5_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
            GameObject queue_P = GameObject.FindGameObjectWithTag("Pink Queue");
            queue_P.tag = "Untagged";
            for (int i = 0; i<spawnPos.Length; i++)
            {
                if (Vector3.Distance(queue_P.transform.position, spawnPos[i].position) < 0.5f)
                {
                    current_index = i;
                    break;
                }
            }
            queue5_no_of_blocks = 0;
            queue5_no_of_blocks_arrived = 0;
            queue_P.transform.GetChild(3).gameObject.SetActive(true);

            queue_P.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);

            
            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }

            MusicManager.instance.PlayClip(2);
            
                Instantiate(destroy, queue_P.transform.position, Quaternion.identity);
            Destroy(queue_P);
            no_of_queue--;
             
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                
                if (cube_pink != null)
                {

                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        

                        if (Ready_Queue.transform.GetChild(k) != null)
                        {

                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Pink)
                            {

                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {

                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;


                                    StartCoroutine(TransferBlock(block));
                                    //yield return new WaitForSeconds(5f);



                                }
                            }
                        }

                    }

                }


            }
            StartCoroutine(spawnQueue());
            
        }
        else if (queue1_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
            GameObject queue_R = GameObject.FindGameObjectWithTag("Red Queue");
            queue_R.tag = "Untagged";
            for (int i = 0; i < spawnPos.Length; i++)
            {
                if (Vector3.Distance(queue_R.transform.position, spawnPos[i].position) < 0.5f)
                {
                    current_index = i;
                    break;
                }
            }

            queue1_no_of_blocks = 0;
            queue1_no_of_blocks_arrived = 0;
            queue_R.transform.GetChild(3).gameObject.SetActive(true);
           
            queue_R.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);


            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }
            MusicManager.instance.PlayClip(2);
            
            Instantiate(destroy, queue_R.transform.position, Quaternion.identity);
            Destroy(queue_R);
            no_of_queue--;
              
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                if (cube_red != null)
                {
                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        if (Ready_Queue.transform.GetChild(k) != null)
                        {
                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Red)
                            {

                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {
                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                    StartCoroutine(TransferBlock(block));
                                    // yield return new WaitForSeconds(.05f);



                                }
                            }
                        }

                    }



                }

            }
            StartCoroutine(spawnQueue());
            

        }
        else if (queue2_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
            
            GameObject queue_B = GameObject.FindGameObjectWithTag("Blue Queue");
            queue_B.tag = "Untagged";
            for (int i = 0; i < spawnPos.Length; i++)
            {
                if (Vector3.Distance(queue_B.transform.position, spawnPos[i].position) < 0.5f)
                {
                    current_index = i;
                    break;
                }
            }
            queue2_no_of_blocks = 0;
            queue2_no_of_blocks_arrived = 0;
            queue_B.transform.GetChild(3).gameObject.SetActive(true);

            queue_B.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);

            
            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }
            MusicManager.instance.PlayClip(2);
            
            Instantiate(destroy,queue_B.transform.position, Quaternion.identity);
            Destroy(queue_B);

            no_of_queue--;
              
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                if (cube_blue != null)
                {
                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        if (Ready_Queue.transform.GetChild(k) != null)
                        {
                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Blue)
                            {
                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {
                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                    StartCoroutine(TransferBlock(block));
                                    //yield return new WaitForSeconds(.2f);


                                }
                            }
                        }

                    }

                }

            }
            StartCoroutine(spawnQueue());
            

        }
         else if (queue3_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
           
            GameObject queue_G = GameObject.FindGameObjectWithTag("Green Queue");
            queue_G.tag = "Untagged";
            for (int i = 0; i < spawnPos.Length; i++)
            {
                
                if (Vector3.Distance(queue_G.transform.position, spawnPos[i].position) < .5f)
                {
                    
                    current_index = i;
                    break;
                }
            }

            queue3_no_of_blocks = 0;
            queue3_no_of_blocks_arrived = 0;
            queue_G.transform.GetChild(3).gameObject.SetActive(true);

            queue_G.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);


            
            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }
            MusicManager.instance.PlayClip(2);
            
            Instantiate(destroy, queue_G.transform.position, Quaternion.identity);
            Destroy(queue_G);
            no_of_queue--;
             
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                if (cube_green != null)
                {

                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        if (Ready_Queue.transform.GetChild(k) != null)
                        {
                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Green)
                            {
                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {
                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                    StartCoroutine(TransferBlock(block));
                                    // yield return new WaitForSeconds(.2f);

                                }
                            }
                        }

                    }

                }


            }
            StartCoroutine(spawnQueue());
            

        }
         else if (queue4_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
            GameObject queue_Y = GameObject.FindGameObjectWithTag("Yellow Queue");
            queue_Y.tag = "Untagged";
            for (int i = 0; i < spawnPos.Length; i++)
            {
                if (Vector3.Distance(queue_Y.transform.position, spawnPos[i].position) < 0.5f)
                {
                    current_index = i;
                    break;
                }
            }
            queue4_no_of_blocks = 0;
            queue4_no_of_blocks_arrived = 0;
            queue_Y.transform.GetChild(3).gameObject.SetActive(true);

            queue_Y.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);

            
            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }
            MusicManager.instance.PlayClip(2);
           
            Instantiate(destroy, queue_Y.transform.position, Quaternion.identity);
            Destroy(queue_Y);
            no_of_queue--;
              
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                if (cube_yellow != null)
                {
                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        if (Ready_Queue.transform.GetChild(k) != null)
                        {
                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Yellow)
                            {
                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {
                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                    StartCoroutine(TransferBlock(block));
                                    //yield return new WaitForSeconds(.2f);



                                }
                            }
                        }

                    }

                }


            }
            StartCoroutine(spawnQueue());
            

        }
        else if (queue6_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
            GameObject queue_V = GameObject.FindGameObjectWithTag("Violet Queue");
            queue_V.tag = "Untagged";
            for (int i = 0; i < spawnPos.Length; i++)
            {
                if (Vector3.Distance(queue_V.transform.position, spawnPos[i].position) < 0.5f)
                {
                    current_index = i;
                    break;
                }
            }

            queue6_no_of_blocks = 0;
            queue6_no_of_blocks_arrived = 0;

            queue_V.transform.GetChild(3).gameObject.SetActive(true);

            queue_V.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);
            
            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }
            MusicManager.instance.PlayClip(2);
            
            Instantiate(destroy, queue_V.transform.position, Quaternion.identity);
            Destroy(queue_V);
            no_of_queue--;
           
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                if (cube_violet != null)
                {
                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        if (Ready_Queue.transform.GetChild(k) != null)
                        {
                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Violet)
                            {
                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {
                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                    StartCoroutine(TransferBlock(block));
                                    //yield return new WaitForSeconds(.2f);


                                }
                            }
                        }

                    }

                }

            }
            StartCoroutine(spawnQueue());
           

        }
        else if (queue7_no_of_blocks_arrived == 3)
        {
            coolDownPlane.SetActive(true);
            GameObject queue_O = GameObject.FindGameObjectWithTag("Orange Queue");
            queue_O.tag = "Untagged";
            for (int i = 0; i < spawnPos.Length; i++)
            {
                if (Vector3.Distance(queue_O.transform.position, spawnPos[i].position) < 0.5f)
                {
                    current_index = i;
                    break;
                }
            }
            queue7_no_of_blocks = 0;
            queue7_no_of_blocks_arrived = 0;
            queue_O.transform.GetChild(3).gameObject.SetActive(true);

            queue_O.GetComponent<Animator>().SetTrigger("PlayDestroy");
            yield return new WaitForSeconds(.8f);


            if (CanvasManager.instance.canVibrate)
            {
                HeavyImpactHaptic();
            }
            MusicManager.instance.PlayClip(2);

            Instantiate(destroy, queue_O.transform.position, Quaternion.identity);
            Destroy(queue_O);

            no_of_queue--;
           
            no_of_queue_will_destroyed--;
            for (int j = 0; j < places.Length; j++)
            {
                if (cube_Orange != null)
                {
                    for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                    {
                        if (Ready_Queue.transform.GetChild(k) != null)
                        {
                            if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Orange)
                            {
                                if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                {
                                    GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                    StartCoroutine(TransferBlock(block));
                                    //yield return new WaitForSeconds(.2f);


                                }
                            }
                        }

                    }

                }

            }
            StartCoroutine(spawnQueue());


        }
        Check_Victory();
        

    }

    public GameObject[] Queue;
    public Transform[] spawnPos;
    
    public int current_index = 0;

    public int no_of_queue = 0;

     bool pink_queue_exist = false;
     bool red_queue_exist = false;
     bool blue_queue_exist = false;
     bool green_queue_exist = false;
     bool yellow_queue_exist = false;
    bool Violet_queue_exist = false;
    bool Orange_queue_exist = false;

    GameObject queue1;
    GameObject queue2;
    GameObject queue3;
    GameObject queue4;
    GameObject queue5;
    GameObject queue6;
    GameObject queue7;


    int queue1_no_of_blocks = 0;
    int queue2_no_of_blocks = 0;
    int queue3_no_of_blocks = 0;
    int queue4_no_of_blocks = 0;
    int queue5_no_of_blocks = 0;
    int queue6_no_of_blocks = 0;
    int queue7_no_of_blocks = 0;

    int queue1_no_of_blocks_arrived = 0;
    int queue2_no_of_blocks_arrived = 0;
    int queue3_no_of_blocks_arrived = 0;
    int queue4_no_of_blocks_arrived = 0;
    int queue5_no_of_blocks_arrived = 0;
    int queue6_no_of_blocks_arrived = 0;
    int queue7_no_of_blocks_arrived = 0;

    [SerializeField]int no_of_queue_will_destroyed ;
   


    public List<int> spawnPool = new List<int>();
    int spawnIndex = 0;
    bool delay_spawn = false;
    public int ready_Queue_child_num;



    IEnumerator spawnQueue()
    {
        coolDownPlane.SetActive(false); 
        CanvasManager.instance.IsPowerUse = false;
        CanvasManager.instance.fillQueuePow_AlreadyinUse = false;
        if (delay_spawn)
        {
            
            yield return new WaitForSeconds(.5f);
            
            
        }
        delay_spawn = true;
        coolDownPlane.SetActive(false);

        if (no_of_queue < 2)
        {
            
            for (int i = no_of_queue; i < 2; i++)
            {
               if (spawnPos[i] != null)
               {
                    int random_num; 
                    


                    if (spawnIndex < spawnPool.Count)
                    {
                        random_num = spawnPool[spawnIndex];
                        spawnIndex++;
                    }
                    else
                    {
                        random_num = -1; // All values used
                    }

                    //Debug.Log("Spawned: " + random_num);


                    
                    if (random_num == 0)
                    {
                        coolDownPlane.SetActive(false);
                        queue1 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90 , -90 , 0));
                       
                        queue1.transform.SetParent(spawnPos[current_index].transform, true);
                        
                        
                        for (int j = 0; j <places.Length; j++)
                        {
                            if (cube_red != null)
                            {
                                for (int k = ready_Queue_child_num ; k < Ready_Queue.transform.childCount ; k++ )
                                {
                                    if (Ready_Queue.transform.GetChild(k)!=null)
                                    {
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Red)
                                        {
                                            
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                StartCoroutine(TransferBlock(block));
                                               // yield return new WaitForSeconds(.05f);



                                            }
                                        }
                                    }

                                }
                               

                                
                            }
                            
                        }
                        
                        no_of_queue++;

                    }
                    else if (random_num == 1)
                    {
                        coolDownPlane.SetActive(false);

                        queue2 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90,-90,0));
                        
                        queue2.transform.SetParent(spawnPos[current_index].transform, true);

                        
                        for (int j = 0; j < places.Length; j++)
                        {
                            if (cube_blue != null)
                            {
                                for (int k = ready_Queue_child_num ; k < Ready_Queue.transform.childCount; k++)
                                {
                                    if (Ready_Queue.transform.GetChild(k)!=null)
                                    {
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Blue)
                                        {
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                StartCoroutine(TransferBlock(block));
                                                //yield return new WaitForSeconds(.2f);


                                            }
                                        }
                                    }
                                  
                                }
                                
                            }
                            
                        }
                        
                        no_of_queue++;


                    }
                    else if (random_num == 2)
                    {
                        coolDownPlane.SetActive(false);

                        queue3 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90, -90, 0));
                        
                        queue3.transform.SetParent(spawnPos[current_index].transform, true);

                        
                        for (int j = 0; j < places.Length; j++)
                        {
                            if (cube_green != null)
                            {

                                for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount ; k++)
                                {
                                    if (Ready_Queue.transform.GetChild(k)!= null)
                                    {
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Green)
                                        {
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                StartCoroutine(TransferBlock(block));
                                               // yield return new WaitForSeconds(.2f);

                                            }
                                        }
                                    }
                                   
                                }
                                
                            }
                           

                        }
                        
                        no_of_queue++;
                    }
                    else if (random_num == 3)
                    {
                        coolDownPlane.SetActive(false);

                        queue4 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90, -90, 0));
                        
                        queue4.transform.SetParent(spawnPos[current_index].transform, true);

                        
                        for (int j = 0; j < places.Length; j++)
                        {
                            if (cube_yellow != null)
                            {
                                for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount ; k++)
                                {
                                    if (Ready_Queue.transform.GetChild(k)!=null)
                                    {
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Yellow)
                                        {
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                StartCoroutine(TransferBlock(block));
                                                //yield return new WaitForSeconds(.2f);



                                            }
                                        }
                                    }
                                   
                                }
                                
                            }
                            

                        }
                        
                        no_of_queue++;
                    }
                    else if (random_num == 4)
                    {
                        coolDownPlane.SetActive(false);

                        queue5 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90, -90, 0));
                        
                        queue5.transform.SetParent(spawnPos[current_index].transform, true);

                        for (int j = 0; j < places.Length; j++)
                        {
                            //print("j");
                            if (cube_pink != null)
                            {
                                
                                for (int k = ready_Queue_child_num ; k < Ready_Queue.transform.childCount ; k++)
                                {
                                   // print("k");
                                   
                                    if (Ready_Queue.transform.GetChild(k)!=null)
                                    {
                                        
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Pink)
                                        {
                                            
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                
                                                    StartCoroutine(TransferBlock(block));
                                                    //yield return new WaitForSeconds(5f);
                                             
                                               

                                            }
                                        }
                                    }
                                    
                                }
                                
                            }
                           

                        }
                        
                        no_of_queue++;
                    }
                    else if (random_num == 5)
                    {
                        coolDownPlane.SetActive(false);

                        queue6 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90, -90, 0));

                        queue6.transform.SetParent(spawnPos[current_index].transform, true);

                        for (int j = 0; j < places.Length; j++)
                        {
                            if (cube_violet != null)
                            {
                                for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                                {
                                    if (Ready_Queue.transform.GetChild(k) != null)
                                    {
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Violet)
                                        {
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                StartCoroutine(TransferBlock(block));
                                                //yield return new WaitForSeconds(.2f);


                                            }
                                        }
                                    }

                                }

                            }

                        }
                        
                        no_of_queue++;
                        

                    }
                    else if (random_num == 6)
                    {
                        coolDownPlane.SetActive(false);

                        queue7 = Instantiate(Queue[random_num], spawnPos[current_index].position, Quaternion.Euler(-90, -90, 0));

                        queue7.transform.SetParent(spawnPos[current_index].transform, true);

                        for (int j = 0; j < places.Length; j++)
                        {
                            if (cube_Orange != null)
                            {
                                for (int k = ready_Queue_child_num; k < Ready_Queue.transform.childCount; k++)
                                {
                                    if (Ready_Queue.transform.GetChild(k) != null)
                                    {
                                        if (Ready_Queue.transform.GetChild(k).transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Orange)
                                        {
                                            if (Vector3.Distance(Ready_Queue.transform.GetChild(k).transform.position, places[j].position) < .3f)
                                            {
                                                GameObject block = Ready_Queue.transform.GetChild(k).gameObject;

                                                StartCoroutine(TransferBlock(block));
                                                //yield return new WaitForSeconds(.2f);


                                            }
                                        }
                                    }

                                }

                            }

                        }

                        no_of_queue++;


                    }
                    current_index++;


                }
                
            }
        }
        
    }


   
   
    IEnumerator lerping(GameObject Object , Vector3 initialPos , Vector3 finalPos , Transform parent)
    {
        MusicManager.instance.PlayClip(4);
        if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Red)
        {
            
            queue1_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }
            

            if (CanvasManager.instance.IsPowerUse == true)
            {

                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x, .7f, finalPos.z);
               
            }
            else
            {
                
                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }


            Object.transform.tag = "Untagged";
            queue1_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());

        }
        else if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Blue)
        {
            queue2_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }

            if (CanvasManager.instance.IsPowerUse == true)
            {

                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x, .7f, finalPos.z);
               
            }
            else
            {

                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }


            Object.transform.tag = "Untagged";
            queue2_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());


        }
        else if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Green)
        {
            queue3_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }

            if (CanvasManager.instance.IsPowerUse == true)
            {

                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x, .7f, finalPos.z);
                
            }
            else
            {

                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }


            Object.transform.tag = "Untagged";
            queue3_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());

        }
        else if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Yellow)
        {
            queue4_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }

            if (CanvasManager.instance.IsPowerUse == true)
            {

                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x, .7f, finalPos.z);
               
            }
            else
            {

                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }


            Object.transform.tag = "Untagged";
            queue4_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());

        }
        else if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Pink)
        {
           
            queue5_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }

            if (CanvasManager.instance.IsPowerUse== true)
            {
               
                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x,.7f,finalPos.z);
                
            }
            else
            {
              
                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }


            
            
            Object.transform.tag = "Untagged";
            queue5_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());
        }
        else if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Violet)
        {
            queue6_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }

            if (CanvasManager.instance.IsPowerUse == true)
            {

                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x, .7f, finalPos.z);
                
            }
            else
            {

                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }



            Object.transform.tag = "Untagged";
            queue6_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());

        }

        else if (Object.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial == Orange)
        {
            queue7_no_of_blocks++;
            initialPos = Object.transform.position;

            Vector3 imagnaryPos = finalPos + new Vector3(0, 5, 0);

            float time = 0;
            float duration = 1f;
            while (time < duration)
            {

                float t = time / duration;
                Object.transform.position = Vector3.Lerp(initialPos, imagnaryPos, time / .4f);
                imagnaryPos = Vector3.Lerp(imagnaryPos, finalPos, time / 3.5f);
                time += Time.deltaTime;
                yield return null;
            }

            if (CanvasManager.instance.IsPowerUse == true)
            {

                Object.transform.SetParent(parent.transform, true);
                Object.transform.position = new Vector3(finalPos.x, .7f, finalPos.z);

            }
            else
            {

                Object.transform.SetParent(parent.transform, false);
                Object.transform.position = finalPos;
            }



            Object.transform.tag = "Untagged";
            queue7_no_of_blocks_arrived++;
            StartCoroutine(Destroy_Queue());

        }



    }

    [SerializeField] private GameObject celebrate;
    [SerializeField] private GameObject victory_popUp;

    public void Check_Victory()
    {
        
        StartCoroutine(func());
        

    }

    IEnumerator func()
    {
        yield return new WaitForSeconds(.5f);
        if (no_of_queue_will_destroyed == 0)
        {
            if (CanvasManager.instance.canVibrate)
            {
                GameManager.Instance.SuccessHaptic();
            }



            if (current_level == 1)
            {
                PlayerPrefs.SetInt("Level cleared", 1);
            }

            else if (current_level== 2)
            {
                PlayerPrefs.SetInt("Level cleared", 2);
                
            }
            else if (current_level == 3)
            {
                PlayerPrefs.SetInt("Level cleared", 3);
                
            }
            else if (current_level == 4)
            {
                PlayerPrefs.SetInt("Level cleared", 4);

            }
            else if (current_level == 5)
            {
                PlayerPrefs.SetInt("Level cleared", 5);

            }
            else if (current_level == 6)
            {
                PlayerPrefs.SetInt("Level cleared", 6);

            }
            else if (current_level == 7)
            {
                PlayerPrefs.SetInt("Level cleared", 7);

            }
            else if (current_level == 8)
            {
                PlayerPrefs.SetInt("Level cleared", 8);

            }
            else if (current_level == 9)
            {
                PlayerPrefs.SetInt("Level cleared", 9);

            }
            else if (current_level == 10)
            {
                PlayerPrefs.SetInt("Level cleared", 10);

            }
            else if (current_level == 11)
            {
                PlayerPrefs.SetInt("Level cleared", 11);

            }
            else if (current_level == 12)
            {
                PlayerPrefs.SetInt("Level cleared", 12);

            }
            else if (current_level == 13)
            {
                PlayerPrefs.SetInt("Level cleared", 13);

            }
            else if (current_level == 14)
            {
                PlayerPrefs.SetInt("Level cleared", 14);

            }
            else if (current_level == 15)
            {
                PlayerPrefs.SetInt("Level cleared", 15);

            }
            else if (current_level == 16)
            {
                PlayerPrefs.SetInt("Level cleared", 16);

            }
            else if (current_level == 17)
            {
                PlayerPrefs.SetInt("Level cleared", 17);

            }
            else if (current_level == 18)
            {
                PlayerPrefs.SetInt("Level cleared", 18);

            }
            else if (current_level == 19)
            {
                PlayerPrefs.SetInt("Level cleared", 19);

            }
            coolDownIMG.SetActive(true);
            coolDownPlane.SetActive(true);
            Timer.Instance.counting_down = false;
            celebrate.SetActive(true);
            yield return new WaitForSeconds(2f);
            victory_popUp.SetActive(true);
            MusicManager.instance.PlayClip(3);
           
        }
    }

    [SerializeField] Image fadePanelimg;
    [SerializeField] GameObject fadePanel;
    public  int current_level;


    public RectTransform coinstorage;
    [SerializeField] GameObject coinPrefab;
    public GameObject glow_prefab;
    [SerializeField] Transform canvas;
    
    public RectTransform final;
    IEnumerator FadeAndLoad()
    {
        MusicManager.instance.PlayClip(10);
        GameObject glow = Instantiate(glow_prefab, canvas);
        Destroy(glow, .5f);
        for (int i = 0; i < 10; i++)
        {
            
            yield return new WaitForSeconds(.1f);
            GameObject coin = Instantiate(coinPrefab, canvas);
           

            Destroy(coin, 1f);
            
        }

        yield return new WaitForSeconds(1f);

        // Fade from 0 to 1 alpha
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = fadePanelimg.color;
            c.a = t;
            fadePanelimg.color = c;
            yield return null;

            
        }

        for (int i = 1; i <20;i++)
        {
            if (current_level == i)
            {
                SceneManager.LoadScene(i+2);
            }
        }
        
    }

    public GameObject red_alert_screen;
    public Image red_alert_screen_img;
    public TextMeshProUGUI one_space_left;
    IEnumerator red_alert_on()
    {
        red_alert_screen.SetActive(true);
        // Fade from 0 to 1 alpha
        for (float t = 0; t < 1; t += Time.deltaTime *5)
        {
            Color c = red_alert_screen_img.color;
            c.a = t;
            red_alert_screen_img.color = c;
            yield return null;


        }
        for (float t = 0; t < 1; t += Time.deltaTime * 5)
        {
            Color c = one_space_left.color;
            c.a = t;
            one_space_left.color = c;
            yield return null;


        }


    }

    void red_alert_off()
    {
        red_alert_screen.SetActive(false);
    }

    public void Continue()
    {
        fadePanel.SetActive(true);
        StartCoroutine(FadeAndLoad());
    }



    //Haptics down below
    public void LightImpactHaptic()
    {
        Most_HapticFeedback.Generate(Most_HapticFeedback.HapticTypes.LightImpact);
    }

    public void SuccessHaptic()
    {
        Most_HapticFeedback.Generate(Most_HapticFeedback.HapticTypes.Success);
    }

    public void HeavyImpactHaptic()
    {
        Most_HapticFeedback.Generate(Most_HapticFeedback.HapticTypes.HeavyImpact);
    }
}
