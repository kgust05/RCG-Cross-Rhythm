using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCG_Cross_Rhythm_Proto_4
{
    //controls game elements, manages statuses and manipulates note properties
    public static class Game
    {
        private static int score;
        private static int health;
        private static int combo;
        private static int maxCombo;
        private static string mode;
        private static int seqAmount;
        private static int interval;
        private static int timingAllowanceMultiplier;
        private static int countInAmount;

        static Game()
        {
            score = 0;
            health = 15;
            combo = 0;
            maxCombo = 0;
            mode = "Normal";
            seqAmount = 8;
            interval = 500;
            timingAllowanceMultiplier = 2;
            countInAmount = 4;
        }

        //handles score calculation for each pressed note
        public static void Calculate(int position, int milliseconds, string keyPressed)
        {
            List<Note> displayedNotesList = Visuals.GetDisplayedNotes().GetList();

            foreach (Note note in displayedNotesList)
            {
                if (note.GetPosition() == position)
                {
                    note.SetClicked(true);

                    int correctTime = interval * note.GetPosition();
                    int timeDiff = milliseconds - correctTime;

                    timeDiff = MakePositive(timeDiff);

                    if (keyPressed == note.GetToPress())
                    {
                        if (timeDiff < 75 * timingAllowanceMultiplier)
                        {
                            score += 250 * ++combo;
                            health += 3;
                            HealthCap();
                            SetMaxCombo();
                            Visuals.DisplayHitDisplay(note, "250");
                        }
                        else if (timeDiff < 125 * timingAllowanceMultiplier)
                        {
                            score += 100 * ++combo;
                            health += 2;
                            HealthCap();
                            SetMaxCombo();
                            Visuals.DisplayHitDisplay(note, "100");
                        }
                        else if (timeDiff < 250 * timingAllowanceMultiplier)
                        {
                            score += 50 * ++combo;
                            health += 1;
                            HealthCap();
                            SetMaxCombo();
                            Visuals.DisplayHitDisplay(note, "50");
                        }
                        else
                        {
                            combo = 0;
                            health -= 3;
                            HealthCap();
                            Visuals.DisplayHitDisplay(note, "0");
                        }
                    }
                    else
                    {
                        combo = 0;
                        health -= 3;
                        HealthCap();
                        Visuals.DisplayHitDisplay(note, "0");
                    }

                    break;
                }
            }
        }

        //makes sure health stays in proper bounds
        private static void HealthCap()
        {
            if (health > 15)
            {
                health = 15;
            }

            if (health < 0)
            {
                health = 0;
            }
        }

        //sets maxCombo to combo if maxCombo is exceeded
        private static void SetMaxCombo()
        {
            if (combo > maxCombo)
            {
                maxCombo = combo;
            }
        }

        //handles score/health changing when a note is missed (not clicked)
        public static void RegisterLateMiss()
        {
            if (CheckIfLateMissed())
            {
                combo = 0;
                health -= 3;
                HealthCap();
            }
        }

        //updates lifetime of visible notes
        public static void UpdateLifetime()
        {
            foreach (Note note in Visuals.GetDisplayedNotes().GetList())
            {
                if (note != default)
                {
                    note.SetLifetime(note.GetLifetime() + 1);
                }
            }
        }

        //checks if a note has been missed, and triggers the appropriate Visuals method
        public static bool CheckIfLateMissed()
        {
            Note recentPastNote = Visuals.GetDisplayedNotes().Dequeue();

            if (recentPastNote != default)
            {
                if (!recentPastNote.GetClicked() && recentPastNote.GetEnabledStat())
                {
                    Visuals.DisplayHitDisplay(recentPastNote, "0");

                    return true;
                }
            }

            return false;
        }

        //returns whether or not health is 0 (game over)
        public static bool CheckIfGameOver()
        {
            if (health == 0)
            {
                return true;
            }

            return false;
        }

        //used to check if the game has reached its end (game cleared)
        public static bool CheckIfFinished()
        {
            foreach (Note note in Visuals.GetDisplayedNotes().GetList())
            {
                if (note != default)
                {
                    return false;
                }
            }

            return true;
        }

        //mass setter for relevant difficulty properties
        public static void SetDifficultyProperties(int s, int i, int t, int c, string m)
        {
            seqAmount = s;
            interval = i;
            timingAllowanceMultiplier = t;
            countInAmount = c;
            mode = m;
        }

        //makes any negative number positive (absolute value modulus)
        public static int MakePositive(int input)
        {
            if (input < 0)
            {
                return input *= -1;
            }
            else
            {
                return input;
            }
        }

        //resets all relevant properties in the class to new objects ready for next game loop
        public static void Reset()
        {
            score = 0;
            combo = 0;
            health = 15;
            maxCombo = 0;
        }

        //individual getters here
        public static int GetScore()
        {
            return score;
        }

        public static int GetHealth()
        {
            return health;
        }

        public static int GetCombo()
        {
            return combo;
        }

        public static int GetMaxCombo()
        {
            return maxCombo;
        }

        public static string GetMode()
        {
            return mode;
        }

        public static int GetSeqAmount()
        {
            return seqAmount;
        }

        public static int GetInterval()
        {
            return interval;
        }

        public static int GetCountInAmount()
        {
            return countInAmount;
        }
    }
}
