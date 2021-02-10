using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoAtaque : StateMachineBehaviour
{

    Rigidbody2D rigidbody2D;
    Inimigo inimigo;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidbody2D = animator.GetComponentInParent<Rigidbody2D>();
        inimigo = animator.GetComponentInParent<Inimigo>();
       /* if(inimigo.distancia == false && inimigo.esquerda == false)
        animator.GetComponent<SpriteRenderer>().flipX = false;
        else if (inimigo.distancia == false && inimigo.esquerda == true)
            animator.GetComponent<SpriteRenderer>().flipX = true;*/
        //inimigo.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        if (Vector2.Distance(rigidbody2D.position, inimigo.RetornaAlvo()) < inimigo.RetornaDistanciaDeAtaque())
        {
            animator.SetTrigger("Atacar");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Atacar");
    }


}
