using UnityEngine;
using System;
using System.Collections.Generic;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private static bool initialized = false;

    private static readonly Queue<Action> executionQueue = new Queue<Action>();

    public static MainThreadDispatcher Instance
    {
        get
        {
            Initialize();
            return instance;
        }
    }

    private static void Initialize()
    {
        if (!initialized)
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MainThreadDispatcher>();
                if (instance == null)
                {
                    var container = new GameObject("MainThreadDispatcher");
                    instance = container.AddComponent<MainThreadDispatcher>();
                    DontDestroyOnLoad(container);
                }
            }
            initialized = true;
        }
    }

    public void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Invoke(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }
}
