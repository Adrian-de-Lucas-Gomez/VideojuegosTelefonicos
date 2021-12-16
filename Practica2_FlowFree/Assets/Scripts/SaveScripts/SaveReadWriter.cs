using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class SaveReadWriter
{
    private string fileName = "SaveFile.json";
    private string savePath = "Assets/";
    SHA256 hashMaker;
    public void Init()
    {
        //Creamos el generador de hash
        hashMaker = SHA256.Create();
    }
    public void SaveData(in flow.ProgressData data)
    {
        flow.SaveData saveFile = new flow.SaveData();

        saveFile.hash = "";     //No ponemos hash aun
        saveFile.data = data;   //Metemos los datos del progreso

        //Generamos el JSON solo con data (ignoramos el hash)
        string serializedData = JsonUtility.ToJson(saveFile.data, true);

        //Sacamos el hash de los datos serializados
        byte[] aux = Encoding.UTF8.GetBytes(serializedData);
        saveFile.hash = Encoding.UTF8.GetString(hashMaker.ComputeHash(aux));

        //Volvemos a hacer un Json ahora teniendo hash y progreso
        string reSerializedData = JsonUtility.ToJson(saveFile, true);

        if (File.Exists(savePath + fileName))
        {
            File.Delete(savePath + fileName);
        }

        //Escribimos el Json generado
        FileStream outStream = File.Open(savePath + fileName, FileMode.Create);

        byte[] aux2= Encoding.UTF8.GetBytes(reSerializedData);

        outStream.Write(aux2, 0, aux2.Length);
        outStream.Close();
    }

    public flow.ProgressData LoadData()
    {
        if (!File.Exists(savePath + fileName))  //Si no existe no se busca
        {
            return null;
        }

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
