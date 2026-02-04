using PurrNet;
using UnityEngine;

public class IdleAnimations : NetworkBehaviour
{


    public Animator MeowscaradaAnimator;
    public Animator VictiniAnimator;
    public Animator SliggooAnimator;
    public Animator GolisopodAnimator;


    public void setIdleAnimation(PlayerLogic player)
    {
        switch (player.character)
        {
            case PlayerLogic.Character.Meowscarada:
                MeowscaradaAnimator.Play("MeowscaradaIdleForward");
                break;

            case PlayerLogic.Character.Victini:

                VictiniAnimator.Play("Victini_Idle 0");
                break;

            case PlayerLogic.Character.Sligoo:

                SliggooAnimator.Play("Sliggoo_Idle");
                break;

            case PlayerLogic.Character.Golisopod:

                GolisopodAnimator.Play("Golisopod_Idle");
                break;

            default:
                break;




        }





    }


}
