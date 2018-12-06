using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BatchTool
{
    public static void Batch(Transform root)
    {
        if (root == null)
        {
            return;
        }

        int count = root.childCount;
        if (count <= 1)
        {
            return;
        }

        Dictionary<string, List<Transform>> imageBatchGroup = new Dictionary<string, List<Transform>>();
        Dictionary<string, List<Transform>> textBatchGroup = new Dictionary<string, List<Transform>>();
        List<List<Transform>> sortedImageBatchGroup = new List<List<Transform>>();
        List<List<Transform>> sortedTextBatchGroup = new List<List<Transform>>();

        for (int i = 0; i < count; i++)
        {
            Transform child = root.GetChild(i);
            IBatch batch = CreateBatch(child);

            if (batch == null)
            {
                continue;
            }

            string batchKey = batch.GetBatchKey();

            if (batch is ImageBatch)
            {
                if (!imageBatchGroup.ContainsKey(batchKey))
                {
                    List<Transform> newBatchGroup = new List<Transform>();
                    sortedImageBatchGroup.Add(newBatchGroup);
                    imageBatchGroup.Add(batchKey, newBatchGroup);
                }

                imageBatchGroup[batchKey].Add(child);
            }
            else if (batch is TextBatch)
            {
                if (!textBatchGroup.ContainsKey(batchKey))
                {
                    List<Transform> newBatchGroup = new List<Transform>();
                    sortedTextBatchGroup.Add(newBatchGroup);
                    textBatchGroup.Add(batchKey, newBatchGroup);
                }

                textBatchGroup[batchKey].Add(child);
            }

            Batch(child);
        }

        for (int i = sortedImageBatchGroup.Count - 1; i >= 0; i--)
        {
            for (int j = sortedImageBatchGroup[i].Count - 1; j >= 0; j--)
            {
                sortedImageBatchGroup[i][j].SetAsFirstSibling();
            }
        }

        for (int i = 0; i < sortedTextBatchGroup.Count; i++)
        {
            for (int j = 0; j < sortedTextBatchGroup[i].Count; j++)
            {
                sortedTextBatchGroup[i][j].SetAsLastSibling();
            }
        }
    }

    private static IBatch CreateBatch(Transform root)
    {
        if(root.GetComponent<Image>())
        {
            return new ImageBatch(root);
        }
        else if(root.GetComponent<RawImage>())
        {
            return new ImageBatch(root);
        }
        else if(root.GetComponent<Text>())
        {
            return new TextBatch(root);
        }

        return null;
    }
}
