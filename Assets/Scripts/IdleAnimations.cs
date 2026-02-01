using PurrNet;
using UnityEngine;

public class IdleAnimations : NetworkBehaviour
{


    public Animator MeowscaradaAnimator;
    public Animator VictiniAnimator;



    public void setIdleAnimation(PlayerLogic player)
    {
        switch (player.character)
        {
            case PlayerLogic.Character.Meowscarada:
                MeowscaradaAnimator.Play("MeowscaradaIdleForward");
                break;

            case PlayerLogic.Character.Victini:
                Debug.Log("Victini Idle");
                VictiniAnimator.Play("Victini_Idle 0");
                break;

            default:
                break;




        }





    }


}
