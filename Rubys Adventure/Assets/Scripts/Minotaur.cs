using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class Minotaur : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    
    Animator animator;

    private RubyController rubyController; // this line of code creates a variable called "rubyController" to store information about the RubyController script!

    public AudioClip collectedClip;

    // Start is called before the first frame update

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby

        if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyConroller Script!");

        }

        if (rubyController == null)

        {

            print ("Cannot find GameController Script!");
        }
    }

    void Update()
    {        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }
    
}