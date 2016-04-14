using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;

public class BioHarness : MonoBehaviour
{
    SerialPort port;

    void Start()
    {
        port = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
        port.Handshake = Handshake.None;
        port.ReadTimeout = 2000;
        port.Open();

        byte[] getMessage = { 0x2, 0x14, 0x1, 0x1, 0x94, 0x3 };
        byte[] answer = new byte[1024];
        port.Write(getMessage, 0, getMessage.Length);
        int bytes = port.Read(answer, 0, answer.Length);
        string a = "";
        for (int i = 0; i < bytes; i++)
        {
            a += answer[i].ToString("x");
        }
        Debug.Log(a);

        StartCoroutine("Reader");
    }

    IEnumerator Reader()
    {
        while (true)
        {
            byte[] answer = new byte[1024];
            try
            {
                int bytes = port.Read(answer, 0, answer.Length);
                string a = "";
                for (int i = 0; i < bytes; i++)
                {
                    a += " " + answer[i].ToString("x") + " ";
                }
                Debug.Log(a);
                if (answer[1] == 0x2b)
                {
                    //Debug.Log(answer[12].ToString());
                    Debug.Log(answer[13].ToString());
                }
            }
            catch (TimeoutException)
            { Debug.Log("catched"); }

            yield return new WaitForSeconds(1f);
        }
    }
}
