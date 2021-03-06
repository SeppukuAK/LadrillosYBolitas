﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Sistema de guardado del juego
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Ruta donde se guarda. Usa una ruta propia del sistema operativo donde se ejecuta
    /// </summary>
    private static string path = Application.persistentDataPath + "/LadrillosYBolas.data";

    /// <summary>
    /// Guarda el estado del juego en un fichero serializado
    /// </summary>
    /// <param name="totalStars"></param>
    /// <param name="gems"></param>
    /// <param name="levelData"></param>
    public static void SaveGameData(uint totalStars, uint gems, uint[] powerUps, List<LevelData> levelData)
    {
        //Creamos un BinaryFormatter
        BinaryFormatter formatter = new BinaryFormatter();

        //Para leer /escribir en el archivo
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveDataGame saveDataGame = new SaveDataGame(totalStars, gems, powerUps, levelData);

        //Convierte la clase a binario
        formatter.Serialize(stream, saveDataGame);

        //Hay que cerrarlo siempre
        stream.Close();
    }

    /// <summary>
    /// Carga el fichero serializado
    /// </summary>
    /// <returns></returns>
    public static SaveDataGame LoadGameDate()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveDataGame saveDataGame = formatter.Deserialize(stream) as SaveDataGame;
            stream.Close();

            return saveDataGame;
        }
        else
            return null;

    }

    /// <summary>
    /// Borra los archivos de guardado
    /// </summary>
    public static void DeleteData()
    {
        File.Delete(path);
    }
}
