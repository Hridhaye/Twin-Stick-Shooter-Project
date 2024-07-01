using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the visual representation of a character, including animations and sprite orientation.
/// </summary>
public class CharacterVisualComponent
{
    private const string RUNNING_BOOL_NAME = "isRunning";

    private IMovable character;
    private Animator animator;
    private SpriteRenderer gunSprite;
    private SpriteRenderer characterSprite;

    public CharacterVisualComponent(IMovable character, Animator animator, SpriteRenderer gunSprite, SpriteRenderer characterSprite)
    {
        this.character = character;
        this.animator = animator;
        this.gunSprite = gunSprite;
        this.characterSprite = characterSprite;
    }

    public void SetRunningAnimationBool()
    {
        //Play running animation if character is running.
        animator.SetBool(RUNNING_BOOL_NAME, character.isRunning);
    }

    public void SetSpriteOrientation(float aimAngle)
    {
        if (Mathf.Abs(aimAngle) < 90f)
        {
            characterSprite.flipX = false;
            gunSprite.flipY = false;
        }
        else
        {
            characterSprite.flipX = true;
            gunSprite.flipY = true;
        }
    }

}
