using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaptopUI : MonoBehaviour
{
    public static LaptopUI sharedInstance;
    public Slider slider;
    public Player player;
    public LayerMask obstacleLayer;

    private void Awake()
    {
        sharedInstance = this;
        slider.value = 5;
    }

    private void Update()
    {
        if(Player.sharedInstance.playerInput.actions.FindAction("Attack").triggered && slider.value > 0 && !player.isUsingShortcut && !player.animator.GetBool("IsJumping") && !player.animator.GetBool("IsSliding") && !player.animator.GetBool("IsHitted") && !ItemsUI.sharedInstance.isActive
            && !PauseMenu.sharedInstance.isActive)
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
