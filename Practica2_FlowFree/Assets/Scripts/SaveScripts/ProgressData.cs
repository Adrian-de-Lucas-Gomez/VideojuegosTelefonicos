using System;
using UnityEngine;

namespace flow
{
    [Serializable]
    public class CategoryProgress
    {
        public string categoryName;
        public PackProgress[] packs;
    }

    [Serializable]
    public class PackProgress
    {
        public string packName;
        public LevelProgress[] levels;
        public int levelsCompleted;
        public int levelsUnlocked;
    }

    [Serializable]
    public class LevelProgress
    {
        public int moveRecord;
        public bool completed;
    }

    [Serializable]
    public class ProgressData
    {
        //Poner aquí los datos relevantes para el guardado
        public int hints;
        public CategoryProgress[] categories;

        //Array de categorias dado por el GameManager
        public void Init(Categories[] cat)
        {
            hints = 3;

            categories = new CategoryProgress[cat.Length];

            for(int i=0; i < categories.Length; i++)
            {
                categories[i] = new CategoryProgress();

                categories[i].categoryName = cat[i].title;

                categories[i].packs = new PackProgress[cat[i].packs.Length];

                for(int j=0; j < categories[i].packs.Length; j++)
                {
                    PackProgress[] aux = categories[i].packs;

                    aux[j] = new PackProgress();

                    aux[j].packName = cat[i].packs[j].title;
                    aux[j].levelsCompleted = 0;

                    //Hacemos el split para saber el número de niveles que hay en el pack
                    int numLevels = cat[i].packs[j].levelsFile.ToString().Split('\n').Length;
                    //-------------------------------------------------------------------
                    //Creamos array de niveles
                    aux[j].levels = new LevelProgress[numLevels];


                    if (cat[i].levelsLocked) { aux[j].levelsUnlocked = 1; }
                    else { aux[j].levelsUnlocked = numLevels-1; } //Todos los niveles están desbloqueados

                    for (int k = 0; k < numLevels; k++)
                    {
                        aux[j].levels[k] = new LevelProgress();

                        aux[j].levels[k].moveRecord = 0;
                        aux[j].levels[k].completed = false;
                    }
                }
            }
        }

        ////Para recuperar datos de otros guardados
        //public ProgressData(ProgressData data)
        //{
        //    hints = data.hints;
        //    categories = data.categories;
        //}


        //Métodos para modificar el progeso en el juego
        public void OnLevelCompleted(int nCat, int nPack, int nLevel, int nMoves)
        {
            LevelProgress aux = categories[nCat].packs[nPack].levels[nLevel];
            //En el progreso del propio nivel
            
            if (aux.completed)  //Si lo estamos rejugando
            {
                Debug.Log("Rejugado");
                if (nMoves < aux.moveRecord)  //Si mejoramos el record previo
                {
                    Debug.Log("Nuevo record");
                    aux.moveRecord = nMoves;
                }
            }
            else    //Si es la primera vez que lo jugamos
            {
                Debug.Log("Nueva entrada");

                aux.completed = true;
                aux.moveRecord = nMoves;

                //En el progreso del pack
                PackProgress auxPack = categories[nCat].packs[nPack];

                auxPack.levelsCompleted++;

                if (auxPack.levelsUnlocked < auxPack.levels.Length)
                {
                    auxPack.levelsUnlocked++;
                }
            }
        }

        public void OnHintUsed()
        {
            //Quitamos pistas hasta que sean 0
            if (hints > 0) hints--;
        }

        public void OnHintAdded()
        {
            //Añadimos pistas por ver el anuncio bonificado
            hints++;
        }
    }

}