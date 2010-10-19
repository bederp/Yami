using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yamiproject
{
    static class Score
    {
        static public int score;
        static public int coins;
        static public int lives;
        static public int top = 0;

        static public void AddCoin()
        {
            coins++;
            if (coins >= 100)
            {
                lives++;
                coins = 0;
            }
        }

        static public void AddScore(int score)
        {
            Score.score += score;
        }

        static public void Death()
        {
            lives--;
        }

        static public void SetTop()
        {
            if (top < score)
                top = score;
        }

        static public void Reset()
        {
            score = 0;
            coins = 0;
            lives = 3; //3
        }
    }



}
