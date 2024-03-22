using UnityEngine;
using UnityEngine.UI;
using ROS2;

namespace ROS2
{
    public class ROS2TalkerExample : MonoBehaviour
    {
        private ROS2UnityComponent ros2Unity;
        private ROS2Node ros2Node;
        private IPublisher<geometry_msgs.msg.Vector3> chatter_pub;

        public InputField inputField;

        private void Start()
        {
            ros2Unity = GetComponent<ROS2UnityComponent>();
            if (ros2Unity.Ok())
            {
                if (ros2Node == null)
                {
                    ros2Node = ros2Unity.CreateNode("ROS2UnityTalkerNode");
                    chatter_pub = ros2Node.CreatePublisher<geometry_msgs.msg.Vector3>("my_string");
                }

                PublishWithInputValue();
            }
        }

        private void PublishWithInputValue()
        {
            geometry_msgs.msg.Vector3 msg = new geometry_msgs.msg.Vector3();

            float inputValue;
            if (float.TryParse(inputField.text, out inputValue))
            {
                msg.X = inputValue;
                chatter_pub.Publish(msg);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Input is not a valid float value.");
            }
        }

        public void OnPublishButtonClicked()
        {
            PublishWithInputValue();
        }

        void Update()
        {
            // You can put any necessary update logic here
        }
    }
}
