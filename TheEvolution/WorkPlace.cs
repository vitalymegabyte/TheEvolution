using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEvolution
{
    /*Перед работой читаем DOCUMENTATION.txt!!!*/
    class WorkPlace
    {
        bool[,] cells; //Просто клетки --
        bool[,] inActive; //Неактивные клетки, обработку которых надо перенести на следующий раунд --
        Environment environment; //Переменная среды
        public void Start() //Вызывается в начале
        {
            environment = new Environment(240, 135); //Создаем среду, указываем размеры поля
            ((App)(App.Current)).Environment = environment; //Подключаем среду к приложению

            /* ^Это код, который лучше не трогать без понимания. Взаимодействие происходит ниже, и его можно менять без страха ^ */

            cells = new bool[environment.WORLD_WIDTH, environment.WORLD_HEIGHT]; //Инициализация массива клеток --
            inActive = new bool[environment.WORLD_WIDTH, environment.WORLD_HEIGHT]; //Инициализация массива неактивных клеток --
            born(new Random().Next(environment.WORLD_WIDTH), new Random().Next(environment.WORLD_HEIGHT)); //Рождение первой клетки в случайном месте --
        }
        public void Update() //Вызывается каждый кадр
        {
            for(int i = 0; i < environment.WORLD_WIDTH; i++) //Перебор поля по ширине --
            {
                for(int j = 0; j < environment.WORLD_HEIGHT; j++) // Перебор поля по высоте --
                {
                    if (!inActive[i, j]) //Если клетку можно обработать... --
                    {
                        if (cells[i, j]) //Если клетка вообще есть --
                        {
                            if (!cells[environment.cycleX(i - 1), j]) //Если свободно место с индексом i-1. --
                                //Обращаю внимание, что если есть уверенность в том, что j не больше высоты, то можно не зацикливать, но предпочтительно использовать зацикливание везде
                            {
                                born(i - 1, j); //Просто рождение
                                continue; //Только одна клетка за кадр! после сразу-же выходим!
                            }
                            if (!cells[environment.cycleX(i + 1), environment.cycleY(j)]) //Аналогично --
                            {
                                born(i + 1, j);
                                inActive[environment.cycleX(i + 1), environment.cycleY(j)] = true; //Если идет дальше по порядку - временно делаем неактивной --
                                continue;
                            }
                            if (!cells[i, environment.cycleY(j - 1)]) //--
                            {
                                born(i, j - 1);
                                continue;
                            }
                            if (!cells[i, environment.cycleY(j + 1)]) //--
                            {
                                born(i, j + 1);
                                inActive[environment.cycleX(i), environment.cycleY(j + 1)] = true; //Если идет дальше по порядку - временно делаем неактивной --
                                continue;
                            }
                        }
                    }
                    else
                        inActive[i, j] = false; //Если клетка не обрабатывалась в этом кадре - обработаем в следующем! --
                }
            }
        }
        void born(int x, int y) //Метод рождения новой клетки. Относится только к ДЕМО, и может быть удален без последствий --
        {
            cells[environment.cycleX(x), environment.cycleY(y)] = true; //Здесь зацикливаем --
            environment.drawPixel(x, y, 0, 0, 0); //Рисование пикселей зацикливает их автоматически, так что можно не бояться проблем, и смело подавать их в простом виде --
        }
    }
}
