using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float attackDemage;//�˺�

    public void EndAttack()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
           
           
            /*if (!collision.gameObject.GetComponent<Enemy>().isAttacked)
            {
                //Debug.Log("attack");
                collision.gameObject.GetComponent<Enemy>().getAttacked(attackDemage);
                //Quaternion.identity����ת
             
                //����Ч�� ���ŷ������˶� ����λ�� - ��ɫλ��
                Vector2 p = collision.transform.position - transform.position;
                //������׼��
                p = p.normalized;
                collision.transform.position = new Vector2(collision.transform.position.x + p.x, collision.transform.position.y + p.y);
            }*/
        }
    }
}
