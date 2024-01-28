using UnityEngine;
using System;
using System.Collections;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile;
    public Transform objectTransform;

    public void Start()
    {
        ProcessCsvData(csvFile.text);
    }

    private void ProcessCsvData(string csvText)
    {
        string[] lines = csvText.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            string[] fields = SplitCsvLine(line);

            if (fields.Length >= 2) // Assuming you need at least two fields (time and rotation)
            {
                float time = float.Parse(fields[0]);
                double rotation = Math.Asin(float.Parse(fields[1])*10)*57.296;
                Debug.Log("Time: " + time + ", Rotation: " + rotation);

                StartCoroutine(DelayedRotationChange(time, (float)rotation));
            }
            else
            {
                Debug.LogWarning("Invalid CSV line at index " + i + ": " + line);
            }
        }
    }

    private IEnumerator DelayedRotationChange(float time, float rotation)
    {
        yield return new WaitForSeconds(time);

        objectTransform.rotation = Quaternion.Euler(rotation,0f, 0f);
    }

    private string[] SplitCsvLine(string line)
    {
        return line.Split(',');
    }
}
