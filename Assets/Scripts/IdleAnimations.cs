using PurrNet;
using UnityEngine;

public class IdleAnimations : NetworkBehaviour
{


    public Animator MeowscaradaAnimator;
    public Animator VictiniAnimator;
    public Animator SliggooAnimator;
    public Animator GolisopodAnimator;
    public Animator JigglypuffAnimator;
    public Animator DrifblimAnimator;
    public Animator PlusleAnimator;
    public Animator MinunAnimator;
    public Animator RabootAnimator;


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

            case PlayerLogic.Character.Jigglypuff:

                JigglypuffAnimator.Play("Jigglypuff_Idle");
                break;

            case PlayerLogic.Character.Drifblim:

                DrifblimAnimator.Play("Drifblim_Idle");
                break;

            case PlayerLogic.Character.Oricorio:

                MinunAnimator.Play("Minun_Idle");
                PlusleAnimator.Play("Plusle_Idle");
                break;

            case PlayerLogic.Character.Raboot:

                RabootAnimator.Play("Raboot_Idle");
                break;



            default:
                break;




        }





    }


}
