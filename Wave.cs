using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave: MonoBehaviour

{
    Rigidbody2D rb;
    [SerializeField]
    private float speed;
    public GameObject target;
    public GameObject position;
    public float tt = 1;
    private float dir;
    // Update is called once per frame
    private void Start()
    {
        transform.position = position.transform.position;
        dir = target.transform.localScale.x;
        //transform.gameObject.SetActive(true);
    }
    void Update()
    {
        transform.position += new Vector3(dir * speed * Time.deltaTime, 0, 0);
        tt -= Time.deltaTime;
        if (tt <= 0)
        {
            destroy();
        }
    }



    public void destroy()
    {
        Destroy(gameObject);
    }
}
