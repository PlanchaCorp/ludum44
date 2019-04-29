using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EventManager
{
    private CommentController commentController;

    int MAX_DURATION_BETWEEN_EVENTS = 11;
    int MIN_DURATION_BETWEEN_EVENTS = 4;
    int nextEventIn;
    Queue<IEvent> eventQueue;

    void populateQueue()
    {
        if(eventQueue.Count == 0)
        {
            GameContext.events.OrderBy((b) => 1 - 2 * UnityEngine.Random.Range(0, 1)).ToList().ForEach(e => eventQueue.Enqueue(e)); 
        }
    }
    public EventManager ()
    {
        commentController = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<CommentController>();
        computeNextEventIn();
        eventQueue = new Queue<IEvent>();
    }

    public void update(PlayerHealth player)
    {
        // Event trigger deactivated
        /*nextEventIn--;
        if(nextEventIn == 0)
        {
            computeNextEventIn();
            newRandomEvent(player);
        }*/

    }

    private void newRandomEvent(PlayerHealth player)
    {
        populateQueue();
        trigger(eventQueue.Dequeue(),player);
    }

    private void computeNextEventIn()
    {
        nextEventIn = UnityEngine.Random.Range(MIN_DURATION_BETWEEN_EVENTS, MAX_DURATION_BETWEEN_EVENTS);
        Debug.Log(nextEventIn);
    }

    public void trigger(IEvent gameEvent, PlayerHealth player)
    {
        commentController.ToggleEventComment(gameEvent.IsBig(), gameEvent.Prompt());
        commentController.SetButtonEffect(gameEvent, player);
        commentController.PlayAnimationForward();
    }
}
