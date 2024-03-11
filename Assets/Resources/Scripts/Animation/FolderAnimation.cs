using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CrewmateInterview
{
    public class FolderAnimation : MonoBehaviour
    {
        //Animations
        [Header("Animation Controls")]
        [SerializeField] private Animator animator;
        [SerializeField] private bool exitFolder;
        [SerializeField] private bool enterFolder;
        [SerializeField] private bool idleFolder;

        //References
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        private void Update()
        {
            switch(gameManager.GetRoundState())
            {
                case RoundState.start:
                    animator.SetBool("exitFolder", false);
                    animator.SetBool("enterFolder", true);
                    animator.SetBool("idleFolder", false);
                    break;

                case RoundState.mid:
                    animator.SetBool("exitFolder", false);
                    animator.SetBool("enterFolder", false);
                    animator.SetBool("idleFolder", true);
                    break;
                
                case RoundState.end:
                    animator.SetBool("exitFolder", true);
                    animator.SetBool("enterFolder", false);
                    animator.SetBool("idleFolder", false);
                    break;
                default:
                    animator.SetBool("exitFolder", false);
                    animator.SetBool("enterFolder", false);
                    animator.SetBool("idleFolder", true);
                    break;
            }
        }
    }
}

