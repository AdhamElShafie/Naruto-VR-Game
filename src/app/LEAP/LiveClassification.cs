using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System.IO;


public class LiveClassification : MonoBehaviour
{

    List<Dictionary<string, object>> points;
    Controller controller;
    float time;

    void Start()
    {
        controller = new Controller();
        time = Time.time;
        points = new List<Dictionary<string, object>>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - time) % 5.0f < 0.5f)
        {
            Dictionary<string, object> l_info = new Dictionary<string, object>();

            if (controller.IsConnected) //controller is a Controller object
            {
                Frame frame = controller.Frame(); //The latest frame
                List<Hand> hands = frame.Hands;
                if (hands.Count == 2)
                {
                    float dis = Vector3.Distance(hands[0].PalmPosition.ToVector3(), hands[1].PalmPosition.ToVector3());
                    string wr = dis.ToString();
                    l_info.Add("Distance between Hands", dis);
                    print(" distance between hands: " + dis);

                    float angle = Vector3.Angle(hands[0].PalmNormal.ToVector3(), Camera.main.transform.forward);
                    wr = wr + " " + angle;
                    l_info.Add("Angle between Left hand and camera", angle);
                    print("angle between Left hand and camera: " + angle);

                    angle = Vector3.Angle(hands[1].PalmNormal.ToVector3(), Camera.main.transform.forward);
                    wr = wr + " " + angle;
                    l_info.Add("Angle between Right hand and camera", angle);
                    print("angle between Right hand and camera: " + angle);

                    angle = Vector3.Angle(hands[0].Direction.ToVector3(), Vector3.up);
                    wr = wr + " " + angle;
                    l_info.Add("Angle between left hand direction and floor", angle);
                    print("angle between left hand direction and floor: " + angle);

                    angle = Vector3.Angle(hands[1].Direction.ToVector3(), Vector3.up);
                    wr = wr + " " + angle;
                    l_info.Add("Angle between right hand direction and floor", angle);
                    print("angle between right hand direction and floor: " + angle);

                    foreach (Hand hand in hands) {

                        string whichHand = hand.IsLeft ? "left" : "right";

                        List<Finger> fingers = hand.Fingers;
                        foreach (Finger finger in fingers)
                        {
                            l_info.Add(whichHand + " " + finger.Type.ToString(), Vector3.Distance(finger.TipPosition.ToVector3(), hand.PalmPosition.ToVector3()));
                            wr = wr + " " + Vector3.Distance(finger.TipPosition.ToVector3(), hand.PalmPosition.ToVector3());
                            print("finger: " + finger.Type + " diff: " + Vector3.Distance(finger.TipPosition.ToVector3(), hand.PalmPosition.ToVector3()));
                        }
                    }
                    points.Add(l_info);
                    using (StreamWriter writer = new StreamWriter("data.txt"))
                    {

                        writer.Write(wr);
                    }
                }
            }
        }
    }
}
