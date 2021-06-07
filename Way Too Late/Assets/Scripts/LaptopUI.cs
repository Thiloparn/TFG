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
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(Player.sharedInstance.attackPoint.position, Player.sharedInstance.attackRange, obstacleLayer);

            List<Obstacle> hitObstacles = new List<Obstacle>();
            foreach(Collider2D obstacleCollider in hitColliders)
            {
                Obstacle obstacle = obstacleCollider.GetComponent<Obstacle>();
                if (!obstacle.isBroken)
                {
                    hitObstacles.Add(obstacle);
                }
            }

            slider.value -= hitObstacles.Count > 0 ? 1 : 0;

            foreach(Obstacle obstacle in hitObstacles)
            {
                if (obstacle.isBreakable)
                {
                    obstacle.brake();
                }
            }
        }
    }
}
