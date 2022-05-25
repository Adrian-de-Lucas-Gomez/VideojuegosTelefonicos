using System;
using System.Collections.Generic;
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
        //public int levelsUnlocked;
    }

    [Serializable]
    public class LevelProgress
    {
        public int moveRecord;
        public bool completed;
        public bool locked;
    }

    [Serializable]
    public class ProgressData
    {
        //Poner aquí los datos relevantes para el guardado
        public int hints;
        //Array de categorias dado por el GameManager
        public CategoryProgress[] categories;

        //Metodo que crea los datos de progreso desde 0
        public void Init(List<Categories> cat)
        {
            hints = 3;

            categories = new CategoryProgress[cat.Count];

            for(int i=0; i < categories.Length; i++)
            {
                categories[i] = new CategoryProgress();

                categories[i].categoryName = cat[i].title;

                categories[i].packs = new PackProgress[cat[i].packs.Count];

                for(int j=0; j < categories[i].packs.Length; j++)
                {
                    PackProgress[] pack = categories[i].packs;

                    pack[j] = new PackProgress();

                    pack[j].packName = cat[i].packs[j].title;
                    pack[j].levelsCompleted = 0;

                    //Hacemos el split para saber el número de niveles que hay en el pack
                    int numLevels = cat[i].packs[j].levelsFile.ToString().Split('\n').Length;
                    //-------------------------------------------------------------------

                    //Creamos array de niveles
                    pack[j].levels = new LevelProgress[numLevels];

                    bool levelInPackLocked;

                    if (cat[i].packs[j].fullyUnlocked) {     //Si todos los niveles estan disponibles
                        levelInPackLocked = false;
                    }
                    else {
                        levelInPackLocked = true;
                    } //Todos los niveles están desbloqueados

                    //El primero de cada pack siempre está desbloqueado
                    pack[j].levels[0] = new LevelProgress();
                    pack[j].levels[0].locked = false;   

                    for (int k = 1; k < numLevels; k++)
                    {
                        pack[j].levels[k] = new LevelProgress();

                        pack[j].levels[k].moveRecord = 0;
                        pack[j].levels[k].completed = false;
                        pack[j].levels[k].locked = levelInPackLocked;
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
            LevelProgress levelProgress = categories[nCat].packs[nPack].levels[nLevel];
            //En el progreso del propio nivel
            
            if (levelProgress.completed)  //Si lo estamos rejugando
            {
                Debug.Log("Rejugado");
                if (nMoves < levelProgress.moveRecord)  //Si mejoramos el record previo
                {
                    Debug.Log("Nuevo record");
                    levelProgress.moveRecord = nMoves;
                }
            }
            else    //Si es la primera vez que lo jugamos (Desbloqueamos el siguiente nivel tambien)
            {
                Debug.Log("Nueva entrada");

                levelProgress.completed = true;
                levelProgress.moveRecord = nMoves;

                //En el progreso del pack
                PackProgress actualPack = categories[nCat].packs[nPack];

                actualPack.levelsCompleted++;

                //Si hay siguiente nivel a jugar el en pack lo desbloqueamos
                if (nLevel + 1 < actualPack.levels.Length)
                {
                    categories[nCat].packs[nPack].levels[nLevel + 1].locked = false;
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