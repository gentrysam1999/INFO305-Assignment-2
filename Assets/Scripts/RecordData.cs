using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR && UNITY_METRO
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Threading.Tasks;
using System.Threading;
#endif

public class RecordData : MonoBehaviour
{

#if !UNITY_EDITOR && UNITY_METRO
    Windows.Storage.StorageFile storage;
#else
    System.IO.StreamWriter storage;
#endif

#if !UNITY_EDITOR && UNITY_METRO
    async
        public void WriteData
        (string fileName, string content)
    {
        Debug.Log("Start writing");
        try
        {
            Debug.Log("Open File");
            storage =
                    await KnownFolders.PicturesLibrary.CreateFileAsync(fileName,
                        CreationCollisionOption.ReplaceExisting);
            Debug.Log("Open Stream");
            var stream = await storage.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                using (var dataWriter = new DataWriter(outputStream))
                {
                    dataWriter.WriteString(content);
                    Debug.Log("store");
                    await dataWriter.StoreAsync();
                    Debug.Log("flush");
                    await outputStream.FlushAsync();
                }
            }
            Debug.Log("discpose");
            stream.Dispose(); // Or use the stream variable (see previous code snippet) with a using statement as well.
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        Debug.Log("done");
    }
#else
    public void WriteData
        (string fileName, string content)
    {
        storage = System.IO.File.CreateText(fileName);
        storage.WriteLine(content);
        storage.Close();
    }
#endif
}
