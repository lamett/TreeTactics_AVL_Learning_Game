using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AnimationHandler : MonoBehaviour
{
    Animator animator;
    GameController gameController;
    AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

    public async Task PlayAnimationAndWait(string triggerName, string stateName)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger(triggerName);

        // Wait for the animation to start (optional but optimized)
        AnimatorStateInfo stateInfo;
        do
        {
            await Task.Yield(); // Move to the next frame
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        } while (!stateInfo.IsName(stateName));

        // Wait for the animation to complete
        while (stateInfo.normalizedTime < 1f)
        {
            await Task.Yield(); // Continue checking each frame
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
    }

    public void ShakeCamera()
    {
        audioManager.PlaySFX(audioManager.JumpOnTable);
        audioManager.StartBossMusic();
        Debug.Log(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>());
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("ToShake");
    }

    public void MixUpNodes()
    {
       
        gameController.randomRot();
    }
}
