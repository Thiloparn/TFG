using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaptopUI : MonoBehaviour
{
    public Slider slider;
    public Player player;
    public LayerMask obstacleLayer;

    private void Awake()
    {
        slider.value = 5;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && slider.value > 0 && !player.isUsingShortcut && !player.animator.GetBool("IsJumping") && !player.animator.GetBool("IsSliding") && !player.animator.GetBool("IsHitted"))
        {
            player.animator.SetTrigger("IsAttacking");
            Collider2D[] hitObstacles = Physics2D.OverlapCircleAll(Player.sharedInstance.attackPoint.position, Player.sharedInstance.attackRange, obstacleLayer);

            slider.value -= hitObstacles.Length > 0 ? 1 : 0;

            foreach(Collider2D obstacleCollider in hitObstacles)
            {
                Obstacle obstacle = obstacleCollider.GetComponent<Obstacle>();
                obstacle.isBroken = obstacle.isBreakable ? true : false;
            }
        }
    }
}
