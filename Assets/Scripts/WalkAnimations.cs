using PurrNet;
using UnityEngine;

public class WalkAnimations : NetworkBehaviour
{

    public int lastDirection = 0;
    public int direction;



    private Vector3 lastPosition;

    public Animator MeowscaradaAnimator;
    public Animator VictiniAnimator;
    public Animator SliggooAnimator;
    public Animator GolisopodAnimator;
    public Animator JigglypuffAnimator;
    public Animator DrifblimAnimator;
    public Animator PlusleAnimator;
    public Animator MinunAnimator;
    public Animator RabootAnimator;



    public void DetermineDirection(PlayerLogic player, Vector3 startPos, Vector3 targetPos)
    {
        Vector3 delta = targetPos - startPos;
        delta.y = 0f;

        if (delta == Vector3.zero)
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
        {
            direction = delta.x > 0 ? 2 : 4;
        }
        else
        {
            direction = delta.z > 0 ? 1 : 3;
        }
        SetWalkAnimation(player);
    }

    public void SetWalkAnimation(PlayerLogic player)
    {
        if (lastDirection == direction)
        {
            //return;
        }
        lastDirection = direction;

        switch (player.character)
        {
            case PlayerLogic.Character.Meowscarada:
                MeoscaradaWalk();
                break;


            case PlayerLogic.Character.Victini:
                VictiniWalk();
                break;

            case PlayerLogic.Character.Sligoo:
                SliggooWalk();
                break;

            case PlayerLogic.Character.Golisopod:
                GolisopodWalk();
                break;

            case PlayerLogic.Character.Drifblim:
                DrifblimWalk();
                break;

            case PlayerLogic.Character.Jigglypuff:
                JigglypuffWalk();
                break;

            case PlayerLogic.Character.Oricorio:
                PlusleMinunWalk();
                break;

            case PlayerLogic.Character.Raboot:
                RabootWalk();
                break;

            default:
                break;

        }


    }


    public void MeoscaradaWalk()
    {
        switch (direction)
        {
            case 1:
                MeowscaradaAnimator.Play("Meowscarada_Walk_Backward");
                break;
            case 2:
                MeowscaradaAnimator.Play("Meowscarada_Walk_Forward");
                break;
            case 3:
                MeowscaradaAnimator.Play("Meowscarada_Walk_Down");
                break;
            case 4:
                MeowscaradaAnimator.Play("Meowscarada_Walk_Backward");
                break;

        }
    }


    public void VictiniWalk()
    {

        switch (direction)
        {
            case 1:
                VictiniAnimator.Play("Victini_Walk_Backward");
                break;
            case 2:
                VictiniAnimator.Play("Victini_Walk_Forward");
                break;
            case 3:
                VictiniAnimator.Play("Victini_Walk_Down");
                break;
            case 4:
                VictiniAnimator.Play("Victini_Walk_Backward");
                break;

        }

    }


    public void SliggooWalk()
    {

        switch (direction)
        {
            case 1:
                SliggooAnimator.Play("Sliggoo_Walk_Backward");
                break;
            case 2:
                SliggooAnimator.Play("Sliggoo_Walk_Forward");
                break;
            case 3:
                SliggooAnimator.Play("Sliggoo_Walk_Down");
                break;
            case 4:
                SliggooAnimator.Play("Sliggoo_Walk_Backward");
                break;

        }

    }

    public void GolisopodWalk()
    {

        switch (direction)
        {
            case 1:
                GolisopodAnimator.Play("Golisopod_Walk_Backward");
                break;
            case 2:
                GolisopodAnimator.Play("Golisopod_Walk_Forward");
                break;
            case 3:
                GolisopodAnimator.Play("Golisopod_Walk_Down");
                break;
            case 4:
                GolisopodAnimator.Play("Golisopod_Walk_Backward");
                break;

        }

    }


    public void JigglypuffWalk()
    {
        switch (direction)
        {
            case 1:
                JigglypuffAnimator.Play("Jigglypuff_Walk_Backward");
                break;
            case 2:
                JigglypuffAnimator.Play("Jigglypuff_Walk_Forward");
                break;
            case 3:
                JigglypuffAnimator.Play("Jigglypuff_Walk_Down");
                break;
            case 4:
                JigglypuffAnimator.Play("Jigglypuff_Walk_Backward");
                break;

        }
    }

    public void DrifblimWalk()
    {
        switch (direction)
        {
            case 1:
                DrifblimAnimator.Play("Drifblim_Walk_Backward");
                break;
            case 2:
                DrifblimAnimator.Play("Drifblim_Walk_Forward");
                break;
            case 3:
                DrifblimAnimator.Play("Drifblim_Walk_Down");
                break;
            case 4:
                DrifblimAnimator.Play("Drifblim_Walk_Backward");
                break;

        }
    }


    public void PlusleMinunWalk()
    {
        switch (direction)
        {
            case 1:
                PlusleAnimator.Play("Plusle_Walk_Backward");
                MinunAnimator.Play("Minun_Walk_Backward");
                break;
            case 2:
                PlusleAnimator.Play("Plusle_Walk_Forward");
                MinunAnimator.Play("Minun_Walk_Forward");
                break;
            case 3:
                PlusleAnimator.Play("Plusle_Walk_Down");
                MinunAnimator.Play("Minun_Walk_Down");
                break;
            case 4:
                PlusleAnimator.Play("Plusle_Walk_Backward");
                MinunAnimator.Play("Minun_Walk_Backward");
                break;

        }

    }


    public void RabootWalk()
    {
        switch (direction)
        {
            case 1:
                RabootAnimator.Play("Raboot_Walk_Backward");
                break;
            case 2:
                RabootAnimator.Play("Raboot_Walk_Forward");
                break;
            case 3:
                RabootAnimator.Play("Raboot_Walk_Down");
                break;
            case 4:
                RabootAnimator.Play("Raboot_Walk_Backward");
                break;

        }
    }

}






















