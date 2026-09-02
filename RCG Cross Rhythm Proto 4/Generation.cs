using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCG_Cross_Rhythm_Proto_4
{
    //used to generate sequences of notes
    public static class Generation
    {
        private static Random random;
        private static OverflowArray<Note> prevNoteCache;
        private static int chance;
        private static int consecutiveCount;
        private static bool enabledCache;
        private static string keyPressCache;
        private static string[] possibleKeyChoices;
        private static Button genericNote;
        private static Form genericFormGameMain;

        static Generation()
        {
            random = new Random();
            prevNoteCache = new OverflowArray<Note>(4);
            chance = 100;
            consecutiveCount = 0;
            enabledCache = true;
            possibleKeyChoices = new string[] { "W", "A", "S", "D" };

            //these two properties are used to access dimensions from FormGameMain and its controls
            genericNote = FormEmulator.GetNotes()[0];
            genericFormGameMain = FormEmulator.GetFormGameMain();
        }

        //generates a sequence of notes with appropriate parameters (including borders)
        public static void Generate()
        {
            for (int i = 0; i < 16; i++)
            {
                int genX = random.Next(20, genericFormGameMain.Width - genericNote.Width - 20);
                int genY = random.Next(100, genericFormGameMain.Height - genericNote.Height - 20);

                //determines based on a defined probability if the note is to be enabled
                bool isEnabled = IsEnabledRNG();
                bool validGen = false;

                //constant regeneration of coordinates based on parameter checks if note enabled
                //reiterates until a valid generation is achieved
                while (!validGen && isEnabled)
                {
                    validGen = CollisionAndChainCheck(genX, genY);

                    if (!validGen)
                    {
                        genX = random.Next(20, genericFormGameMain.Width - genericNote.Width - 20);
                        genY = random.Next(100, genericFormGameMain.Height - genericNote.Height - 20);
                    }
                }

                //generates a key to be pressed in a separate method
                string keyToPress = KeyPressRNG();

                //changes probability of note being enabled for next generation based on current generation
                IsEnabledChanceModifier(isEnabled);

                //queues the generated note to into appropriate queue structures
                Visuals.EnqueueNote(new Note(genX, genY, isEnabled, i, keyToPress));
                prevNoteCache.Enqueue(new Note(genX, genY, isEnabled, i, keyToPress));
            }
        }

        //determines whether the generated notes is enabled in the sequence
        private static bool IsEnabledRNG()
        {
            if (random.Next(0, 100) < chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //for checking against collision with other added notes and sufficient distance between "chained" notes (see ChainParameter)
        private static bool CollisionAndChainCheck(int x, int y)
        {
            bool valid = CollisionParameter(x, y);

            if (valid)
            {
                valid = ChainParameter(x, y);
            }

            return valid;
        }

        //method for checking if a generated note overlaps with any other visible notes
        private static bool CollisionParameter(int x, int y)
        {
            foreach (Note note in prevNoteCache.GetList())
            {
                if (note != default)
                {
                    int[] diffCoords = DistanceCalculation(note, x, y);

                    if ((diffCoords[0] < genericNote.Width + 10) && (diffCoords[1] < genericNote.Height + 10))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //method for checking for appropriate "chaining" distance where appropriate
        //"chained" notes are notes that are directly adjacent to each other in a sequence
        private static bool ChainParameter(int x, int y)
        {
            CustomStack<Note> noteCacheStack = new CustomStack<Note>();

            foreach (Note note in prevNoteCache.GetList())
            {
                noteCacheStack.Push(note);
            }

            Note lastNote = noteCacheStack.Pop();

            if ((consecutiveCount > 0) && (lastNote != default))
            {
                int[] diffCoords = DistanceCalculation(lastNote, x, y);

                if ((diffCoords[0] > 120) || (diffCoords[1] > 120))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        //method for checking the distance between a note and given coordinates
        private static int[] DistanceCalculation(Note note, int x, int y)
        {
            int xDiff = note.GetCoords()[0] - x;
            int yDiff = note.GetCoords()[1] - y;

            xDiff = Game.MakePositive(xDiff);
            yDiff = Game.MakePositive(yDiff);

            return new int[] { xDiff, yDiff };
        }

        //generates a random key to be pressed as a string, recalling itself if it generates the same key as was just generated
        private static string KeyPressRNG()
        {
            int keyRNG = random.Next(0, 4);

            if (keyPressCache != null)
            {
                if (keyPressCache == possibleKeyChoices[keyRNG])
                {
                    return KeyPressRNG();
                }
                else
                {
                    keyPressCache = possibleKeyChoices[keyRNG];
                    return possibleKeyChoices[keyRNG];
                }
            }
            else
            {
                keyPressCache = possibleKeyChoices[keyRNG];
                return possibleKeyChoices[keyRNG];
            }
        }

        //modifies the probability of IsEnabledRNG() based on consecutive number of true/false outputs from related method
        private static void IsEnabledChanceModifier(bool input)
        {
            if (enabledCache)
            {
                if (input == enabledCache)
                {
                    chance -= ++consecutiveCount * 10;
                }
                else
                {
                    enabledCache = input;
                    consecutiveCount = 0;
                    chance = 25;
                }
            }
            else
            {
                if (input == enabledCache)
                {
                    chance += 25;
                }
                else
                {
                    enabledCache = input;
                    ++consecutiveCount;
                    chance = 90;
                }
            }
        }

        //notes added to every sequence as standard that serves two purposes
        //offsets the visible notes to align properly with the stopwatch and gives the player ample time to ready themselves
        public static void OffsetNotes()
        {
            for (int i = 0; i < 12; i++)
            {
                //position set to -1 so that it does not interfere with other methods ("ghost" notes)
                Visuals.EnqueueNote(new Note(0, 0, false, -1, "W"));
            }
        }

        //resets all relevant properties in the class to new objects ready for next game loop
        public static void Reset()
        {
            prevNoteCache = new OverflowArray<Note>(4);
            chance = 100;
            consecutiveCount = 0;
            enabledCache = true;
            keyPressCache = null;
        }
    }
}
