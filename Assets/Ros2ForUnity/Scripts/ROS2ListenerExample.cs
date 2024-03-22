using UnityEngine;
using System;
using System.Collections.Generic;

namespace ROS2
{
    /// <summary>
    /// An example class provided for testing of basic ROS2 communication
    /// </summary>
    public class ROS2ListenerExample : MonoBehaviour
    {
        private ROS2UnityComponent ros2Unity;
        private ROS2Node ros2Node;
        private ISubscription<geometry_msgs.msg.Vector3> chatter_sub;

        public Transform cubeTransform;
       
   
        private void Awake()
        {
            MainThreadDispatcher.Initialize();
            
        }

        void Start()
        {
            ros2Unity = GetComponent<ROS2UnityComponent>();
           
        }

        void Update()
        {
            if (!ros2Unity.Ok())
            {
                return;
            }

            if (ros2Node == null)
            {
                ros2Node = ros2Unity.CreateNode("ROS2UnityListenerNode");
                chatter_sub = ros2Node.CreateSubscription<geometry_msgs.msg.Vector3>(
                    "my_topic", msg => MainThreadDispatcher.Instance.Invoke(() => UpdateCubeRotation(msg)));
            }
        }

        void UpdateCubeRotation(geometry_msgs.msg.Vector3 msg)
        {
            if (cubeTransform != null)
            {
                float xAngle = (float)msg.X;
                float yAngle = (float)msg.Y;
                float zAngle = (float)msg.Z;
                cubeTransform.eulerAngles = new Vector3(xAngle, 0, zAngle);

               
            }
        }
    }

    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher instance;
        private static bool initialized = false;

        private static readonly Queue<Action> executionQueue = new Queue<Action>();

        public static void Initialize()
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

        public static MainThreadDispatcher Instance
        {
            get
            {
                Initialize();
                return instance;
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
}
