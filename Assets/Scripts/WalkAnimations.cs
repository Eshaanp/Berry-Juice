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


}






















