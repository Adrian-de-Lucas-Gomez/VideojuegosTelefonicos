using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SaveReadWriter
{
    [SerializeField] string fileName;
    private string savePath = "Assets/SaveProgress/";
    SHA256 hashMaker;
    public void Init()
    {
        //Creamos el generador de hash
        hashMaker = SHA256.Create();
    }
    public void SaveData(ProgressData data)
    {
        flow.SaveData saveFile = new flow.SaveData();

        saveFile.hash = "";     //No ponemos hash aun
        saveFile.data = data;   //Metemos los datos del progreso

        //Generamos el JSON solo con data (ignoramos el hash)
        string serializedData = JsonUtility.ToJson(saveFile.data);

        //Sacamos el hash de los datos serializados
        byte[] aux = Encoding.UTF8.GetBytes(serializedData);
        saveFile.hash = Encoding.UTF8.GetString(hashMaker.ComputeHash(aux));
    
        //Volvemos a hacer un Json ahora teniendo hash y progreso
        serializedData = JsonUtility.ToJson(saveFile);

        //Escribimos el Json generado
        FileStream outStream = File.Open(savePath, FileMode.Create);
        outStream.Write(Encoding.UTF8.GetBytes(serializedData), 0, serializedData.Length);
        outStream.Close();
    }

    public ProgressData LoadData()
    {
        flow.SaveData dataFromJson;
        string jsonSave = File.ReadAllText(savePath + fileName, Encoding.UTF8);

        dataFromJson = JsonUtility.FromJson<flow.SaveData>(jsonSave);

        if (dataFromJson == null) Debug.Log("Error al desserializar datos de guardado");

        //Comprobacion del hash guardado y el que crearemos
        string auxJson = JsonUtility.ToJson(dataFromJson.data);
        byte[] progressRead = Encoding.UTF8.GetBytes(auxJson);

        string newHash = Encoding.UTF8.GetString(hashMaker.ComputeHash(progressRead));

        if (dataFromJson.hash != newHash)
        {
            //Los hash no coinciden y por lo tantose ha modificado externamente
            Debug.Log("Error: Archivo modificado externamente");
            return null;
        } 

        return dataFromJson.data;
    }
}
