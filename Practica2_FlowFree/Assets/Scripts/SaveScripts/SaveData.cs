using System;

namespace flow
{
    [Serializable]
    public class SaveData
    {
        //Codigo hash para evitar modificaciones externas
        public string hash;
        //Informaci�n del progreso del jugador en el juego
        public ProgressData data;
    }
}